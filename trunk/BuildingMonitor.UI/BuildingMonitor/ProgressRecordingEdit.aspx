<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ProgressRecordingEdit.aspx.cs" Inherits="BuildingMonitor.UI.ProgressRecordingEditPage" %>

<%@ Register src="Controls/NavProjectToItem.ascx" tagname="NavProjectToItem" tagprefix="uc1" %>

<%@ Register src="Controls/NavProjectToItemPath.ascx" tagname="NavProjectToItemPath" tagprefix="uc2" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper">
<div class="modulecontent">	
	<asp:UpdatePanel ID="pnlMain" runat="server">
	<ContentTemplate>
		<div class="bm-navigator ui-widget-content ui-corner-all">
			<uc1:NavProjectToItem ID="navProjectToItem" runat="server" />
		</div>
		<div class="bm-navigator-path">
			<uc2:NavProjectToItemPath ID="navProjectToItemPath" runat="server" />
		</div>
		<br />
		<div>
			<asp:Panel ID="pnlContractDetail" runat="server" Visible="false">
				<div class="settingrow">
					<a href="javascript:bmSelectCheckBoxes('tblContractDetail','all')">Seleccionar Todos</a> |
					<a href="javascript:bmSelectCheckBoxes('tblContractDetail','none')">Seleccionar Ninguno</a> |
					<a href="javascript:bmProgressRecReset()">Reestablecer Progreso</a> |
					<a href="javascript:bmProgressRecActive(true)">Activar Progreso</a> |
					<a href="javascript:bmProgressRecActive(false)">Desactivar Progreso</a>
				</div>
				<table id="tblContractDetail" class="bm-table">
					<asp:Repeater ID="rptContractDetail" runat="server" >
						<HeaderTemplate>
							<thead>
							<tr>
							<th style="width:1%">&nbsp;</th>
							<th style="width:38%"><%# Resources.BuildingMonitorResources.LabelSubItem %></th>
							<th style="width:13%"><%# Resources.BuildingMonitorResources.LabelQuantity %></th>
							<th style="width:45%"><%# Resources.BuildingMonitorResources.ProgressRecFieldCurrent %></th>
							<th>&nbsp;</th>
							</tr>
							</thead>
						</HeaderTemplate>
						<SeparatorTemplate><tbody></SeparatorTemplate>
						<ItemTemplate>
							<tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "alternate" %>'>
							<td class="bm-progress-options">
								<asp:CheckBox ID="chkSet" runat="server" EnableViewState="false" />
								<asp:HiddenField ID="hdnData" runat="server" Value='<%# FillRowData((int)Eval("ContractId"), (int)Eval("ProjectId"), (int)Eval("BlockId"), (int)Eval("WorkId"), (int)Eval("GroupId"), (int)Eval("ItemId"), (int)Eval("SubItemId")) %>' EnableViewState="false" />
							</td>
							<td><%# Eval("Name") %></td>
							<td class="bm-number"><%# BuildingMonitor.UI.Helpers.Formatter.Decimal(Eval("Quantity")) %> <%# Eval("Unit") %></td>
							<td class="bm-progress">
								<span class="bm-progress-status"></span>
								<span class="bm-progress-label"></span>
								<div class="bm-progress-slider"></div>
							</td>
							<td class="bm-progress-action">
								<a href="javascript:bmSaveProgress(<%# Container.ItemIndex %>)" class="bm-progress-action-save"><span class='ui-icon ui-icon-disk'></span></a>
							</td>
							</tr>
						</ItemTemplate>
						<SeparatorTemplate></tbody></SeparatorTemplate>
					</asp:Repeater>
				</table>
			</asp:Panel>
		</div>
	</ContentTemplate>
	</asp:UpdatePanel>
	<div id="dialog_status"></div>
	<div id="progress-animation" style="display:none;z-index:1000;background:url(../Data/style/img/default/ajax-loader.gif) no-repeat center center">&nbsp;</div>
	<script type="text/javascript">
		function bmAnimation(show) {
			bmLoadingAnimation('progress-animation', '<% Response.Write(pnlMain.ClientID); %>', show);
		}
		function onBeginRequest(sender, args) {
			bmAnimation(true);
		}
		function onEndRequest(sender, args) {
			jQuery('#<% Response.Write(pnlContractDetail.ClientID); %> td.bm-progress').each(function(i) {
				bmProgressRecInitSlider(this);
			});

			bmAnimation(false);
		}
		
		jQuery(document).ready(function() {
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest);
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onBeginRequest);
			jQuery('#dialog_status').dialog({
				autoOpen: false,
				width: 300,
				height: 150,
				modal: true,
				buttons: {
					Aceptar: function() {
						jQuery(this).dialog('close');
					}
				}
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

 