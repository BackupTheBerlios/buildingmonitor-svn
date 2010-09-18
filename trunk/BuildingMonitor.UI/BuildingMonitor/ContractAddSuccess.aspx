<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ContractAddSuccess.aspx.cs" Inherits="BuildingMonitor.UI.ContractAddSuccessPage" %>

<%@ Register src="Controls/NavProjectToItem.ascx" tagname="NavProjectToItem" tagprefix="uc1" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper">
<div class="modulecontent">
	<fieldset>
		<legend><asp:Literal ID="litTabMain" runat="server" /></legend>
		
		<div style="float:left;width:44%">
		<div class="settingrow">
			<mp:SiteLabel ID="lblContractor" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldContractor" ResourceFile="BuildingMonitorResources" />
			<asp:Literal ID="litContractor" runat="server" />
		</div>
		<div class="settingrow">
			<mp:SiteLabel ID="lblDateStart" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldDateStart" ResourceFile="BuildingMonitorResources" />
			<asp:Literal ID="litDateStart" runat="server" />
		</div>
		<div class="settingrow">
			<mp:SiteLabel ID="lblDateEnd" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldDateEnd" ResourceFile="BuildingMonitorResources" />
			<asp:Literal ID="litDateEnd" runat="server" />
		</div>
		<div class="settingrow">
			<mp:SiteLabel ID="lblCurrency" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldCurrency" ResourceFile="BuildingMonitorResources" />
			<asp:Literal ID="litCurrency" runat="server" />
		</div>
		<div class="settingrow">
			<mp:SiteLabel ID="lblCurrencyRate" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldCurrencyRate" ResourceFile="BuildingMonitorResources" />
			<asp:Literal ID="litCurrencyRate" runat="server" />
		</div>
		</div>
		<div style="margin-left:46%">
			Glosa (opcional)
			<asp:TextBox ID="txtGloss" runat="server" ReadOnly="true" Columns="54" Rows="6" TextMode="MultiLine" />
		</div>
	</fieldset>
	
	<fieldset id="contract-detail">
		<legend><asp:Literal ID="litTabDetail" runat="server" /></legend>
		<asp:HiddenField ID="hdnDataRef" runat="server" EnableViewState="false" />
		<div>
			<asp:Table ID="tblContractDetail" runat="server" CssClass="bm-table" EnableViewState="false">
				<asp:TableHeaderRow TableSection="TableHeader">
					<asp:TableHeaderCell Width="1%">&nbsp;</asp:TableHeaderCell>
					<asp:TableHeaderCell>SubItem</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="20%">Cantidad</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="20%">Total <% Response.Write(litCurrency.Text); %></asp:TableHeaderCell>
				</asp:TableHeaderRow>
				
			</asp:Table>
		</div>
	</fieldset>
	<fieldset>
		<legend><asp:Literal ID="litTabPaidWork" runat="server" /></legend>
		<div class="settingrow" id="paidwork-container">
			<mp:SiteLabel ID="lblAdvance" runat="server" CssClass="settinglabel" ConfigKey="PaidWorkAdvance" ResourceFile="BuildingMonitorResources" />	
			<asp:TextBox ID="txtAdvance" runat="server" CssClass="forminput" /> <% Response.Write(litCurrency.Text); %>
			<fieldset>
				<legend>Plan de pagos</legend>
				<asp:Table ID="tblPayment" runat="server" CssClass="bm-table" EnableViewState="false">
					<asp:TableHeaderRow TableSection="TableHeader">
						<asp:TableHeaderCell Width="20%">Fecha</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="20%">Monto <% Response.Write(litCurrency.Text); %></asp:TableHeaderCell>
						<asp:TableHeaderCell>Avance Requerido</asp:TableHeaderCell>
					</asp:TableHeaderRow>
				</asp:Table>
				<br />
			</fieldset>
		</div>
	</fieldset>

	<div style="clear:both;margin-top:0.5em">
		
	</div>
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" >
 </asp:Content>

 