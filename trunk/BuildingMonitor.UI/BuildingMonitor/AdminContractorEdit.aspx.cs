// Author:					Jose Luis Ferrufino Rivera
// Created:					2010-2-5
// Last Modified:			2010-2-5
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

	public partial class AdminContractorEditPage : mojoBasePage
	{
		private int _pageId = -1;
		private int _moduleId = -1;
		private int _contractorId = -1;

		protected int BankAccountId1
		{
			get
			{
				return ViewState["BankAccountId1"] == null ? -1 : (int)ViewState["BankAccountId1"];
			}
			set
			{
				ViewState["BankAccountId1"] = value;
			}
		}

		protected int BankAccountId2
		{
			get
			{
				return ViewState["BankAccountId2"] == null ? -1 : (int)ViewState["BankAccountId2"];
			}
			set
			{
				ViewState["BankAccountId2"] = value;
			}
		}

		protected int BankAccountId3
		{
			get
			{
				return ViewState["BankAccountId3"] == null ? -1 : (int)ViewState["BankAccountId3"];
			}
			set
			{
				ViewState["BankAccountId3"] = value;
			}
		}

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
			if (!IsPostBack)
			{
				using (IDataReader reader = Contractor.GetAllSpecialties())
				{
					this.lstSpecialties.DataSource = reader;
					this.lstSpecialties.DataBind();
				}

				using (IDataReader reader = Contractor.GetAllStatus())
				{
					ddlStatus.DataSource = reader;
					ddlStatus.DataBind();
					ddlStatus.Items.Insert(0, new ListItem(BuildingMonitorResources.OptionSelect, ""));
				}
				
				if (_contractorId > 0)
				{
					Contractor contractor = Contractor.Create(_contractorId);
					
					txtName.Text = contractor.Name;
					txtPhone.Text = contractor.Phone;
					txtMovile.Text = contractor.Movile;

					int i = 0;
					foreach(ListItem item in ddlStatus.Items)
					{
						if (contractor.StatusCode == item.Value)
						{
							ddlStatus.SelectedIndex = i;
							break;
						}

						i++;
					}
					
					foreach(ListItem item in lstSpecialties.Items)
					{
						item.Selected = contractor.Specialties.Contains(item.Value);
					}

					FillBankAccounts(contractor);
				}
			}
		}

		private void FillBankAccounts(Contractor contractor)
		{
			int count = contractor.BankAccounts.Count;

			if (count > 0)
			{
				BankAccountId1 = contractor.BankAccounts[0].Id;
				txtBank1.Text = contractor.BankAccounts[0].Bank;
				txtBankAccount1.Text = contractor.BankAccounts[0].AccountNumber;
			}

			if (count > 1)
			{
				BankAccountId2 = contractor.BankAccounts[1].Id;
				txtBank2.Text = contractor.BankAccounts[1].Bank;
				txtBankAccount2.Text = contractor.BankAccounts[1].AccountNumber;
			}

			if (count > 2)
			{
				BankAccountId3 = contractor.BankAccounts[2].Id;
				txtBank3.Text = contractor.BankAccounts[2].Bank;
				txtBankAccount3.Text = contractor.BankAccounts[2].AccountNumber;
			}
		}

		private void PopulateLabels()
		{
			this.litHeading.Text = BuildingMonitorResources.Contractor;
			this.litSettingsTab.Text = BuildingMonitorResources.Contractor;
			this.litSpecialtiesTab.Text = BuildingMonitorResources.Specialties;
			this.litStatusTab.Text = BuildingMonitorResources.Status;
			this.btnSave.Text = BuildingMonitorResources.LabelSave;
			this.btnDelete.Text = BuildingMonitorResources.LabelDelete;
		}

		private void LoadSettings()
		{

		}

		private void LoadParams()
		{
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
			_contractorId = WebUtils.ParseInt32FromQueryString("contractorid", _contractorId);
		}

		private void Save()
		{
			Contractor contractor = new Contractor(_contractorId);
			ContractorBankAccount bankAccount = null;

			contractor.Name = this.txtName.Text;
			contractor.Phone = this.txtPhone.Text;
			contractor.Movile = this.txtMovile.Text;
			contractor.StatusCode = this.ddlStatus.SelectedValue;


			foreach (ListItem item in this.lstSpecialties.Items)
			{
				if (item.Selected)
				{
					contractor.Specialties.Add(item.Value);
				}
			}
			
			bankAccount = new ContractorBankAccount(BankAccountId1);
			bankAccount.Bank = txtBank1.Text;
			bankAccount.AccountNumber = txtBankAccount1.Text;
			contractor.BankAccounts.Add(bankAccount);
			
			bankAccount = new ContractorBankAccount(BankAccountId2);
			bankAccount.Bank = txtBank2.Text;
			bankAccount.AccountNumber = txtBankAccount2.Text;
			contractor.BankAccounts.Add(bankAccount);

			bankAccount = new ContractorBankAccount(BankAccountId3);
			bankAccount.Bank = txtBank3.Text;
			bankAccount.AccountNumber = txtBankAccount3.Text;
			contractor.BankAccounts.Add(bankAccount);

			contractor.Save();
		}

		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);

			SuppressPageMenu();
			SuppressGoogleAds();
			ScriptConfig.IncludeYuiTabs = true;
			IncludeYuiTabsCss = true;
		}

		#endregion

		protected void SaveClick(object sender, EventArgs e)
		{
			Page.Validate();
			
			if (Page.IsValid)
			{
				Save();
			}
		}
	}
}
