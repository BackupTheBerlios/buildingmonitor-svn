// Author:					Jose Luis Ferrufino Rivera
// Created:					2010-2-4
// Last Modified:			2010-2-4
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
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Text;
using System.Globalization;
using System.Web.Script.Serialization;
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
using UserControls = BuildingMonitor.UI.BuildingMonitor.Controls;
using BuildingMonitor.UI.Helpers;



namespace BuildingMonitor.UI
{

	public partial class ContractAddSuccessPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;
		private int _contractId = -1;
		private List<ContractDetail> _contractDetails = new List<ContractDetail>();

		protected void Page_Load(object sender, EventArgs e)
		{
			LoadParams();

			// one of these may be usefull
			if (!UserCanViewPage(_moduleId))
			{
			    SiteUtils.RedirectToAccessDeniedPage();
			    return;
			}
			//if (!UserCanEditModule(_moduleId))
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
			int contractorId = Contractor.GetIdFromContract(_contractId);
			Contract contract = Contract.Create(_contractId, true);
			
			litContractor.Text = Contractor.Create(contractorId).Name;
			litDateStart.Text = Helpers.Formatter.ShortDate(contract.Start);
			litDateEnd.Text = Helpers.Formatter.ShortDate(contract.End);
			litCurrency.Text = contract.Currency;
			litCurrencyRate.Text = Helpers.Formatter.Decimal(contract.ExchangeRate);
			txtGloss.Text = contract.Description;
			PopulateDetail(contract);
			PopulatePaidWorkPlan();
		}


		private void PopulateLabels()
		{
			litHeading.Text = BuildingMonitorResources.AdminContractEdit;
			litTabMain.Text = BuildingMonitorResources.LabelData;
			litTabDetail.Text = BuildingMonitorResources.LabelDetail;
			litTabPaidWork.Text = BuildingMonitorResources.PaidWork;
		}

		private void LoadSettings()
		{
		}

		private void LoadParams()
		{
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
			_contractId = WebUtils.ParseInt32FromQueryString("contractid", _contractId);
			
		}

		private void PopulateDetail(Contract contract)
		{
			BuildingComparer current = new BuildingComparer();
			BuildingComparer previous = new BuildingComparer();
			string projectName = string.Empty;
			string blockName = string.Empty;
			string workName = string.Empty;
			string groupName = string.Empty;
			string arrow = Resources.BuildingMonitorResources.SeparatorLR;
			TableRow row = null;
			TableCell cell = null;

			foreach (ContractDetail detail in contract.Detail)
			{
				current.Set(detail.ProjectId, detail.BlockId, detail.WorkId, detail.GroupId, detail.ItemId, detail.SubItemId);

				if (!current.IsProject(previous))
					projectName = Project.Create(detail.ProjectId).Name;

				if (!current.IsBlock(previous))
					blockName = Block.Create(detail.ProjectId, detail.BlockId).Name;

				if (!current.IsWork(previous))
					workName = Work.Create(detail.ProjectId, detail.BlockId, detail.WorkId).Name;

				if (!current.IsGroup(previous))
				{
					groupName = Group.Create(detail.ProjectId, detail.BlockId, detail.WorkId, detail.GroupId).Name;
					row = new TableRow();
					row.CssClass = "bm-group-hd";
					cell = new TableCell();
					row.Cells.Add(cell);
					cell.ColumnSpan = 4;
					cell.Text = projectName + arrow + blockName + arrow + workName + arrow + groupName;
					tblContractDetail.Rows.Add(row);
				}
				
				row = new TableRow();
				cell = new TableCell();	
				row.Cells.Add(cell);
				cell.Text = "&nbsp;";
				cell = new TableCell();
				row.Cells.Add(cell);
				cell.Text = detail.Name;
				cell = new TableCell();
				row.Cells.Add(cell);
				cell.Text = Helpers.Formatter.Decimal(detail.Quantity) + " " + detail.Unit;
				cell.CssClass = "bm-number";
				cell = new TableCell();
				row.Cells.Add(cell);
				cell.Text = Helpers.Formatter.Decimal(detail.Price);
				cell.CssClass = "bm-number";
				tblContractDetail.Rows.Add(row);
				previous.CopyFrom(current);
			}

			row = new TableRow();
			row.TableSection = TableRowSection.TableFooter;

			cell = new TableCell();
			cell.ColumnSpan = 3;
			cell.CssClass = "bm-label-total";
			cell.Text = "Total: ";
			row.Cells.Add(cell);

			cell = new TableCell();
			cell.CssClass = "bm-total bm-number";
			cell.Text = Helpers.Formatter.Decimal(contract.Amount);
			row.Cells.Add(cell);

			tblContractDetail.Rows.Add(row);
		}

		private void PopulatePaidWorkPlan()
		{
			using (IDataReader reader = ContractPaidWork.PayablePaidWorkPlan(_contractId))
			{
				if (reader.Read())
				{
				}
			}
		}

		#region Protected Override Methods

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			Load += new EventHandler(Page_Load);
		}


		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			
			Helpers.JQuery.Register(this);
				
			if (!ClientScript.IsClientScriptIncludeRegistered("BuildingMonitorLibrary"))
				ClientScript.RegisterClientScriptInclude(GetType(), "BuildingMonitorLibrary", ResolveUrl("ClientScript/library-min.js"));

			if (!ClientScript.IsClientScriptIncludeRegistered("BuildingMonitor"))
				ClientScript.RegisterClientScriptInclude(GetType(), "BuildingMonitor", ResolveUrl("ClientScript/main.js"));

			ClientScript.RegisterClientScriptInclude(GetType(), "", ResolveUrl("ClientScript/contract-add.js"));
		}
		
		#endregion
	}
}



