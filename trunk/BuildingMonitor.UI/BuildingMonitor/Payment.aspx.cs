// Author:					Jose Luis Ferrufino Rivera
// Created:					2010-4-12
// Last Modified:			2010-4-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using BuildingMonitor.Business;
using BuildingMonitor.UI.Helpers;
using mojoPortal.Business.WebHelpers;
using Resources;



namespace BuildingMonitor.UI
{

	public partial class PaymentPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;
		private int _contractId = -1;
		protected bool _isCancel = false;
		private Contract _contract = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			LoadParams();

			// one of these may be usefull
			//if (!UserCanViewPage(moduleId))
			//{
			//    SiteUtils.RedirectToAccessDeniedPage();
			//    return;
			//}
			//if (!UserCanEditModule(moduleId))
			//{
			//    SiteUtils.RedirectToAccessDeniedPage();
			//    return;
			//}

			LoadSettings();
			PopulateLabels();
			PopulateControls();
		}

		private void PopulateControls()
		{
			using (IDataReader reader = Contract.Get(_contractId))
			{
				if (reader.Read())
					ltrCurrency.Text = (string)reader["Currency"];
			}

			if (!IsPostBack)
			{
				txtExchangeRate.Text = Helpers.Formatter.Decimal(Settings.Instance.ExchangeRateBuy);
				PopulateListPaymentFrom();
				PopulateListPaymentTo();
				PopulateTableDetail();
			}
		}

		private void PopulateListPaymentTo()
		{
			int contractorId = Contractor.GetIdFromContract(_contractId);
			
			using (IDataReader reader = Contractor.GetAllAccounts(contractorId))
			{
				while (reader.Read())
				{
					ddlPaymentTo.Items.Add(new ListItem(
						(string)reader["Bank"] + ", " + (string)reader["Account"],
						reader["ContractorAccountId"].ToString()));
				}

				ddlPaymentTo.Items.Insert(0, new ListItem(Resources.BuildingMonitorResources.OptionSelect, ""));
				ddlPaymentTo.Items.Insert(1, new ListItem(Resources.BuildingMonitorResources.LabelCash, "cash"));
				ddlPaymentTo.Items.Insert(1, new ListItem(Resources.BuildingMonitorResources.LabelCheck, "check"));
			}
		}

		private void PopulateListPaymentFrom()
		{
			using (IDataReader reader = Payment.GetAllAccounts())
			{
				while (reader.Read())
				{
					ddlPaymentFrom.Items.Add(new ListItem(
						(string)reader["Bank"] + ", " + (string)reader["Description"],
						(string)reader["Account"]));
				}

				ddlPaymentFrom.Items.Insert(0, new ListItem(Resources.BuildingMonitorResources.OptionSelect, ""));
			}
		}

		private void PopulateTableDetail()
		{
			using (IDataReader reader = SubItem.GetAllPayable(_contractId, -1))
			{
				BuildingComparer previous = BuildingComparer.Empty;
				BuildingComparer current = BuildingComparer.Empty;
				string groupName = string.Empty;
				string itemName = string.Empty;
				string separator = Resources.BuildingMonitorResources.SeparatorLR;

				while (reader.Read())
				{
					TableRow rowHeader = null;
					TableRow rowBody = null;
					TableRow rowFooter = null;
					TableCell cell = null;

					current.Set((int)reader["ProjectId"], (int)reader["BlockId"], (int)reader["WorkId"],
						(int)reader["GroupId"], (int)reader["ItemId"], (int)reader["SubItemId"]);

					if (!current.IsWork(previous))
					{
						rowHeader = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 5;
						cell.CssClass = "bm-group-hd";
						cell.Text = string.Format("{0}: {1}",
							Resources.BuildingMonitorResources.LabelWork,
							Work.Create(current.ProjectId, current.BlockId, current.WorkId).Name);
						rowHeader.Cells.Add(cell);
					}

					if (!current.IsGroup(previous))
					{
						groupName = Group.Create(current.ProjectId, current.BlockId, current.WorkId, current.GroupId).Name;

						if (!current.IsItem(previous))
							itemName = Item.Create(current.ProjectId, current.BlockId, current.WorkId, current.GroupId, current.ItemId).Name;
					}

					rowBody = new TableRow();
					cell = new TableCell();
					cell.Text = "<input type=\"checkbox\" name=\"payable-data\" value=\"" +
						HttpUtility.HtmlEncode(string.Format("{{\"id\":{0},\"amount\":{1}}}", reader["ProgressId"], Helpers.Formatter.Decimal(reader["Amount"]))) +
						"\"/>";
					rowBody.Cells.Add(cell);

					cell = new TableCell();
					cell.Text = string.Format("{0} {1} {2} {1} {3}", groupName, separator, itemName, reader["Name"]);
					rowBody.Cells.Add(cell);

					cell = new TableCell();
					cell.CssClass = "bm-date";
					cell.Text = Helpers.Formatter.ShortDate(reader["Date"]);
					rowBody.Cells.Add(cell);

					cell = new TableCell();
					cell.CssClass = "bm-number";
					cell.Text = reader["CurrentProgress"].ToString() + "%";
					rowBody.Cells.Add(cell);

					cell = new TableCell();
					cell.CssClass = "bm-number progress-payable";
					cell.Text = Helpers.Formatter.Decimal(reader["Amount"]);
					rowBody.Cells.Add(cell);

					if (rowFooter != null)
						tblProgress.Rows.Add(rowFooter);

					if (rowHeader != null)
						tblProgress.Rows.Add(rowHeader);

					tblProgress.Rows.Add(rowBody);
					previous.CopyFrom(current);
				}
			}
		}

		private int Save()
		{
			Payment payment = new Payment();
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string[] payables = Request.Form.GetValues("payable-data");
			int hold = -1;

			if (payables != null)
			{
				int.TryParse(ddlPaymentTo.SelectedValue, out hold);
				payment.Amount = 0;
				payment.CreatedBy = SiteUtils.GetCurrentSiteUser().Name;
				payment.Check = txtCheckNumber.Text;
				payment.ContractorAccount = Contractor.GetIdFromContract(_contractId);
				payment.Currency = ltrCurrency.Text;
				payment.BankAccount = ddlPaymentFrom.SelectedValue;
				payment.ExchangeRate = decimal.Parse(txtExchangeRate.Text, CultureInfo.InvariantCulture);
				payment.ContractorAccount = hold;
				payment.ContractorPaymentType = ddlPaymentTo.SelectedValue;

				foreach (string payableData in payables)
				{
					Dictionary<string, object> data = (Dictionary<string, object>)serializer.DeserializeObject(payableData);

					payment.Amount += Convert.ToDecimal(data["amount"]);
					payment.ProgressIds.Add((int)data["id"]);
				}

				hold = payment.Save();
			}

			return hold;
		}

		private void PopulateLabels()
		{

		}

		private void LoadSettings()
		{
		}

		private void LoadParams()
		{
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
			_contractId = WebUtils.ParseInt32FromQueryString("contractid", _contractId);
			_isCancel = Request.QueryString["cancel"] == "1";
			_contract = Contract.Create(_contractId);
		}


		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);
		}

		#endregion

		protected override void OnPreRender(EventArgs e)
		{
			if (!ClientScript.IsClientScriptIncludeRegistered("BuildingMonitor"))
				ClientScript.RegisterClientScriptInclude(GetType(), "BuildingMonitor", ResolveUrl("ClientScript/main.js"));

			base.OnPreRender(e);
		}

		protected void btnOk_Click(object sender, EventArgs e)
		{
			int paymentId = Save();
			string target = Request.QueryString["target"];
			string successUrl = string.Format("PaymentSuccess.aspx?mid={0}&pageid={1}&contractid={2}&paymentid={3}&target={4}&cancel={5}", _moduleId, _pageId, _contractId, paymentId, target, (_isCancel ? "1" : ""));

			if (paymentId > 0)
			{
				if (_isCancel)
					_contract.UpdateStatus(ContractStatus.Canceled);

				Server.Transfer(successUrl, true);
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			string target = Request.QueryString["target"];

			SiteUtils.RedirectToUrl(string.Format("Contracts.aspx?mid={0}&pageid={1}&target=", _moduleId, _pageId, target));
		}
	}
}
