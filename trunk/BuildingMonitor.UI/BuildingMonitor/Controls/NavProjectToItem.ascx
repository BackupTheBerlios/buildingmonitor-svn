<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavProjectToItem.ascx.cs" Inherits="BuildingMonitor.UI.BuildingMonitor.Controls.NavProjectToItem" %>
<table>
<tr>
<td class="project">
	&nbsp;<asp:Literal ID="litProject" runat="server" />
	<asp:LinkButton ID="lbtProjectR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" /><br />
	<asp:DropDownList ID="ddlProject" runat="server" DataValueField="ProjectId" DataTextField="Name" AutoPostBack="true" EnableTheming="False" />
</td>
<td class="block">
	&nbsp;<asp:Literal ID="litBlock" runat="server" />
	<asp:LinkButton ID="lbtBlockR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" /><br />
	<asp:DropDownList ID="ddlBlock" runat="server" DataValueField="BlockId" DataTextField="Name" AutoPostBack="true" EnableTheming="False" />
</td>
<td class="work">
	&nbsp;<asp:Literal ID="litWork" runat="server" />
	<asp:LinkButton ID="lbtWorkR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" /><br />
	<asp:DropDownList ID="ddlWork" runat="server" DataValueField="WorkId" DataTextField="Name" AutoPostBack="true" EnableTheming="False" />
	
</td>
<td class="group">
	&nbsp;<asp:Literal ID="litGroup" runat="server" />
	<asp:LinkButton ID="lbtGroupR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" /><br />
	<asp:DropDownList ID="ddlGroup" runat="server" DataValueField="GroupId" DataTextField="Name" AutoPostBack="true" EnableTheming="False" />
</td>
<td class="item">
	&nbsp;<asp:Literal ID="litItem" runat="server" />
	<asp:LinkButton ID="lbtItemR" runat="server" Text="<span class='ui-icon ui-icon-circle-triangle-e' style='float:right'></span>" /><br />
	<asp:DropDownList ID="ddlItem" runat="server" DataValueField="ItemId" DataTextField="Name" AutoPostBack="true" EnableTheming="False" />
</td>
</tr>
</table>
