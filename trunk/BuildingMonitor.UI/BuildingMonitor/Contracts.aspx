<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Contracts.aspx.cs" Inherits="BuildingMonitor.UI.ContractsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper">
<div class="modulecontent">

<asp:HyperLink ID="hplNewContract" runat="server" />

<fieldset>
<legend>Ver Contratos</legend>
	<asp:DropDownList ID="ddlContractor" runat="server" CssClass="forminput" DataValueField="IdContractor" DataTextField="Name" AutoPostBack="false" />
	<asp:CheckBox ID="chkIsPaidWork" runat="server" Text="Obra Vendida" AutoPostBack="false" />
	<asp:Button ID="btnFilter" runat="server" Text="Aceptar" />
</fieldset>
<br />
<mp:mojoGridView ID="mgvContracts" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" EnableTheming="false" PageSize="15">
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:HyperLink ID="hlkEdit" runat="server" Text="<%# BuildCommandLabel() %>" NavigateUrl='<%# BuildCommandUrl(Eval("ContractId")) %>' />					
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate><%# BuildingMonitor.UI.Helpers.Formatter.ShortDate(Eval("DateCreated")) %></ItemTemplate>
			<ItemStyle CssClass="bm-date" />
			<HeaderTemplate><%# Resources.BuildingMonitorResources.ContractFieldCreatedOn %></HeaderTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate><%# BuildingMonitor.UI.Helpers.Formatter.ShortDate(Eval("DateStart")) %></ItemTemplate>
			<ItemStyle CssClass="bm-date" />
			<HeaderTemplate><%# Resources.BuildingMonitorResources.ContractFieldDateStart %></HeaderTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate><%# BuildingMonitor.UI.Helpers.Formatter.ShortDate(Eval("DateEnd")) %></ItemTemplate>
			<ItemStyle CssClass="bm-date" />
			<HeaderTemplate><%# Resources.BuildingMonitorResources.ContractFieldDateEnd %></HeaderTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate><%# Eval("Gloss") %></ItemTemplate>
			<HeaderTemplate><%# Resources.BuildingMonitorResources.ContractFieldDescription %></HeaderTemplate>
		</asp:TemplateField>
	</Columns>
</mp:mojoGridView>
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
