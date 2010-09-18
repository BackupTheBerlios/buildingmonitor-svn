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

	public partial class PaymentPaidWorkPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;
		private int _contractId = -1;
		private int _absoluteProgress = 0;
		private int _progressPlan = 0;
		private DateTime _datePlan;
		private decimal _amountPlan = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			LoadParams();

			// one of these may be usefull
			if (!UserCanViewPage(_moduleId))
			{
				SiteUtils.RedirectToAccessDeniedPage();
				return;
			}
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
					litCurrency.Text = (string)reader["Currency"];
			}

			if (!IsPostBack)
			{
				txtExchangeRate.Text = Helpers.Formatter.Decimal(Settings.Instance.ExchangeRateBuy);
				PopulateListPaymentFrom();
				PopulateListPaymentTo();
			}
			
			PopulateTableDetail();
			PopulatePaymentConditions();
		}

		private void PopulatePaymentConditions()
		{
			btnOk.Enabled = false;
			litDate.Text = Helpers.Formatter.ShortDate(_datePlan);
			litProgressRequired.Text = _progressPlan.ToString() + "%";
			litAmount.Text = Helpers.Formatter.Decimal(_amountPlan);
			btnOk.Enabled = _absoluteProgress >= _progressPlan && _amountPlan > 0;
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
			decimal totalProgress = 0;

			using (IDataReader reader = SubItem.GetAllPayablePaidWork(_contractId))
			{
				BuildingComparer previous = BuildingComparer.Empty;
				BuildingComparer current = BuildingComparer.Empty;
				string groupName = string.Empty;
				string itemName = string.Empty;
				string separator = Resources.BuildingMonitorResources.SeparatorLR;

				while (reader.Read())
				{
					TableRow rowHdBlock = null;
					TableRow rowHdWork = null;
					TableRow rowBody = null;
					TableCell cell = null;

					current.Set((int)reader["ProjectId"], (int)reader["BlockId"], (int)reader["WorkId"],
						(int)reader["GroupId"], (int)reader["ItemId"], (int)reader["SubItemId"]);

					if (!current.IsBlock(previous))
					{
						rowHdBlock = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 5;
						cell.CssClass = "bm-block-hd";
						cell.Text = string.Format("{0}: {1}",
							Resources.BuildingMonitorResources.LabelBlock,
							Block.Create(current.ProjectId, current.BlockId).Name);
						rowHdBlock.Cells.Add(cell);
					}

					if (!current.IsWork(previous))
					{
						rowHdWork = new TableRow();
						cell = new TableCell();
						cell.Text = "&nbsp;";
						rowHdWork.Cells.Add(cell);
						cell = new TableCell();
						cell.ColumnSpan = 4;
						cell.CssClass = "bm-group-hd";
						cell.Text = string.Format("{0}: {1}",
							Resources.BuildingMonitorResources.LabelWork,
							Work.Create(current.ProjectId, current.BlockId, current.WorkId).Name);
						rowHdWork.Cells.Add(cell);
					}

					if (!current.IsGroup(previous))
					{
						groupName = Group.Create(current.ProjectId, current.BlockId, current.WorkId, current.GroupId).Name;

						if (!current.IsItem(previous))
							itemName = Item.Create(current.ProjectId, current.BlockId, current.WorkId, current.GroupId, current.ItemId).Name;
					}

					rowBody = new TableRow();
					cell = new TableCell();
					cell.Text = "&nbsp;";
					rowBody.Cells.Add(cell);
					cell = new TableCell();
					cell.Text = "&nbsp;";
					rowBody.Cells.Add(cell);

					cell = new TableCell();
					cell.Text = string.Format("{0} {1} {2} {1} {3}", groupName, separator, itemName, reader["Name"]);
					rowBody.Cells.Add(cell);

					cell = new TableCell();
					cell.CssClass = "bm-number";
					cell.Text = reader["Progress"].ToString() + "%";
					rowBody.Cells.Add(cell);

					cell = new TableCell();
					cell.CssClass = "bm-number";
					cell.Text = Helpers.Formatter.Decimal(reader["AbsoluteProgress"]) + "%";
					rowBody.Cells.Add(cell);

					if (rowHdBlock != null)
						tblProgress.Rows.Add(rowHdBlock);

					if (rowHdWork != null)
						tblProgress.Rows.Add(rowHdWork);

					tblProgress.Rows.Add(rowBody);
					previous.CopyFrom(current);
					totalProgress +=  Convert.ToDecimal(reader["AbsoluteProgress"]);
				}
			}

			_absoluteProgress = (int)Math.Min(Math.Round(totalProgress), 100);
			litTotalProgress.Text = _absoluteProgress.ToString() + "%";
		}

		private int Save()
		{
			PaymentPaidWork payment = new PaymentPaidWork();
			int hold = -1;

			int.TryParse(ddlPaymentTo.SelectedValue, out hold);
			payment.Amount = _amountPlan;
			payment.ContractId = _contractId;
			payment.ProgressRequired = _progressPlan;
			payment.CreatedBy = SiteUtils.GetCurrentSiteUser().Name;
			payment.Check = txtCheckNumber.Text;
			payment.ContractorAccount = Contractor.GetIdFromContract(_contractId);
			payment.Currency = litCurrency.Text;
			payment.BankAccount = ddlPaymentFrom.SelectedValue;
			payment.ExchangeRate = decimal.Parse(txtExchangeRate.Text, CultureInfo.InvariantCulture);
			payment.ContractorAccount = hold;
			payment.ContractorPaymentType = ddlPaymentTo.SelectedValue;

			return payment.Save();
		}

		private void PopulateLabels()
		{

		}

		private void LoadSettings()
		{
			using (IDataReader reader = Contract.PayablePaidWorkPlan(_contractId))
			{
				if (reader.Read())
				{
					_datePlan = Convert.ToDateTime(reader["Date"]);
					_progressPlan = Convert.ToInt32(reader["ProgressCondition"]);
					_amountPlan = Convert.ToDecimal(reader["Amount"]);
				}
			}
		}

		private void LoadParams()
		{
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
			_contractId = WebUtils.ParseInt32FromQueryString("contractid", _contractId);
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
			
			if (paymentId > 0)
				Server.Transfer(string.Format("PaymentPaidWorkSuccess.aspx?mid={0}&pageid={1}&contractid={2}&paymentid={3}", _moduleId, _pageId, _contractId, paymentId), true);
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			SiteUtils.RedirectToUrl(string.Format("Contracts.aspx?mid={0}&pageid={1}", _moduleId, _pageId));
		}
	}
}
