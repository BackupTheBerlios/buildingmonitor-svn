// Author:					Jose Luis Ferrufino Rivera
// Created:					2010-2-11
// Last Modified:			2010-2-11
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

	public partial class AdminProjectPage : mojoBasePage
	{
		private int pageId = -1;
		private int moduleId = -1;

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
			if (!IsPostBack)
			{
				BindGrid();
			}	
		}


		private void PopulateLabels()
		{

		}

		private void LoadSettings()
		{


		}

		private void LoadParams()
		{
			pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
			moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

		}

		private void BindGrid()
		{
			using (DataSet ds = Project.GetAllDataSet())
			{
				m_gridView.DataSource = ds.Tables[0];
				m_gridView.DataBind();
			}
		}
		
		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);

			m_gridView.SelectedIndexChanging += new GridViewSelectEventHandler(m_gridView_SelectedIndexChanging);
			m_gridView.SelectedIndexChanged += new EventHandler(m_gridView_SelectedIndexChanged);
		}

		private void m_gridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
		{
			m_gridView.SelectedIndex = e.NewSelectedIndex;
			BindGrid();
		}

		private void m_gridView_SelectedIndexChanged(object sender, EventArgs e)
		{
			//m_edtId.ReadOnly = false;
			//m_edtId.Text = m_gridView.SelectedRow.Cells[Soporte.Index(m_gridView, "Id") + 1].Text;
			//m_edtNombre.Text = m_gridView.SelectedRow.Cells[Soporte.Index(m_gridView, "Nombre") + 1].Text;
			//m_edtId.ReadOnly = true;
		}

		#endregion
	}
}



