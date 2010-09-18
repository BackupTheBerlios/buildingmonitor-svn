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
using mojoPortal.Business.WebHelpers;
using Resources;



namespace BuildingMonitor.UI
{

	public partial class PaymentPaidWorkSuccessPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;
		private int _contractId = -1;
		private int _paymentId = -1;
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
			using (IDataReader reader = PaymentPaidWork.GetPaidPlan(_contractId, _paymentId))
			{
				if (reader.Read())
				{
					litProgress.Text = reader["ProgressRequired"].ToString() + "%";
				}
			}
		}


		private void PopulateLabels()
		{
			litHeading.Text = Resources.BuildingMonitorResources.TitlePaymentSuccess;
			ltrCurrency.Text = _payment.Currency;
			litDate.Text = Helpers.Formatter.ShortDate(_payment.Date);
			ltrTotal.Text = Helpers.Formatter.Decimal(_payment.Amount);
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


		private void LoadSettings()
		{
			int contractorId = Contractor.GetIdFromContract(_contractId);

			_payment = PaymentPaidWork.Create(_paymentId);
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
	}
}
