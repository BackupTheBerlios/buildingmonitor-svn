// Author:					Jose Luis Ferrufino Rivera
// Created:					2010-4-8
// Last Modified:			2010-4-8
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
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using BuildingMonitor.Business;
using mojoPortal.Business.WebHelpers;
using Resources;



namespace BuildingMonitor.UI
{

	public partial class ContractsPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;
		private string _target = string.Empty;
		private bool _isPaidWork = false;

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
			hplNewContract.NavigateUrl = string.Format("ContractAdd.aspx?mid={0}&pageid={1}", _moduleId, _pageId);

			if (!IsPostBack)
			{
				using (IDataReader reader = Contractor.GetAll())
				{
					ddlContractor.DataSource = reader;
					ddlContractor.DataBind();
				}

				ddlContractor.Items.Insert(0, new ListItem(BuildingMonitorResources.OptionSelect, ""));

				BindGrid();
			}
		}


		private void PopulateLabels()
		{
			litHeading.Text = "Contratos vigentes";
			hplNewContract.Text = "Crear Nuevo Contrato";
		}

		private void LoadSettings()
		{
			_isPaidWork = chkIsPaidWork.Checked;
		}

		private void LoadParams()
		{
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
			_target = Request.QueryString["target"] == null ? string.Empty : Request.QueryString["target"].ToLower();
		}

		private void BindGrid()
		{
			int contractorId = -1;

			int.TryParse(ddlContractor.SelectedValue, out contractorId);

			using (DataSet dataset = Contract.GetAll(contractorId, _isPaidWork, ContractStatus.Active))
			{
				mgvContracts.DataSource = dataset;
				mgvContracts.DataBind();
			}
		}

		private void mgvContracts_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			BindGrid();
			mgvContracts.PageIndex = e.NewPageIndex;
			mgvContracts.DataBind();
		}

		private void btnFilter_Click(object sender, EventArgs e)
		{
			BindGrid();
		}

		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);
			btnFilter.Click += new EventHandler(btnFilter_Click);
			mgvContracts.PageIndexChanging += new GridViewPageEventHandler(mgvContracts_PageIndexChanging);
		}

		#endregion

		protected string BuildCommandUrl(object contractId)
		{
			string url = "{0}.aspx?pageid={1}&mid={2}&contractid={3}";
			int cId = contractId == null ? -1 : Convert.ToInt32(contractId);

			switch (_target)
			{
				case "payment":
					return string.Format(url, (_isPaidWork ? "PaymentPaidWork" : "Payment"), _pageId, _moduleId, cId);
				case "contract":
					return string.Format(url, "ContractDetail", _pageId, _moduleId, cId);	
			}

			return "#";
		}

		protected string BuildCommandLabel()
		{
			switch (_target)
			{
				case "payment":
					return BuildingMonitorResources.LabelPay;
				case "contract":
					return BuildingMonitorResources.LabelData;	
			}

			return string.Empty;
		}
	}
}



