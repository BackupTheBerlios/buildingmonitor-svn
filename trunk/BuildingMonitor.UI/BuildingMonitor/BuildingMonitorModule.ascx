<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="BuildingMonitorModule.ascx.cs" Inherits="BuildingMonitor.UI.BuildingMonitorModule" %>

<mp:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="panelwrapper BuildingMonitor">
<portal:ModuleTitleControl EditText="Edit" EditUrl="~/BuildingMonitor/AdminDashboard.aspx" runat="server" id="TitleControl" />

<asp:Panel ID="pnlBuildingMonitor" runat="server" CssClass="modulecontent">

<div class="settingrow">
    <asp:HyperLink ID="hplContract" runat="server" />
</div>
<div class="settingrow">
    <asp:HyperLink ID="hplProgressRecording" runat="server" />
</div>
<div class="settingrow">
    <asp:HyperLink ID="hplPayment" runat="server" />
</div>
<div class="settingrow">
    <asp:HyperLink ID="hplAnalisisRiesgo" runat="server" />
</div>
<div class="settingrow">
    <asp:HyperLink ID="hplResumenRiesgo" runat="server" />
</div>

</asp:Panel>
</asp:Panel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
