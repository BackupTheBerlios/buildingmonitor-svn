<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminContractEdit.aspx.cs" Inherits="BuildingMonitor.UI.AdminContractEditPage" %>

<%@ Register src="Controls/NavProjectToItem.ascx" tagname="NavProjectToItem" tagprefix="uc1" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
	<mp:YUIPanel ID="pnlContract" runat="server" CssClass="panelwrapper bm bm-contractedit" DefaultButton="btnSave">
		<div id="divtabs" class="yui-navset">
			<ul class="yui-nav">
				<li class="selected"><a href="#tabMain"><em><asp:Literal ID="litMainTab" runat="server" /></em></a></li>
				<li><a href="#tabDescription"><em><asp:Literal ID="litDescriptionTab" runat="server" /></em></a></li>
				<li><a href="#tabDetail"><em><asp:Literal ID="litDetailTab" runat="server" /></em></a></li>
				<li><a href="#tabMore"><em><asp:Literal ID="litMoreTab" runat="server" /></em></a></li>
			</ul>
			<div class="yui-content">
				<div id="tabMain">
					<div class="settingrow">
						<mp:SiteLabel ID="lblIdContract" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldId" ResourceFile="BuildingMonitorResources" />
						<asp:TextBox ID="txtId" Columns="20" runat="server" MaxLength="50" CssClass="forminput" Enabled="false" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblContractor" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldContractor" ResourceFile="BuildingMonitorResources" />
						<asp:DropDownList ID="ddlContractor" runat="server" CssClass="forminput" DataValueField="IdContractor" DataTextField="Name"  />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblDateStart" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldDateStart" ResourceFile="BuildingMonitorResources" />
						<asp:TextBox ID="txtDateStart" runat="server" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblDateEnd" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldDateEnd" ResourceFile="BuildingMonitorResources" />
						<asp:TextBox ID="txtDateEnd" runat="server" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblAmount" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldAmount" ResourceFile="BuildingMonitorResources" />
						<asp:TextBox ID="txtAmount" runat="server" />
						<asp:DropDownList ID="ddlCurrency" runat="server" DataValueField="Code" DataTextField="Code" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblCurrencyRate" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldCurrencyRate" ResourceFile="BuildingMonitorResources" />
						<asp:TextBox ID="txtCurrencyRate" runat="server" />
					</div>
					<div class="settingrow">&nbsp;</div>
				</div>
				<div id="tabDescription">
					<mpe:EditorControl ID="edtDescription" runat="server" ></mpe:EditorControl>
				</div>
				<div id="tabDetail">
					<asp:UpdatePanel ID="pnlDetail" runat="server">
						<ContentTemplate>
							<div style="float:right;width:68%">
								<div class="settingrow">
									<asp:DropDownList ID="ddlSubItems" runat="server"/>
									<asp:Button ID="btnAddSubItem" runat="server" />
								</div>
								<div class="settingrow AspNet-GridView">
									<asp:Table ID="tblDetailSubItems" runat="server">
									<asp:TableRow TableSection="TableHeader">
										<asp:TableHeaderCell>
										<asp:Literal ID="litDetailNameHead" runat="server" /></asp:TableHeaderCell>
											<asp:TableHeaderCell><asp:Literal ID="litDetailQtyHead" runat="server" /></asp:TableHeaderCell>
											<asp:TableHeaderCell><asp:Literal ID="litDetailPriceHead" runat="server" /></asp:TableHeaderCell>
											<asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
									</asp:TableRow>
									</asp:Table>
								</div>
								<div class="settingrow">&nbsp;</div>
							</div>
							<div style="width:29%">
								<uc1:NavProjectToItem ID="navProjectToItem" runat="server" />
							</div>
							<br style="clear:both"/>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<div id="tabMore">
					<div class="settingrow">
						<mp:SiteLabel ID="lblCreatedBy" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldCreatedBy" ResourceFile="BuildingMonitorResources" />
						<span class="forminput"><asp:Literal ID="litCreatedBy" runat="server" /></span>
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblCreatedOn" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldCreatedOn" ResourceFile="BuildingMonitorResources" />
						<span class="forminput"><asp:Literal ID="litCreatedOn" runat="server" /></span>
					</div>
					<br />
					<div class="settingrow">
						<mp:SiteLabel ID="lblModifiedBy" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldModifiedBy" ResourceFile="BuildingMonitorResources" />
						<span class="forminput"><asp:Literal ID="litModifiedBy" runat="server" /></span>
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblModifiedOn" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldModifiedOn" ResourceFile="BuildingMonitorResources" />
						<span class="forminput"><asp:Literal ID="litModifiedOn" runat="server" /></span>
					</div>
					<br />
					<div class="settingrow">
						<mp:SiteLabel ID="lblDeletedBy" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldDeletedBy" ResourceFile="BuildingMonitorResources" />
						<span class="forminput"><asp:Literal ID="litDeletedBy" runat="server" /></span>
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblDeletedOn" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldDeletedOn" ResourceFile="BuildingMonitorResources" />
						<span class="forminput"><asp:Literal ID="litDeletedOn" runat="server" /></span>
					</div>
					<div class="settingrow">&nbsp;</div>
				</div>                
			</div>
		</div>
	</mp:YUIPanel>
	<div>
		<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="Contract" ShowSummary="true" />
		<asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtDateStart" Display="None" ValidationGroup="Contract" />
		<asp:Button ID="btnSave" runat="server" ValidationGroup="Contract" />
		<asp:Button ID="btnDelete" runat="server" CausesValidation="false" />
	</div>
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
	runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
