using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace BuildingMonitor.UI.BuildingMonitor.Controls
{
	public partial class NavProjectToItemPath : System.Web.UI.UserControl
	{
		private NavProjectToItem _source;

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void SetSource(NavProjectToItem source)
		{
			_source = source;
			_source.SelectChanged += new NavProjectToItem.SelectEventHandler(Source_SelectChanged);
		}

		private void Source_SelectChanged(object sender, int projectId, int blockId, int workId, int groupId, int itemId)
		{
			string path = string.Empty;

			if (projectId > 0)
			{
				path += string.Format("{0} {1} ", _source.GetSelectedText(NavProjectToItem.NavItem.Project), BuildingMonitorResources.SeparatorLR);

				if (blockId > 0)
				{
					path += string.Format("{0} {1} ", _source.GetSelectedText(NavProjectToItem.NavItem.Block), BuildingMonitorResources.SeparatorLR);

					if (workId > 0)
					{
						path += string.Format("{0} {1} ", _source.GetSelectedText(NavProjectToItem.NavItem.Work), BuildingMonitorResources.SeparatorLR);

						if (groupId > 0)
						{
							path += string.Format("{0} {1} ", _source.GetSelectedText(NavProjectToItem.NavItem.Group), BuildingMonitorResources.SeparatorLR);

							if (itemId > 0)
								path += string.Format("{0} {1} ", _source.GetSelectedText(NavProjectToItem.NavItem.Item), BuildingMonitorResources.SeparatorLR);
						}
					}
				}
			}

			if (path.EndsWith(BuildingMonitorResources.SeparatorLR + " "))
				path = path.Substring(0, path.Length - (BuildingMonitorResources.SeparatorLR.Length + " ".Length));

			litPath.Text = path;
		}
	}
}