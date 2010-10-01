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

	public partial class ContractAddPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;
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
			if (!IsPostBack)
			{
				using (IDataReader reader =  Contractor.GetAll())
				{
					ddlContractor.DataSource = reader;
					ddlContractor.DataBind();
				}
				
				ddlContractor.Items.Insert(0, new ListItem(BuildingMonitorResources.OptionSelect, ""));

				using (IDataReader reader = Contractor.GetAll())
				{
					ddlCurrency.DataSource = Currency.GetAllCodes();
					ddlCurrency.DataBind();
				}

				txtCurrencyRate.Text = Helpers.Formatter.Decimal(Settings.Instance.ExchangeRateBuy);
			}
			/*else
			{
				// Detail				
				foreach (ContractDetail detail in _contractDetails)
				{
					TableRow row = new TableRow();
					TableCell cell = new TableCell();
					string ids = string.Format("{0},{1},{2},{3},{4},{5}", detail.ProjectId, detail.BlockId, detail.WorkId, detail.GroupId, detail.ItemId, detail.SubItemId);

					using (IDataReader reader = SubItem.Get(detail.ProjectId, detail.BlockId, detail.WorkId, detail.GroupId, detail.ItemId, detail.SubItemId, ddlCurrency.SelectedValue))
					{
						if (reader.Read())
						{
							cell.Text = "<input type=\"checkbox\"/><input type=\"hidden\" value=\"" + ids + "\" name=\"subitem-data\"/>";
							row.Cells.Add(cell);

							cell = new TableCell();
							cell.Text = (string)reader["Name"];
							row.Cells.Add(cell);

							cell = new TableCell();
							cell.CssClass = "bm-number";
							cell.Text = Helpers.Formatter.Decimal(reader["UnitPrice"]);
							row.Cells.Add(cell);

							cell = new TableCell();
							cell.CssClass = "bm-number";
							cell.Text = Helpers.Formatter.Decimal(reader["Quantity"]) + " " + (string)reader["Unit"];
							row.Cells.Add(cell);

							//cell = new TableCell();
							//cell.CssClass = "bm-number";
							//cell.Text = reader["CurrentProgress"].ToString() + " %";
							//row.Cells.Add(cell);

							cell = new TableCell();
							cell.CssClass = "bm-number";
							cell.Text = Helpers.Formatter.Decimal(reader["TotalPrice"]);
							row.Cells.Add(cell);

							cell = new TableCell();
							cell.CssClass = "bm-number";
							cell.Text = "<input type=\"text\" name=\"subitem-price-total\" value=\"" + Helpers.Formatter.Decimal(detail.Price) + "\" size=\"12\"/>";
							row.Cells.Add(cell);
						}
					}

					tblContractDetail.Rows.AddAt(tblContractDetail.Rows.Count-1, row);
				}

				// PaidWork
				string[] paymentDates = Request.Form.GetValues("payment-date");
				string[] paymentAmounts = Request.Form.GetValues("payment-amount");
				string[] paymentConditions = Request.Form.GetValues("payment-condition");

				if (paymentDates != null && paymentAmounts != null && paymentConditions != null)
				{
					for (int i = 0; i < paymentDates.Length; i++)
					{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();

						cell.Text = "<input type='checkbox' />";
						row.Cells.Add(cell);

						cell = new TableCell();
						cell.CssClass = "bm-number";
						cell.Text = string.Format("<input type='text' value='{0}' name='payment-date'/>", paymentDates[i]);
						row.Cells.Add(cell);

						cell = new TableCell();
						cell.CssClass = "bm-number";
						cell.Text = string.Format("<input type='text' value='{0}' name='payment-amount'/>", paymentAmounts[i]);
						row.Cells.Add(cell);

						cell = new TableCell();
						cell.CssClass = "bm-progress";
						cell.Text = "<span class='bm-progress-label'></span><div class='bm-progress-slider'></div>" + string.Format("<input type='hidden' value='{0}' name='payment-condition'/>", paymentConditions[i]);
						row.Cells.Add(cell);

						tblPayment.Rows.Add(row);
					}
				}
			}*/

			navProjectToItem.SetAllowLinkButton(UserControls.NavProjectToItem.NavItem.Group, UserControls.NavProjectToItem.NavItem.Item);
		}


		private void PopulateLabels()
		{
			litHeading.Text = "Nuevo Contrato";
			litTabMain.Text = BuildingMonitorResources.LabelData;
			//litTabDescription.Text = BuildingMonitorResources.LabelDescription;
			litTabDetail.Text = BuildingMonitorResources.LabelDetail;
			litTabPaidWork.Text = BuildingMonitorResources.PaidWork;
			//litTabMore.Text = BuildingMonitorResources.LabelMore;
			//litDetailNameHead.Text = BuildingMonitorResources.LabelSubItem;
			//litDetailQtyHead.Text = BuildingMonitorResources.LabelQuantity;
			//litDetailPriceHead.Text = BuildingMonitorResources.LabelPrice;
			btnSave.Text = BuildingMonitorResources.LabelSave;
			//btnDelete.Text = BuildingMonitorResources.LabelDelete;
			//btnAddSubItem.Text = BuildingMonitorResources.ContractAddSubItem;
		}

		private void LoadSettings()
		{
		}

		private void LoadParams()
		{
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);

			if (!IsPostBack)
				return;
			
			JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
			string[] rows = Request.Form.GetValues("subitem-data");
			string[] totals = Request.Form.GetValues("subitem-price-total");
			
			if (rows != null && totals != null)
			{
				for (int i = 0; i < rows.Length; i++)
				{
					ListDictionary dataRow = null;
					ContractDetail detail = new ContractDetail();

					try
					{
						dataRow = jsSerializer.Deserialize<ListDictionary>(rows[i]);
						detail.ProjectId = Convert.ToInt32(dataRow["pId"]);
						detail.BlockId = Convert.ToInt32(dataRow["bId"]);
						detail.WorkId = Convert.ToInt32(dataRow["wId"]);
						detail.GroupId = Convert.ToInt32(dataRow["gId"]);
						detail.ItemId = Convert.ToInt32(dataRow["iId"]);
						detail.SubItemId = Convert.ToInt32(dataRow["sId"]);
						detail.InitialProgress = Convert.ToInt32(dataRow["progress"]);
						detail.Price = decimal.Parse(totals[i], CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
						continue;
					}

					_contractDetails.Add(detail);
				}
			}
		}


		private int Save()
		{
			Contract contract = null;
			int i = 0;

			if (chkIsPaidWork.Checked)
				contract = new ContractPaidWork();
			else
				contract = new Contract();

			try
			{
				contract.Start = Convert.ToDateTime(txtDateStart.Text, CultureInfo.CurrentCulture);
				contract.End = Convert.ToDateTime(txtDateEnd.Text, CultureInfo.CurrentCulture);
				contract.ExchangeRate = Convert.ToDecimal(txtCurrencyRate.Text, CultureInfo.InvariantCulture);
				contract.ContractorId = Convert.ToInt32(ddlContractor.SelectedValue, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				return -1;
			}
			
			contract.Currency = ddlCurrency.SelectedValue;
			contract.CreatedBy = SiteUtils.GetCurrentSiteUser().Name;
			contract.Description = txtGloss.Text;
			contract.Detail = _contractDetails;

			if (chkIsPaidWork.Checked)
			{
				ContractPaidWork contractPaidWork = (ContractPaidWork)contract;
				string[] paymentDates = Request.Form.GetValues("payment-date");
				string[] paymentAmounts = Request.Form.GetValues("payment-amount");
				string[] paymentConditions = Request.Form.GetValues("payment-condition");

				contractPaidWork.Advance = double.Parse(txtAdvance.Text, CultureInfo.InvariantCulture);

				for (i = 0; i < paymentDates.Length; i++)
				{
					try
					{
						ContractPaymentPlan plan = new ContractPaymentPlan();

						plan.Amount = Convert.ToDouble(paymentAmounts[i], CultureInfo.InvariantCulture);
						plan.Condition = Convert.ToInt32(paymentConditions[i], CultureInfo.InvariantCulture);
						plan.Date = Convert.ToDateTime(paymentDates[i], CultureInfo.CurrentCulture);

						contractPaidWork.PaymentPlan.Add(plan);
					}
					catch (Exception)
					{
						return -1;
					}
				}
			}

			return contract.Save();
		}


		private void BindGridContractDetail(int projectId, int blockId, int workId, int groupId, int itemId, string currencyCode)
		{			
			using (IDataReader reader = SubItem.GetAllForContract(projectId, blockId, workId, groupId, itemId, currencyCode))
			{
				HierarchicalSerializer hSerializer = new HierarchicalSerializer();
                
				hSerializer.Read(hdnDataRef.Value);
				
				while (reader.Read())
				{
					hSerializer.AddEntry(reader);
				}

				hdnDataRef.Value = hSerializer.ToString();
			}
		}

	
		private void navProjectToItem_ClickLinkItem(object sender, UserControls.NavProjectToItem.NavItem item)
		{
			int[] selectedValues = navProjectToItem.GetSelectedValue();

			BindGridContractDetail(selectedValues[(int)UserControls.NavProjectToItem.NavItem.Project],
			    selectedValues[(int)UserControls.NavProjectToItem.NavItem.Block],
			    selectedValues[(int)UserControls.NavProjectToItem.NavItem.Work],
			    selectedValues[(int)UserControls.NavProjectToItem.NavItem.Group],
			    selectedValues[(int)UserControls.NavProjectToItem.NavItem.Item],
			    ddlCurrency.SelectedValue);
		}


		private void btnSave_Click(object sender, EventArgs e)
		{
			int savedId = Save();

			if (savedId > 0)
			{
				Server.Transfer(string.Format("ContractDetail.aspx?mid={0}&pageid={1}&contractid={2}&target=contract",_moduleId, _pageId, savedId), true);
				litTraceSuccess.Text = "<span class='ui-icon ui-icon-check' style='float:left;margin-right:0.3em;'></span>Contrato Guardado exitosamente";
				btnSave.Visible = false;
			}
			else
				litTraceError.Text = "<span class='ui-icon ui-icon-alert' style='float:left;margin-right:0.3em;'></span>Error, verifique los datos introducidos.";
		}

		#region Protected Override Methods

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			navProjectToItemPath.SetSource(navProjectToItem);
			Load += new EventHandler(Page_Load);
			navProjectToItem.ClickLinkItem += new UserControls.NavProjectToItem.ClickEventHandler(navProjectToItem_ClickLinkItem);
			btnSave.Click += new EventHandler(btnSave_Click);
			//ddlCurrency.SelectedIndexChanged += new EventHandler(ddlCurrency_SelectedIndexChanged);
			lnkRemoveRow.Click += new EventHandler(lnkRemoveRow_Click);
		}

		private void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
		{
			int[] selectedValues = navProjectToItem.GetSelectedValue();

			BindGridContractDetail(selectedValues[(int)UserControls.NavProjectToItem.NavItem.Project],
				selectedValues[(int)UserControls.NavProjectToItem.NavItem.Block],
				selectedValues[(int)UserControls.NavProjectToItem.NavItem.Work],
				selectedValues[(int)UserControls.NavProjectToItem.NavItem.Group],
				selectedValues[(int)UserControls.NavProjectToItem.NavItem.Item],
				ddlCurrency.SelectedValue);
		}

		private void lnkRemoveRow_Click(object sender, EventArgs e)
		{
			string[] subitemsId = Request.Form.GetValues("bmRowChecked");

			if (subitemsId == null)
				return;

			HierarchicalSerializer hSerializer = new HierarchicalSerializer();
			JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
			BuildingComparer bc = BuildingComparer.Empty;

			hSerializer.Read(hdnDataRef.Value);

			foreach (string ids in subitemsId)
			{
				ListDictionary dataRow = jsSerializer.Deserialize<ListDictionary>(ids);
				
				bc.Set(Convert.ToInt32(dataRow["pId"]),
					Convert.ToInt32(dataRow["bId"]),
					Convert.ToInt32(dataRow["wId"]),
					Convert.ToInt32(dataRow["gId"]),
					Convert.ToInt32(dataRow["iId"]),
					Convert.ToInt32(dataRow["sId"]));

				hSerializer.RemoveEntry(bc);
			}
			
			hdnDataRef.Value = hSerializer.ToString();
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



