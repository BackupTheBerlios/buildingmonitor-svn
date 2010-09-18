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

	public partial class PaymentSuccessPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;
		private int _contractId = -1;
		private int _paymentId = -1;
		private List<PaymentSuccesDetail> _detail = null;
		private Payment _payment = null;
		private Contractor _contractor = null;

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
			FillTableDetail();
			SetTotal();
		}


		private void PopulateLabels()
		{
			litHeading.Text = Resources.BuildingMonitorResources.TitlePaymentSuccess + (Request.QueryString["cancel"]=="1" ? " Contrato Cancelado" : "");
			ltrCurrency.Text = _payment.Currency;
			litDate.Text = Helpers.Formatter.ShortDate(_payment.Date);
			litContractorName.Text = ContractorInfo();
			litCompanyName.Text = Settings.Instance.CompanyName;
			btnPrint.Text = Resources.BuildingMonitorResources.LabelPrint;
		}


		private string ContractorInfo()
		{
			string info = string.Empty;
			ContractorBankAccount contractorBankAccount = null;

			info += _contractor.Name + ", ";

			if (_payment.ContractorPaymentType == "check")
				info += Resources.BuildingMonitorResources.LabelCheck + ": " + _payment.Check;
			else if (_payment.ContractorPaymentType == "cash")
				info += Resources.BuildingMonitorResources.LabelCash;
			else
			{
				contractorBankAccount = ContractorBankAccount.Create(_contractor.Id, _payment.ContractorAccount);
				info += contractorBankAccount.Bank + ": " + contractorBankAccount.AccountNumber;
			}

			return info;
		}

		private void SetTotal()
		{
			decimal total = 0;

			foreach (PaymentSuccesDetail data in _detail)
			{
				total += data.Amount;
			}

			ltrTotal.Text = Helpers.Formatter.Decimal(total);
		}

		private void FillTableDetail()
		{
			DataSet paidProgress = Progress.GetPaid(_paymentId);
			DataView dataView = paidProgress.Tables[0].DefaultView;
			BuildingComparer previous = BuildingComparer.Empty;
			BuildingComparer current = BuildingComparer.Empty;
			string groupName = string.Empty;
			string itemName = string.Empty;
			string subItemName = string.Empty;
			string separator = Resources.BuildingMonitorResources.SeparatorLR;

			foreach (PaymentSuccesDetail data in _detail)
			{
				TableRow rowHeader = null;
				TableRow rowBody = null;
				TableRow rowFooter = null;
				TableCell cell = null;

				dataView.RowFilter = string.Format("ProgressId={0}", data.Id);
				
				current.Set((int)dataView[0]["ProjectId"], 
					(int)dataView[0]["BlockId"],
					(int)dataView[0]["WorkId"],
					(int)dataView[0]["GroupId"],
					(int)dataView[0]["ItemId"],
					(int)dataView[0]["SubItemId"]);

				if (!current.IsWork(previous))
				{
					rowHeader = new TableRow();
					cell = new TableCell();
					cell.ColumnSpan = 4;
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
				subItemName = SubItem.Create(current.ProjectId, current.BlockId, current.WorkId, current.GroupId, current.ItemId, current.SubItemId).Name;
				cell.Text = string.Format("{0} {1} {2} {1} {3}", groupName, separator, itemName, subItemName);
				rowBody.Cells.Add(cell);

				cell = new TableCell();
				cell.CssClass = "bm-date";
				cell.Text = Helpers.Formatter.ShortDate(dataView[0]["Date"]);
				rowBody.Cells.Add(cell);

				cell = new TableCell();
				cell.CssClass = "bm-number";
				cell.Text = dataView[0]["CurrentProgress"].ToString() + "%";
				rowBody.Cells.Add(cell);

				cell = new TableCell();
				cell.CssClass = "bm-number progress-payable";
				cell.Text = Helpers.Formatter.Decimal(data.Amount);
				rowBody.Cells.Add(cell);

				if (rowFooter != null)
					tblPaidProgress.Rows.Add(rowFooter);

				if (rowHeader != null)
					tblPaidProgress.Rows.Add(rowHeader);

				tblPaidProgress.Rows.Add(rowBody);
				previous.CopyFrom(current);
			}

			paidProgress.Dispose();
		}

		private void LoadSettings()
		{
			JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
			PaymentSuccesDetail data;
			int contractorId = Contractor.GetIdFromContract(_contractId);
			
			_detail = new List<PaymentSuccesDetail>();

			foreach (string payableData in Request.Form.GetValues("payable-data"))
			{
				data = jsSerializer.Deserialize<PaymentSuccesDetail>(payableData);
				_detail.Add(data);
			}

			_payment = Payment.Create(_paymentId);
			_contractor = Contractor.Create(contractorId);
		}

		private void LoadParams()
		{
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
			_contractId = WebUtils.ParseInt32FromQueryString("contractid", _contractId);
			_paymentId = WebUtils.ParseInt32FromQueryString("paymentid", _paymentId);
		}


		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);
		}

		#endregion

		class PaymentSuccesDetail
		{
			public int Id { get; set; }
			public decimal Amount { get; set; }
		}
	}
}
