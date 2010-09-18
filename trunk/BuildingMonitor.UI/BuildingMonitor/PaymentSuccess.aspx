<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PaymentSuccess.aspx.cs" Inherits="BuildingMonitor.UI.PaymentSuccessPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
<div class="settingrow">
	<asp:Literal ID="litCompanyName" runat="server" />
</div>
<div class="settingrow">
	<mp:SiteLabel ID="lblDate" runat="server" CssClass="settinglabel" ConfigKey="LabelDate" ResourceFile="BuildingMonitorResources" />
	<asp:Literal ID="litDate" runat="server" />
</div>
<div class="settingrow">
	<mp:SiteLabel ID="lblPaidTo" runat="server" CssClass="settinglabel" ConfigKey="PaymentPaidToContractor" ResourceFile="BuildingMonitorResources" />
	<asp:Literal ID="litContractorName" runat="server" />
</div>
<asp:Table ID="tblPaidProgress" runat="server" CssClass="bm-table">
	<asp:TableHeaderRow TableSection="TableHeader">
		<asp:TableHeaderCell>SubItem</asp:TableHeaderCell>
		<asp:TableHeaderCell>Fecha</asp:TableHeaderCell>
		<asp:TableHeaderCell>Avance</asp:TableHeaderCell>
		<asp:TableHeaderCell>Monto <asp:Literal ID="ltrCurrency" runat="server" /></asp:TableHeaderCell>
	</asp:TableHeaderRow> 
</asp:Table>
<div style="margin:0.3em 1% 0 0;font-weight:bold;text-align:right" id="total-payment">
Total (<% Response.Write(ltrCurrency.Text); %>): <span class="total-payment"><asp:Literal ID="ltrTotal" runat="server" /></span>
</div>

<div style="margin:0.5em 0" class="no-print">
	<asp:Button ID="btnPrint" runat="server" OnClientClick="print();return false" />
</div>

</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
	runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
