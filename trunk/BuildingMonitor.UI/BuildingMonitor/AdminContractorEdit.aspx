<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminContractorEdit.aspx.cs" Inherits="BuildingMonitor.UI.AdminContractorEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<mp:YUIPanel ID="pnlContractor" runat="server" CssClass="panelwrapper bm bm-contractoredit" DefaultButton="btnSave">
    <h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
    <div class="modulecontent">
        <div id="divtabs" class="yui-navset">
            <ul class="yui-nav">
                <li class="selected"><a href="#tabSettings"><em><asp:Literal ID="litSettingsTab" runat="server" /></em></a></li>
                <li><a href="#tabSpecialties"><em><asp:Literal ID="litSpecialtiesTab" runat="server" /></em></a></li>
                <li><a href="#tabStatus"><em><asp:Literal ID="litStatusTab" runat="server" /></em></a></li>
            </ul>
            <div class="yui-content">
                <div id="tabSettings">
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblName" runat="server" CssClass="settinglabel" ConfigKey="ContractorNameLabel" ResourceFile="BuildingMonitorResources" />
                        <asp:TextBox ID="txtName" Columns="70" runat="server" MaxLength="255" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblPhone" runat="server" CssClass="settinglabel" ConfigKey="ContractorPhoneLabel" ResourceFile="BuildingMonitorResources" />
                        <asp:TextBox ID="txtPhone" Columns="20" runat="server" MaxLength="50" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblMovile" runat="server" CssClass="settinglabel" ConfigKey="ContractorMovileLabel" ResourceFile="BuildingMonitorResources" />
                        <asp:TextBox ID="txtMovile" Columns="20" runat="server" MaxLength="50" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblSpecialties" runat="server" CssClass="settinglabel" ConfigKey="ContractorSpecialtiesLabel" ResourceFile="BuildingMonitorResources" />
                        <asp:ListBox ID="lstSpecialties" runat="server" DataValueField="IdSpecialty" DataTextField="Name" SelectionMode="Multiple" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblStatus" runat="server" CssClass="settinglabel" ConfigKey="ContractorStatusLabel" ResourceFile="BuildingMonitorResources" />
                        <asp:DropDownList ID="ddlStatus" runat="server" DataValueField="IdStatusContractor" DataTextField="Name" CssClass="forminput" />
                    </div>
                    <div class="settingrow">&nbsp;</div>
                </div>
                <div id="tabSpecialties">
					 <span>&nbsp;</span>
                </div>
                <div id="tabStatus">
					<fieldset>
						<legend>Banco 1</legend>
						<div class="settingrow">
							<mp:SiteLabel ID="lblBank1" runat="server" CssClass="settinglabel" ConfigKey="ContractorBank" ResourceFile="BuildingMonitorResources" />
							<asp:TextBox ID="txtBank1" Columns="20" runat="server" MaxLength="50" CssClass="forminput" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="lblBankAccount1" runat="server" CssClass="settinglabel" ConfigKey="ContractorBankAccount" ResourceFile="BuildingMonitorResources" />
							<asp:TextBox ID="txtBankAccount1" Columns="20" runat="server" MaxLength="20" CssClass="forminput" />
						</div>
					</fieldset>
					<fieldset>
						<legend>Banco 2</legend>
						<div class="settingrow">
							<mp:SiteLabel ID="lblBank2" runat="server" CssClass="settinglabel" ConfigKey="ContractorBank" ResourceFile="BuildingMonitorResources" />
							<asp:TextBox ID="txtBank2" Columns="20" runat="server" MaxLength="50" CssClass="forminput" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="lblBankAccount2" runat="server" CssClass="settinglabel" ConfigKey="ContractorBankAccount" ResourceFile="BuildingMonitorResources" />
							<asp:TextBox ID="txtBankAccount2" Columns="20" runat="server" MaxLength="20" CssClass="forminput" />
						</div>
					</fieldset>
					<fieldset>
						<legend>Banco 3</legend>
						<div class="settingrow">
							<mp:SiteLabel ID="lblBank3" runat="server" CssClass="settinglabel" ConfigKey="ContractorBank" ResourceFile="BuildingMonitorResources" />
							<asp:TextBox ID="txtBank3" Columns="20" runat="server" MaxLength="50" CssClass="forminput" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="lblBankAccount3" runat="server" CssClass="settinglabel" ConfigKey="ContractorBankAccount" ResourceFile="BuildingMonitorResources" />
							<asp:TextBox ID="txtBankAccount3" Columns="20" runat="server" MaxLength="20" CssClass="forminput" />
						</div>
					</fieldset>
                </div>                
            </div>
        </div>
        <div>
            <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="Contractor" />
            <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName" Display="None" ValidationGroup="Contractor" />
            <asp:Button ID="btnSave" runat="server" ValidationGroup="Contractor" onclick="SaveClick" />
            <asp:Button ID="btnDelete" runat="server" CausesValidation="false" />
        </div>
    </div>
</mp:YUIPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
	runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
