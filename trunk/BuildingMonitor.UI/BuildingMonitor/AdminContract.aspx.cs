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
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using BuildingMonitor.Business;
using mojoPortal.Business.WebHelpers;
using Resources;



namespace BuildingMonitor.UI
{

    public partial class AdminContractPage : mojoBasePage
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
            hlkNewContract.NavigateUrl = SiteRoot + "/BuildingMonitor/AdminContractEdit.aspx?pageid=" + _pageId.ToInvariantString() + "&mid=" + _moduleId.ToInvariantString();

        }


        private void PopulateLabels()
        {
            hlkNewContract.Text = BuildingMonitorResources.AdminContractAdd;
        }

        private void LoadSettings()
        {

        }

        private void LoadParams()
        {
            _pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
            _moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);

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