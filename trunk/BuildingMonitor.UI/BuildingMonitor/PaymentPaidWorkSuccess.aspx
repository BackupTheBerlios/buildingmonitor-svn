<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PaymentPaidWorkSuccess.aspx.cs" Inherits="BuildingMonitor.UI.PaymentPaidWorkSuccessPage" %>

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
<div class="settingrow">
	<mp:SiteLabel ID="lblProgress" runat="server" CssClass="settinglabel" ConfigKey="ProgressRecLastProgress" ResourceFile="BuildingMonitorResources" />
	<asp:Literal ID="litProgress" runat="server" />
</div>
<div style="margin:0.3em 1% 0 0;font-weight:bold" id="total-payment">
Total (<asp:Literal ID="ltrCurrency" runat="server" />): <span class="total-payment"><asp:Literal ID="ltrTotal" runat="server" /></span>
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
