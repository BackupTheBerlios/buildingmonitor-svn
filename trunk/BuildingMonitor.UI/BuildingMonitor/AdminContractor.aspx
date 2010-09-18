<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminContractor.aspx.cs" Inherits="BuildingMonitor.UI.AdminContractorPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<div class="modulecontent">
	<div class="settingrow">
		<asp:HyperLink ID="hlkNewContractor" runat="server" />
	</div>
	<mp:mojoGridView ID="mgvContractor" runat="server" 
		AllowPaging="true" 
		AllowSorting="true" 
		AutoGenerateColumns="false" 
		EnableTheming="false" 
		PageSize="15">
		<Columns>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ID="hlkEdit" runat="server" Text="<%# Resources.BuildingMonitorResources.LabelEdit %>" NavigateUrl='<%# BuildTargetEditContractorUrl(Eval("ContractorId").ToString()) %>' />					
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate><%# Eval("Name") %></ItemTemplate>
				<HeaderTemplate><%# Resources.BuildingMonitorResources.ContractorNameLabel %></HeaderTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate><%# Eval("Phone") %></ItemTemplate>
				<HeaderTemplate><%# Resources.BuildingMonitorResources.ContractorPhoneLabel %></HeaderTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate><%# Eval("Movile") %></ItemTemplate>
				<HeaderTemplate><%# Resources.BuildingMonitorResources.ContractorMovileLabel %></HeaderTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate><%# Eval("Status") %></ItemTemplate>
				<HeaderTemplate><%# Resources.BuildingMonitorResources.ContractorStatusLabel %></HeaderTemplate>
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