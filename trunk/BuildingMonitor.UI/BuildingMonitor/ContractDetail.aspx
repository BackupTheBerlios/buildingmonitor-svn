<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ContractDetail.aspx.cs" Inherits="BuildingMonitor.UI.ContractDetailPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper">
<div class="modulecontent">	
	<div class="no-print" style="text-align:right">
		<asp:HyperLink ID="hlkPrint" runat="server" Text="Imprimir" Target="_blank" />
	</div>
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
			<asp:TextBox ID="txtGloss" runat="server" Columns="54" Rows="6" TextMode="MultiLine" ReadOnly="true" EnableViewState="false" />
		</div>
		<br />
		<asp:Table ID="tblContractDetail" runat="server" CssClass="bm-table" EnableViewState="false">
			<asp:TableHeaderRow TableSection="TableHeader">
				<asp:TableHeaderCell Width="1%">&nbsp;</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="60%">SubItem</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="10%">Cantidad</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="9%">A. Inicial</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="10%">Precio <% Response.Write(litCurrency.Text); %></asp:TableHeaderCell>
			</asp:TableHeaderRow>
			<asp:TableFooterRow TableSection="TableFooter">
				<asp:TableCell ColumnSpan="4" CssClass="bm-label-total">Total Contrato: </asp:TableCell>
				<asp:TableCell CssClass="bm-total bm-number"><asp:Literal ID="litTotalContract" runat="server" /></asp:TableCell>
			</asp:TableFooterRow>
		</asp:Table>
	<br />
	<div class="no-print">
		<asp:Button ID="btnCancelContract" runat="server" />
		<asp:Button ID="btnCancel" runat="server" />
	</div>
</div>
</asp:Panel>
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
	runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
