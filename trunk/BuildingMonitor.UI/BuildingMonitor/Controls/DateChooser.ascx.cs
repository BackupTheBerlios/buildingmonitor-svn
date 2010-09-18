using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;

namespace BuildingMonitor.UI.BuildingMonitor.Controls
{
	public partial class DateChooser : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				Value = DateTime.Today;
				
			SetupScripts();
		}
		
		#region Private Methods
		
		private void SetupScripts()
		{
			if (Page.ClientScript.IsClientScriptBlockRegistered("BuildingMonitorLib"))
				return;

			Page.ClientScript.RegisterClientScriptInclude("BuildingMonitorLib", ResolveUrl("../ClientScript/library-min.js"));
		}
		

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
			
			writer.Write("<script type='text/javascript'>");
			writer.Write("jQuery.noConflict();jQuery(document).ready(function(){");
			writer.Write("var input=jQuery('#" + txtDate.ClientID + "');");
			writer.Write("input.datepicker({dateFormat:'dd/mm/yy',dayNamesMin:" + Helpers.JQuery.GetDayNamesMin() + ",monthNames:" + Helpers.JQuery.GetMonthNames() + "});");
			writer.Write("})");
			writer.Write("</script>");
		}
		
		#endregion
		
		#region Public Properties
		
		public DateTime Value
		{
			get
			{
				DateTime date = DateTime.Today;
				
				DateTime.TryParse(txtDate.Text, out date);
				
				return date;
			}
			set
			{
				txtDate.Text = value.ToShortDateString();
			}
		}
		
		#endregion
	}
}