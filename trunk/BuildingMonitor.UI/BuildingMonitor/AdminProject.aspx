<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminProject.aspx.cs" Inherits="BuildingMonitor.UI.AdminProjectPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
	<div class="settingrow">
		<asp:HyperLink ID="hlkNewProject" runat="server" />
	</div>

	<mp:mojoGridView ID="m_gridView" runat="server" 
            EmptyDataText="" AutoGenerateColumns="false"
            Caption="Listado de Proyectos">
            <Columns>
				<asp:TemplateField>
					<ItemTemplate><%# Eval("Name") %></ItemTemplate>
					<HeaderTemplate>Nombre</HeaderTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate><%# Eval("CreatedBy") %></ItemTemplate>
					<HeaderTemplate>Creado Por</HeaderTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate><%# Eval("DateCreated") %></ItemTemplate>
					<HeaderTemplate>Fecha Creacion</HeaderTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate><%# Eval("ModifiedBy") %></ItemTemplate>
					<HeaderTemplate>Modificado Por</HeaderTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate><%# Eval("DateModified") %></ItemTemplate>
					<HeaderTemplate>Fecha Modificacion</HeaderTemplate>
				</asp:TemplateField>
            </Columns>        
	</mp:mojoGridView>
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
	runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />