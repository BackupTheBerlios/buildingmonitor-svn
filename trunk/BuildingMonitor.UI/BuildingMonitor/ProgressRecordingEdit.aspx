<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ProgressRecordingEdit.aspx.cs" Inherits="BuildingMonitor.UI.ProgressRecordingEditPage" %>

<%@ Register src="Controls/NavProjectToItem.ascx" tagname="NavProjectToItem" tagprefix="uc1" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper">
<div class="modulecontent">	
	<asp:UpdatePanel ID="pnlMain" runat="server">
		<ContentTemplate>
			<div style="float:right;width:69%">
				<asp:Panel ID="pnlContractDetail" runat="server" Visible="false">
					<div class="settingrow">
						<a href="javascript:bmSelectCheckBoxes('tblContractDetail','all')">Seleccionar Todos</a> |
						<a href="javascript:bmSelectCheckBoxes('tblContractDetail','none')">Seleccionar Ninguno</a> |
						<a href="javascript:bmProgressRecReset()">Reestablecer Progreso</a>
					</div>
					<table id="tblContractDetail" class="bm-table">
						<asp:Repeater ID="rptContractDetail" runat="server" >
							<HeaderTemplate>
								<tr>
								<th>&nbsp;</th>
								<th style="width:30%"><%# Resources.BuildingMonitorResources.LabelSubItem %></th>
								<th style="width:15%"><%# Resources.BuildingMonitorResources.LabelQuantity %></th>
								<th style="width:51%"><%# Resources.BuildingMonitorResources.ProgressRecFieldCurrent %></th>
								</tr>
							</HeaderTemplate>
							<ItemTemplate>
								<tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "alternate" %>'>
								<td class="bm-progress-options">
									<asp:CheckBox ID="chkSet" runat="server" EnableViewState="false" />
									<asp:HiddenField ID="hdnData" runat="server" Value='<%# FillRowData((int)Eval("ContractId"), (int)Eval("ProjectId"), (int)Eval("BlockId"), (int)Eval("WorkId"), (int)Eval("GroupId"), (int)Eval("ItemId"), (int)Eval("SubItemId")) %>' EnableViewState="false" />
								</td>
								<td><%# Eval("Name") %></td>
								<td style="text-align:right"><%# BuildingMonitor.UI.Helpers.Formatter.Decimal(Eval("Quantity")) %> <%# Eval("Unit") %></td>
								<td class="bm-progress">
									<span class="bm-progress-label"></span>
									<div class="bm-progress-slider"></div>
									<span style="clear:both"></span>
								</td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
					</table>
				</asp:Panel>
				<br />
				<asp:Panel ID="pnlSavedSuccess" runat="server" Visible="false" CssClass="ui-widget">
					<div class="ui-state-highlight ui-corner-all" style="padding:0.2em 1em">
						<span style="float:left; margin-right: 0.3em;" class="ui-icon ui-icon-check"></span><asp:Literal ID="litSavedSuccess" runat="server" />
					</div>
				</asp:Panel>
			</div>
			<div style="width:29%;padding:0.5em" class="bm-navigator ui-widget-content ui-corner-all">
				<uc1:NavProjectToItem ID="navProjectToItem" runat="server" />
			</div>
	</ContentTemplate>
	</asp:UpdatePanel>
	<div style="clear:both;margin:0.5em 0">
		<asp:Button ID="btnSave" runat="server" ValidationGroup="ProgressRecording" Enabled="false" />
	</div>
	<div id="progress-animation" style="display:none;z-index:1000;background:url(../Data/style/img/default/ajax-loader.gif) no-repeat center center">&nbsp;</div>
	<script type="text/javascript">
		function bmAllowSaving() {
			if (jQuery('#<% Response.Write(pnlContractDetail.ClientID); %>').length != 1)
				jQuery('#<% Response.Write(btnSave.ClientID); %>').attr('disabled', 'disabled');
			else
				jQuery('#<% Response.Write(btnSave.ClientID); %>').removeAttr('disabled');
		}
		function onBeginRequest(sender, args) {
			bmLoadingAnimation('progress-animation', '<% Response.Write(pnlMain.ClientID); %>', true);
		}
		function onEndRequest(sender, args) {
			jQuery('#<% Response.Write(pnlContractDetail.ClientID); %> td.bm-progress').each(function(i) {
				bmProgressRecInitSlider(this);
			});

			bmAllowSaving();
			bmLoadingAnimation('progress-animation', '<% Response.Write(pnlMain.ClientID); %>', false);
		}
		jQuery(document).ready(function() {
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest);
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onBeginRequest)
			jQuery('#<% Response.Write(pnlContractDetail.ClientID); %> td.bm-progress').each(function(i) {
				bmProgressRecInitProgressBar(this);
			});
		})
	</script>
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
    runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" 
	runat="server" >
 </asp:Content>

 