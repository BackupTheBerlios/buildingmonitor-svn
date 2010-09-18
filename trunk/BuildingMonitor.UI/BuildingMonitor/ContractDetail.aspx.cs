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
using BuildingMonitor.UI.Helpers;
using mojoPortal.Business.WebHelpers;
using Resources;


namespace BuildingMonitor.UI
{

	public partial class ContractDetailPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;
		private Contract _contract = null;

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
			litContractor.Text = Contractor.Create(_contract.ContractorId).Name;
			litDateStart.Text = Helpers.Formatter.ShortDate(_contract.Start);
			litDateEnd.Text = Helpers.Formatter.ShortDate(_contract.End);
			litCurrency.Text = _contract.Currency;
			litCurrencyRate.Text = Helpers.Formatter.Decimal(_contract.ExchangeRate);
			txtGloss.Text = _contract.Description;
			litTotalContract.Text = Helpers.Formatter.Decimal(_contract.Amount);
			hlkPrint.NavigateUrl = string.Format("ContractDetail.aspx?pageid={0}&mid={1}&contractid={2}&skin=printerfriendly", _pageId, _moduleId, _contract.Id);
			PopulateDetail();
		}


		private void PopulateLabels()
		{
			litHeading.Text = "Contrato " + _contract.Id.ToInvariantString();
			//litTabMain.Text = BuildingMonitorResources.LabelData;
			//litTabDetail.Text = BuildingMonitorResources.LabelDetail;
			btnCancel.Text = "Volver";
			btnCancelContract.Text = "Cancelar Contrato";
		}

		private void LoadSettings()
		{
			btnCancel.UseSubmitBehavior = false;
			btnCancel.PostBackUrl = string.Format("Contracts.aspx?mid={0}&pageid={1}&target=contract", _moduleId, _pageId);
			btnCancelContract.Visible = _contract.Status == ContractStatus.Active;
		}

		private void LoadParams()
		{
			int contractId = -1;

			contractId = WebUtils.ParseInt32FromQueryString("contractid", contractId);
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
			_contract = Contract.Create(contractId, true);
		}

		private void PopulateDetail()
		{
			BuildingComparer prevBc = new BuildingComparer();
			BuildingComparer currBc = new BuildingComparer();
			int counter = 0;
			int rowCounter = 0;

			foreach(ContractDetail detail in _contract.Detail)
			{
				TableRow row = new TableRow();
				TableCell cell = new TableCell();
				
				currBc.Set(detail.ProjectId, detail.BlockId, detail.WorkId, detail.GroupId, detail.ItemId, detail.SubItemId);

				if (!currBc.IsGroup(prevBc))
				{
					cell.Text = string.Format("{1} {0} {2} {0} {3} {0} {4}",
						Resources.BuildingMonitorResources.SeparatorLR,
						Project.Create(detail.ProjectId).Name,
						Block.Create(detail.ProjectId, detail.BlockId).Name,
						Work.Create(detail.ProjectId, detail.BlockId, detail.WorkId).Name,
						Group.Create(detail.ProjectId, detail.BlockId, detail.WorkId, detail.GroupId).Name);
					cell.ColumnSpan = 6;
					row.CssClass = "bm-group-hd";
					row.Cells.Add(cell);
					prevBc.CopyFrom(currBc);
					tblContractDetail.Rows.AddAt(tblContractDetail.Rows.Count - 1, row);

					cell = new TableCell();
					row = new TableRow();
					counter = 1;
				}
				
				cell.CssClass = "bm-number";
				cell.Text = (++rowCounter).ToInvariantString();
				row.Cells.Add(cell);

				cell = new TableCell();
				cell.Text = detail.Name;
				row.TableSection = TableRowSection.TableBody;
				row.Cells.Add(cell);

				cell = new TableCell();
				cell.CssClass = "bm-number";
				cell.Text = Helpers.Formatter.Decimal(detail.Quantity) + " " + detail.Unit;
				row.Cells.Add(cell);

				cell = new TableCell();
				cell.CssClass = "bm-number";
				cell.Text = detail.InitialProgress.ToInvariantString() + "%";
				row.Cells.Add(cell);

				cell = new TableCell();
				cell.CssClass = "bm-number";
				cell.Text = Helpers.Formatter.Decimal(detail.Price);
				row.Cells.Add(cell);

				row.CssClass = counter++ % 2 == 0 ? "alternate" : string.Empty;
				tblContractDetail.Rows.AddAt(tblContractDetail.Rows.Count - 1, row);
			}
		}

		private void btnCancelContract_Click(object sender, EventArgs e)
		{
			CancelContract();
		}

		private void CancelContract()
		{
			bool hasPayableSubItems = false;
			
			using (DataSet payables = _contract.GetPayableSubItems())
			{
				hasPayableSubItems = payables.Tables[0].Rows.Count > 0;
			}

			if (hasPayableSubItems)
				Response.Redirect(string.Format("Payment.aspx?pageid={0}&mid={1}&contractid={2}&target=contract&cancel=1", _pageId, _moduleId, _contract.Id));
			else if (_contract.UpdateStatus(ContractStatus.Canceled))
				Response.Redirect(string.Format("ContractCanceledSuccess.aspx?pageid={0}&mid={1}&contractid={2}&target=contract", _pageId, _moduleId, _contract.Id));
		}

		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);
			btnCancelContract.Click += new EventHandler(btnCancelContract_Click);
		}

		#endregion

		
	}
}



