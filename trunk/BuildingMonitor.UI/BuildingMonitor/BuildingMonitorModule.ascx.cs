// Author:					Jose Luis Ferrufino Rivera
// Created:					2010-2-1
// Last Modified:			2010-2-1
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
using System.Data;
using System.Configuration;
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
using Resources;

namespace BuildingMonitor.UI
{

	public partial class BuildingMonitorModule : SiteModuleControl
	{
		// FeatureGuid 55116935-e907-459c-91ab-47e21970cd97

		#region OnInit

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(Page_Load);

		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			LoadSettings();
			PopulateLabels();
			PopulateControls();
		}

		private void PopulateControls()
		{
			string paramsGet = string.Format("?pageid={0}&mid={1}", PageId, ModuleId);
			
			TitleControl.EditUrl = SiteRoot + "/BuildingMonitor/AdminDashboard.aspx";
			TitleControl.Visible = !this.RenderInWebPartMode;
			
			if (this.ModuleConfiguration != null)
			{
				this.Title = this.ModuleConfiguration.ModuleTitle;
				this.Description = this.ModuleConfiguration.FeatureName;
			}

            hplProgressRecording.NavigateUrl = ResolveUrl("ProgressRecordingEdit.aspx") + paramsGet;
            hplContract.NavigateUrl = "Contracts.aspx" + paramsGet + "&target=contract";
			hplPayment.NavigateUrl = "Contracts.aspx" + paramsGet + "&target=payment";
			hplAnalisisRiesgo.NavigateUrl = "AnalisisRiesgo.aspx" + paramsGet;
			hplResumenRiesgo.NavigateUrl = "ResumenRiesgo.aspx" + paramsGet;
		}


		private void PopulateLabels()
		{
			TitleControl.EditText = "Administraci&oacute;n";
            hplProgressRecording.Text = BuildingMonitorResources.ProgressRec;
            hplContract.Text = "Contratos";
			hplPayment.Text = "Pagos";
			hplAnalisisRiesgo.Text = "Analisis de Riesgo";
			hplResumenRiesgo.Text = "Resumen de Analisis de Riesgo";
		}

		private void LoadSettings()
		{


		}


	}
}



