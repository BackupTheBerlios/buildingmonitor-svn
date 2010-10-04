using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using mojoPortal.Web;
using BuildingMonitor.Business;
using Resources;

namespace BuildingMonitor.UI.BuildingMonitor.Controls
{
	public partial class NavProjectToItem : System.Web.UI.UserControl
	{
		/// <summary>
		/// 
		/// </summary>
		public enum NavItem
		{
			/// <summary>
			/// 
			/// </summary>
			None = -1,
			/// <summary>
			/// 
			/// </summary>
			Project = 0,
			/// <summary>
			/// 
			/// </summary>
			Block = 1,
			/// <summary>
			/// 
			/// </summary>
			Work = 2,
			/// <summary>
			/// 
			/// </summary>
			Group = 3,
			/// <summary>
			/// 
			/// </summary>
			Item = 4
		}

		private DropDownList[] _dropDownLists = null;
		private LinkButton[] _linkButtonsR = null;
		private NavItem _allowDropDownListFrom = NavItem.Project;
		private NavItem _allowDropDownListTo = NavItem.Item;
		private NavItem _allowLinkButtonFrom = NavItem.None;
		private NavItem _allowLinkButtonTo = NavItem.None;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				using (IDataReader reader = Project.GetAll())
				{
					ddlProject.DataSource = reader;
					ddlProject.DataBind();
					ddlProject.Items.Insert(0, new ListItem(BuildingMonitorResources.OptionSelect, ""));
				}

				ddlBlock.Items.Clear();
				ddlBlock.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
				ddlWork.Items.Clear();
				ddlWork.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
				ddlGroup.Items.Clear();
				ddlGroup.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
				ddlItem.Items.Clear();
				ddlItem.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			}
			
			PopulateLabels();
			SetupControls();
		}

		
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			InitArrays();

			ddlProject.SelectedIndexChanged += new EventHandler(ddlProject_SelectedIndexChanged);
			ddlBlock.SelectedIndexChanged += new EventHandler(ddlBlock_SelectedIndexChanged);
			ddlWork.SelectedIndexChanged += new EventHandler(ddlWork_SelectedIndexChanged);
			ddlGroup.SelectedIndexChanged += new EventHandler(ddlGroup_SelectedIndexChanged);
			ddlItem.SelectedIndexChanged += new EventHandler(ddlItem_SelectedIndexChanged);

			for (int i = 0; i < _linkButtonsR.Length; i++)
			{
				_linkButtonsR[i].CommandName = "Click";
				_linkButtonsR[i].CommandArgument = i.ToString();
				_linkButtonsR[i].Click += new EventHandler(NavProjectToItem_Click);
			}
		}

		private void NavProjectToItem_Click(object sender, EventArgs e)
		{
			LinkButton lbt = (LinkButton)sender;
			int item = -1;

			SetupControls();

			if (int.TryParse(lbt.CommandArgument, out item))
			{
				OnClickLinkItem((NavItem)item);
			}
		}


		#region Private Methods

		private void InitArrays()
		{
			_dropDownLists = new DropDownList[5];
			_dropDownLists[(int)NavItem.Project] = ddlProject;
			_dropDownLists[(int)NavItem.Block] = ddlBlock;
			_dropDownLists[(int)NavItem.Work] = ddlWork;
			_dropDownLists[(int)NavItem.Group] = ddlGroup;
			_dropDownLists[(int)NavItem.Item] = ddlItem;

			_linkButtonsR = new LinkButton[5];
			_linkButtonsR[(int)NavItem.Project] = lbtProjectR;
			_linkButtonsR[(int)NavItem.Block] = lbtBlockR;
			_linkButtonsR[(int)NavItem.Work] = lbtWorkR;
			_linkButtonsR[(int)NavItem.Group] = lbtGroupR;
			_linkButtonsR[(int)NavItem.Item] = lbtItemR;
		}

		private void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
		{
			int projectId = -1;

			if (string.IsNullOrEmpty(ddlProject.SelectedValue))
			{
				ddlBlock.Items.Clear();
				ddlBlock.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			}
			else if (int.TryParse(ddlProject.SelectedValue, out projectId))
			{
				using (IDataReader reader = Block.GetAll(projectId))
				{
					ddlBlock.DataSource = reader;
					ddlBlock.DataBind();
					ddlBlock.Items.Insert(0, new ListItem(BuildingMonitorResources.OptionSelect, ""));
				}
			}

			ddlWork.Items.Clear();
			ddlWork.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			ddlGroup.Items.Clear();
			ddlGroup.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			ddlItem.Items.Clear();
			ddlItem.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			OnSelectChanged(projectId, -1, -1, -1, -1);
		}

		private void ddlBlock_SelectedIndexChanged(object sender, EventArgs e)
		{
			int projectId = -1;
			int blockId = -1;

			if (string.IsNullOrEmpty(ddlBlock.SelectedValue))
			{
				ddlWork.Items.Clear();
				ddlWork.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			}
			else if (int.TryParse(ddlProject.SelectedValue, out projectId) && 
					int.TryParse(ddlBlock.SelectedValue, out blockId))
			{
				using (IDataReader reader = Work.GetAll(projectId, blockId))
				{
					ddlWork.DataSource = reader;
					ddlWork.DataBind();
					ddlWork.Items.Insert(0, new ListItem(BuildingMonitorResources.OptionSelect, ""));
				}
			}

			ddlGroup.Items.Clear();
			ddlGroup.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			ddlItem.Items.Clear();
			ddlItem.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			OnSelectChanged(projectId, blockId, -1, -1, -1);
		}

		private void ddlWork_SelectedIndexChanged(object sender, EventArgs e)
		{
			int projectId = -1;
			int blockId = -1;
			int workId = -1;

			if (string.IsNullOrEmpty(ddlWork.SelectedValue))
			{
				ddlGroup.Items.Clear();
				ddlGroup.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			}
			else if (int.TryParse(ddlProject.SelectedValue, out projectId) &&
					int.TryParse(ddlBlock.SelectedValue, out blockId) &&
					int.TryParse(ddlWork.SelectedValue, out workId))
			{
				using (IDataReader reader = Group.GetAll(projectId, blockId, workId))
				{
					ddlGroup.DataSource = reader;
					ddlGroup.DataBind();
					ddlGroup.Items.Insert(0, new ListItem(BuildingMonitorResources.OptionSelect, ""));
				}
			}
			
			ddlItem.Items.Clear();
			ddlItem.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			OnSelectChanged(projectId, blockId, workId, -1, -1);
		}

		private void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
		{
			int projectId = -1;
			int blockId = -1;
			int workId = -1;
			int groupId = -1;

			if (string.IsNullOrEmpty(ddlGroup.SelectedValue))
			{
				ddlItem.Items.Clear();
				ddlItem.Items.Add(new ListItem(BuildingMonitorResources.OptionEmpty, ""));
			}
			else if (int.TryParse(ddlProject.SelectedValue, out projectId) &&
					int.TryParse(ddlBlock.SelectedValue, out blockId) &&
					int.TryParse(ddlWork.SelectedValue, out workId) &&
					int.TryParse(ddlGroup.SelectedValue, out groupId))
			{
				using (IDataReader reader = Item.GetAll(projectId, blockId, workId, groupId))
				{
					ddlItem.DataSource = reader;
					ddlItem.DataBind();
					ddlItem.Items.Insert(0, new ListItem(BuildingMonitorResources.OptionSelect, ""));
				}	
			}

			OnSelectChanged(projectId, blockId, workId, groupId, -1);
		}

		private void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			int projectId = -1;
			int blockId = -1;
			int workId = -1;
			int groupId = -1;
			int itemId = -1;

			int.TryParse(ddlProject.SelectedValue, out projectId);
			int.TryParse(ddlBlock.SelectedValue, out blockId);
			int.TryParse(ddlWork.SelectedValue, out workId);
			int.TryParse(ddlGroup.SelectedValue, out groupId);
			int.TryParse(ddlItem.SelectedValue, out itemId);
			
			OnSelectChanged(projectId, blockId, workId, groupId, itemId);
			OnItemChanged(projectId, blockId, workId, groupId, itemId);					
		}

		private void SetupControls()
		{
			int i = 0;
			int[] selectedValues = GetSelectedValue();

			for (i = 0; i < _linkButtonsR.Length; i++)
			{
				_linkButtonsR[i].Visible = i >= (int)_allowLinkButtonFrom && i <= (int)_allowLinkButtonTo && selectedValues[i] > 0;
			}

			for (i = 0; i < _dropDownLists.Length; i++)
			{
				_dropDownLists[i].Visible = i >= (int)_allowDropDownListFrom && i <= (int)_allowDropDownListTo;
			}
		}

		private void PopulateLabels()
		{
			litProject.Text = BuildingMonitorResources.LabelProject;
			litBlock.Text = BuildingMonitorResources.LabelBlock;
			litWork.Text = BuildingMonitorResources.LabelWork;
			litGroup.Text = BuildingMonitorResources.LabelGroup;
			litItem.Text = BuildingMonitorResources.LabelItem;
		}

		#endregion

		#region Public Methods

		public int GetSelectedValue(NavItem item)
		{
			int i = (int)item;
			int value = -1;

			if (i < 0 || i >= _dropDownLists.Length)
				return value;

			int.TryParse(_dropDownLists[i].SelectedValue, out value);

			return value;
		}

		public int[] GetSelectedValue()
		{
			int length = _dropDownLists.Length;
			int[] selected = new int[length];

			for (int i = 0; i < length; i++)
			{
				selected[i] = -1;
				int.TryParse(_dropDownLists[i].SelectedValue, out selected[i]);
			}

			return selected;
		}

		public string GetSelectedText(NavItem item)
		{
			int i = (int)item;

			if (i < 0 || i >= _dropDownLists.Length)
				return string.Empty;

			return _dropDownLists[i].SelectedItem.Text;
		}

		public string[] GetSelectedText()
		{
			int length = _dropDownLists.Length;
			string[] selected = new string[length];

			for (int i=0; i<_dropDownLists.Length; i++)
			{
				selected[i] = _dropDownLists[i].SelectedItem.Text;
			}
			
			return selected;
		}

		public void SetAllowLinkButton(NavItem from, NavItem to)
		{
			_allowLinkButtonFrom = from;
			_allowLinkButtonTo = to;
		}

		public void SetAllowDropDownList(NavItem from, NavItem to)
		{
			_allowDropDownListFrom = from;
			_allowDropDownListTo = to;
		}

		#endregion

		public delegate void ItemEventHandler(object sender, int projectId, int blockId, int workId, int groupId, int itemId);
		public event ItemEventHandler ItemChanged;
		
		protected virtual void OnItemChanged(int projectId, int blockId, int workId, int groupId, int itemId)
		{
			if (ItemChanged != null)
			{
				ItemChanged(this, projectId, blockId, workId, groupId, itemId);
			}
		}
		
		public delegate void SelectEventHandler(object sender, int projectId, int blockId, int workId, int groupId, int itemId);
		public event SelectEventHandler SelectChanged;
		
		protected virtual void OnSelectChanged(int projectId, int blockId, int workId, int groupId, int itemId)
		{
			if (SelectChanged != null)
			{
				SelectChanged(this, projectId, blockId, workId, groupId, itemId);
			}
		}

		public delegate void ClickEventHandler(object sender, NavItem item);
		public event ClickEventHandler ClickLinkItem;

		protected virtual void OnClickLinkItem(NavItem item)
		{
			if (ClickLinkItem != null)
			{
				ClickLinkItem(this, item);
			}
		}
	}
}