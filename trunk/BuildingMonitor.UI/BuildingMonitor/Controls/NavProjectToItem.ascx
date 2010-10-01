<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavProjectToItem.ascx.cs" Inherits="BuildingMonitor.UI.BuildingMonitor.Controls.NavProjectToItem" %>
<strong><asp:Literal ID="litHeading" runat="server" /></strong>
<div class="settingrow">
	<asp:Label ID="lblProject" runat="server" /><br />
	<asp:DropDownList ID="ddlProject" runat="server" DataValueField="ProjectId" DataTextField="Name" AutoPostBack="true" />
	<asp:LinkButton ID="lbtProjectR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" />
</div>
<div class="settingrow">
	<asp:Label ID="lblBlock" runat="server" /><br />
	<asp:DropDownList ID="ddlBlock" runat="server" DataValueField="BlockId" DataTextField="Name" AutoPostBack="true" />
	<asp:LinkButton ID="lbtBlockR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" />
</div>
<div class="settingrow">
	<asp:Label ID="lblWork" runat="server" /><br />
	<asp:DropDownList ID="ddlWork" runat="server" DataValueField="WorkId" DataTextField="Name" AutoPostBack="true" />
	<asp:LinkButton ID="lbtWorkR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" />
</div>
<div class="settingrow">
	<asp:Label ID="lblGroup" runat="server" /><br />
	<asp:DropDownList ID="ddlGroup" runat="server" DataValueField="GroupId" DataTextField="Name" AutoPostBack="true" />
	<asp:LinkButton ID="lbtGroupR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" />
</div>
<div class="settingrow">
	<asp:Label ID="lblItem" runat="server" /><br />
	<asp:DropDownList ID="ddlItem" runat="server" DataValueField="ItemId" DataTextField="Name" AutoPostBack="true" />
	<asp:LinkButton ID="lbtItemR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" />
</div>

