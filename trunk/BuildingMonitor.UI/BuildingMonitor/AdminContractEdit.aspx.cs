// Author:					Jose Luis Ferrufino Rivera
// Created:					2010-2-8
// Last Modified:			2010-2-8
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
using System.Threading;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using BuildingMonitor.Business;
using mojoPortal.Business.WebHelpers;
using Resources;



namespace BuildingMonitor.UI
{

    public partial class AdminContractEditPage : mojoBasePage
    {
        private int _pageId = -1;
        private int _moduleId = -1;
        private int _contractId = -1;
		
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            // one of these may be usefull
            if (!UserCanViewPage(_moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }
            if (!UserCanEditModule(_moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();
            SetupScripts();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
			if(!IsPostBack)
			{
				ddlContractor.DataSource = Contractor.GetAll();
				ddlContractor.DataBind();
				ddlContractor.Items.Insert(0, new ListItem(BuildingMonitorResources.OptionSelect, ""));
				
				ddlCurrency.DataSource = Currency.GetAllCodes();
				ddlCurrency.DataBind();
			}
        }


        private void PopulateLabels()
        {
            litHeading.Text = BuildingMonitorResources.AdminContractEdit;
            litMainTab.Text = BuildingMonitorResources.LabelData;
            litDescriptionTab.Text = BuildingMonitorResources.LabelDescription;
            litDetailTab.Text = BuildingMonitorResources.LabelDetail;
            litMoreTab.Text = BuildingMonitorResources.LabelMore;
            litDetailNameHead.Text = BuildingMonitorResources.LabelSubItem;
            litDetailQtyHead.Text = BuildingMonitorResources.LabelQuantity;
            litDetailPriceHead.Text = BuildingMonitorResources.LabelPrice;
            btnSave.Text = BuildingMonitorResources.LabelSave;
            btnDelete.Text = BuildingMonitorResources.LabelDelete;
            btnAddSubItem.Text = BuildingMonitorResources.ContractAddSubItem;
        }

        private void LoadSettings()
        {
			btnAddSubItem.OnClientClick = "bmContractEdit.addRow();return false;";
			btnAddSubItem.UseSubmitBehavior = false;
			txtAmount.ReadOnly = true;
			btnDelete.Visible = _contractId > 0;
        }
        
        private void SetupScripts()
        {
	        if (ClientScript.IsClientScriptBlockRegistered("BuildingMonitor"))
				return;
			
			System.Text.StringBuilder script = new System.Text.StringBuilder();
			
			script.Append("<script type='text/javascript'>");
			script.Append("jQuery(document).ready(function() {");
			script.Append("jQuery('#" + txtDateStart.ClientID + "').datepicker({dateFormat:'dd/mm/yy',dayNamesMin:" + Helpers.JQuery.GetDayNamesMin() + ",monthNames:" + Helpers.JQuery.GetMonthNames() + "});");
			script.Append("jQuery('#" + txtDateEnd.ClientID + "').datepicker({dateFormat:'dd/mm/yy',dayNamesMin:" + Helpers.JQuery.GetDayNamesMin() + ",monthNames:" + Helpers.JQuery.GetMonthNames() + "});");
			script.Append("bmContractEdit.initialize('" + ddlSubItems.ClientID + "','" + tblDetailSubItems.ClientID + "','" + BuildingMonitorResources.LabelRemove + "');");
			script.Append("})");
			script.Append("</script>");
			
			ClientScript.RegisterClientScriptInclude("BuildingMonitor", SiteRoot + "/BuildingMonitor/ClientScript/main.js");
			ClientScript.RegisterStartupScript(GetType(), "BuildingMonitor", script.ToString());
        }

        private void LoadParams()
        {
            _pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
            _moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
			_contractId = WebUtils.ParseInt32FromQueryString("contractid", _contractId);
        }

		private void BindSubItemList(int projectId, int blockId, int workId, int groupId, int itemId)
		{
			using (IDataReader reader = SubItem.GetAll(projectId, blockId, workId, groupId, itemId))
			{
				ddlSubItems.Items.Clear();
				
				while(reader.Read())
				{
					string name = reader["Name"].ToString();
					int subitemId = -1;
					double quantity = -1;
					string json = string.Empty;
					
					if (!int.TryParse(reader["SubItemId"].ToString(), out subitemId))
						continue;
					
					if(!double.TryParse(reader["Quantity"].ToString(), out quantity))
						continue;
					
					json = "{\"id\":" +
						"{\"pId\":" + projectId.ToInvariantString() +
						",\"bId\":" + blockId.ToInvariantString() +
						",\"wId\":" + workId.ToInvariantString() +
						",\"gId\":" + groupId.ToInvariantString() +
						",\"iId\":" + itemId.ToInvariantString() +
						",\"sId\":" + subitemId.ToInvariantString() +
						"},\"qty\":" + quantity.ToString(Thread.CurrentThread.CurrentCulture) + "}";
					
					ddlSubItems.Items.Add(new ListItem(name, json));
				}
			}
		}
		
		private bool Save()
		{
			Contract contract = new Contract();
			string[] subitemIds = Request.Form.GetValues("subitem-id");
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			int i = -1;
			
			try
			{
				contract.Start = Convert.ToDateTime(txtDateStart.Text, Thread.CurrentThread.CurrentCulture);
				contract.End = Convert.ToDateTime(txtDateEnd.Text, Thread.CurrentThread.CurrentCulture);
				contract.ExchangeRate = Convert.ToDecimal(txtCurrencyRate.Text, CultureInfo.InvariantCulture);
				contract.ContractorId = Convert.ToInt32(ddlContractor.SelectedValue, CultureInfo.InvariantCulture);	
			}
			catch(Exception)
			{
				return false;
			}
			
			contract.Description = edtDescription.Text;
			contract.Currency = ddlCurrency.SelectedValue;
			contract.CreatedBy = SiteUtils.GetCurrentSiteUser().Name;
			
			foreach(string inputPrice in Request.Form.GetValues("subitem-price"))
			{
				decimal price = 0;
				string jsonIds = null;
				Dictionary<string, object> ids = null;
				ContractDetail detail = null;
				
				i++;
				
				if(!decimal.TryParse(inputPrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price))
					continue;
				
				try
				{
					jsonIds =Server.UrlDecode(subitemIds[i]);
					ids = (Dictionary<string, object>)serializer.DeserializeObject(jsonIds);
					detail = new ContractDetail();
					detail.ProjectId = (int)ids["pId"];
					detail.BlockId = (int)ids["bId"];
					detail.WorkId = (int)ids["wId"];
					detail.GroupId = (int)ids["gId"];
					detail.ItemId = (int)ids["iId"];
					detail.SubItemId = (int)ids["sId"];
					detail.Price = price;
				}
				catch(Exception)
				{
					continue;
				}
				
				contract.Detail.Add(detail);
			}
			
			return contract.Save() > 0;
		}
		
		private void btnSave_Click(object sender, EventArgs e)
		{
			Save();
		}
		
        #region Protected Override Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            this.Load += new EventHandler(this.Page_Load);
			btnSave.Click += new EventHandler(btnSave_Click);
			navProjectToItem.ItemChanged += new global::BuildingMonitor.UI.BuildingMonitor.Controls.NavProjectToItem.ItemEventHandler(navProjectToItem_ItemChanged);
			
			SuppressPageMenu();
			SuppressGoogleAds();
			ScriptConfig.IncludeYuiTabs = true;
			IncludeYuiTabsCss = true;
        }

		private void navProjectToItem_ItemChanged(object sender, int projectId, int blockId, int workId, int groupId, int itemId)
		{
			BindSubItemList(projectId, blockId, workId, groupId, itemId);
		}


		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
			
			SiteUtils.SetupEditor(edtDescription);
		}
		
        #endregion
    }
}