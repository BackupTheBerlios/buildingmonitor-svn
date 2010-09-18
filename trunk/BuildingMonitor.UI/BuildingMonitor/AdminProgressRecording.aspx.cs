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

	public partial class AdminProgressRecordingPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;

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
			PopulateLabels();
			PopulateControls();
		}

		private void PopulateControls()
		{
		}


		private void PopulateLabels()
		{
			litHeading.Text = BuildingMonitorResources.ProgressRecAdd;
			btnSave.Text = BuildingMonitorResources.LabelSave;
		}

		private void LoadSettings()
		{
		}

		private void LoadParams()
		{
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
		}

		private void FillContractGrid()
		{
			using (IDataReader reader = ContractDetail.GetAll(ProjectId, BlockId, WorkId, GroupId, ItemId))
			{
				rptContractDetail.DataSource = reader;
				rptContractDetail.DataBind();
			}
		}
		
		private void UpdateIds(int contractId, int projectId, int blockId, int workId, int groupId, int itemId, int subItemId)
		{
			ContractId = contractId;
			ProjectId = projectId;
			BlockId = blockId;
			WorkId = workId;
			GroupId = groupId;
			ItemId = itemId;
			SubItemId = subItemId;
		}

		private void DisplaySavedSuccess()
		{
			pnlSavedSuccess.Visible = true;
			litSavedSuccess.Text = BuildingMonitorResources.ProgressRecSuccessAdd;
		}

		private bool Save(List<Progress> progresses)
		{
			bool isOk = false;
				
			isOk = Progress.Save(progresses, SiteUtils.GetCurrentSiteUser().Name);
			
			return isOk;
		}
		
		private void navProjectToItem_SelectChanged(object sender, int projectId, int blockId, int workId, int groupId, int itemId)
		{
			bool isValidItem = projectId > 0 && blockId > 0 && workId > 0 && groupId > 0 && itemId > 0;

			UpdateIds(-1, projectId, blockId, workId, groupId, itemId, -1);

			if (isValidItem)
			{
				FillContractGrid();
			}

			pnlContractDetail.Visible = isValidItem;
			pnlSavedSuccess.Visible = false;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			List<Progress> progresses = new List<Progress>();

			foreach(RepeaterItem item in rptContractDetail.Items)
			{
				HiddenField hdnData = (HiddenField)item.FindControl("hdnData");
				Dictionary<string, object> data = (Dictionary<string, object>)serializer.DeserializeObject(hdnData.Value);

				if ((int)data["active"] == 0)
					continue;
				if ((int)data["curr"] == (int)data["newp"])
					continue;

				Progress progress = new Progress();

				progress.ContractDetail.ContractId = (int)data["cId"];
				progress.ContractDetail.ProjectId = (int)data["pId"];
				progress.ContractDetail.BlockId = (int)data["bId"];
				progress.ContractDetail.WorkId = (int)data["wId"];
				progress.ContractDetail.GroupId = (int)data["gId"];
				progress.ContractDetail.ItemId = (int)data["iId"];
				progress.ContractDetail.SubItemId = (int)data["sId"];
				progress.Set((int)data["curr"], (int)data["newp"]);
				
				progresses.Add(progress);
			}

			if (!Save(progresses))
				return;

			DisplaySavedSuccess();
			FillContractGrid();
		}

		#region Protected Methods
		
		protected string FillRowData(int contractId, int projectId, int blockId, int workId, int groupId, int itemId, int subItemId)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			
			data.Add("cId", contractId);
			data.Add("pId", projectId);
			data.Add("bId", blockId);
			data.Add("wId", workId);
			data.Add("gId", groupId);
			data.Add("iId", itemId);
			data.Add("sId", subItemId);

			using (IDataReader reader = Progress.GetLast(contractId, projectId, blockId, workId, groupId, itemId, subItemId))
			{
				if (reader.Read())
				{
					data.Add("date", ((DateTime)reader["Date"]).ToString("yyyy-MM-dd"));
					data.Add("init", reader["InitialProgress"]);
					data.Add("curr", reader["CurrentProgress"]);
					data.Add("newp", reader["CurrentProgress"]);
				}
				else
				{
					data.Add("date", "");
					data.Add("init", 0);
					data.Add("curr", 0);
					data.Add("newp", 0);
				}

				data["active"] = 1;
			}

			return serializer.Serialize(data);
		}

		#endregion

		#region Protected Override Methods

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			Load += new EventHandler(Page_Load);
			navProjectToItem.SelectChanged += new global::BuildingMonitor.UI.BuildingMonitor.Controls.NavProjectToItem.SelectEventHandler(navProjectToItem_SelectChanged);
			btnSave.Click += new EventHandler(btnSave_Click);
		}
		

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			
			Helpers.JQuery.Register(this);
				
			if (!ClientScript.IsClientScriptIncludeRegistered("BuildingMonitorLibrary"))
				ClientScript.RegisterClientScriptInclude(GetType(), "BuildingMonitorLibrary", ResolveUrl("ClientScript/library-min.js"));

			if (!ClientScript.IsClientScriptIncludeRegistered("BuildingMonitor"))
				ClientScript.RegisterClientScriptInclude(GetType(), "BuildingMonitor", ResolveUrl("ClientScript/main.js"));
		}
		
		#endregion
		
		#region Public Properties

		public int ContractId
		{
			get
			{
				if (ViewState["ContractId"] is int)
					return (int)ViewState["ContractId"];

				return -1;
			}
			set
			{
				ViewState["ContractId"] = value;
			}
		}
		
		public int ProjectId
		{
			get
			{
				if (ViewState["ProjectId"] is int)
					return (int)ViewState["ProjectId"];
					
				return  -1;
			}
			set
			{
				ViewState["ProjectId"] = value;
			}
		}

		public int BlockId
		{
			get
			{
				if (ViewState["BlockId"] is int)
					return (int)ViewState["BlockId"];

				return -1;
			}
			set
			{
				ViewState["BlockId"] = value;
			}
		}

		public int WorkId
		{
			get
			{
				if (ViewState["WorkId"] is int)
					return (int)ViewState["WorkId"];

				return -1;
			}
			set
			{
				ViewState["WorkId"] = value;
			}
		}

		public int GroupId
		{
			get
			{
				if (ViewState["GroupId"] is int)
					return (int)ViewState["GroupId"];

				return -1;
			}
			set
			{
				ViewState["GroupId"] = value;
			}
		}

		public int ItemId
		{
			get
			{
				if (ViewState["ItemId"] is int)
					return (int)ViewState["ItemId"];

				return -1;
			}
			set
			{
				ViewState["ItemId"] = value;
			}
		}

		public int SubItemId
		{
			get
			{
				if (ViewState["SubItemId"] is int)
					return (int)ViewState["SubItemId"];

				return -1;
			}
			set
			{
				ViewState["SubItemId"] = value;
			}
		}
		
		#endregion
	}
}



