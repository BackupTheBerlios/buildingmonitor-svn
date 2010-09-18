<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ContractAdd.aspx.cs" Inherits="BuildingMonitor.UI.ContractAddPage" %>

<%@ Register src="Controls/NavProjectToItem.ascx" tagname="NavProjectToItem" tagprefix="uc1" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<script type="text/javascript" src="../ClientScript/jquery/i18n/ui.datepicker-<% Response.Write(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName); %>.js"></script>
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper">
<div class="modulecontent">
	<fieldset>
		<legend><asp:Literal ID="litTabMain" runat="server" /></legend>
		
		<div style="float:left;width:44%">
		<div class="settingrow bm-contract-contractor">
			<mp:SiteLabel ID="lblContractor" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldContractor" ResourceFile="BuildingMonitorResources" />
			<asp:DropDownList ID="ddlContractor" runat="server" CssClass="forminput" DataValueField="IdContractor" DataTextField="Name" />
		</div>
		<div class="settingrow">
			<mp:SiteLabel ID="lblDateStart" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldDateStart" ResourceFile="BuildingMonitorResources" />
			<asp:TextBox ID="txtDateStart" runat="server" />
		</div>
		<div class="settingrow">
			<mp:SiteLabel ID="lblDateEnd" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldDateEnd" ResourceFile="BuildingMonitorResources" />
			<asp:TextBox ID="txtDateEnd" runat="server" />
		</div>
		<div class="settingrow">
			<mp:SiteLabel ID="lblCurrency" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldCurrency" ResourceFile="BuildingMonitorResources" />
			<asp:DropDownList ID="ddlCurrency" runat="server" DataValueField="Code" DataTextField="Code" AutoPostBack="true" />
		</div>
		<div class="settingrow">
			<mp:SiteLabel ID="lblCurrencyRate" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldCurrencyRate" ResourceFile="BuildingMonitorResources" />
			<asp:TextBox ID="txtCurrencyRate" runat="server" />
		</div>
		</div>
		<div style="margin-left:46%">
			Glosa (opcional)
			<asp:TextBox ID="txtGloss" runat="server" Columns="54" Rows="6" TextMode="MultiLine" />
		</div>
	</fieldset>

	<fieldset id="contract-detail">
		<legend><asp:Literal ID="litTabDetail" runat="server" /></legend>
		<div style="float:right;width:68%;padding:0 0.5em">
			<div class="settingrow">
				<asp:LinkButton ID="lnkRemoveRow" runat="server" OnClientClick="beforeRemoveRows()" Text="Borrar Seleccionados"/> | 
				<a href="javascript:bmSetRefPriceTotal('<% Response.Write(tblContractDetail.ClientID); %>');bmContractSumTotal('<% Response.Write(tblContractDetail.ClientID); %>','subitem-price-total','bm-total')">Usar Dato Referencial</a>	
			</div>
			<asp:Table ID="tblContractDetail" runat="server" CssClass="bm-table" EnableViewState="false">
				<asp:TableHeaderRow TableSection="TableHeader">
					<asp:TableHeaderCell Width="1%"><input type="checkbox" onclick="selectCheckBox(this,'<% Response.Write(tblContractDetail.ClientID); %>')" /></asp:TableHeaderCell>
					<asp:TableHeaderCell>SubItem</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="15%">Precio <% Response.Write(ddlCurrency.SelectedValue); %></asp:TableHeaderCell>
					<asp:TableHeaderCell Width="15%">Cantidad</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="10%">Avance</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="15%">Total <% Response.Write(ddlCurrency.SelectedValue); %></asp:TableHeaderCell>
				</asp:TableHeaderRow>
				<asp:TableFooterRow TableSection="TableFooter">
					<asp:TableCell ColumnSpan="5" CssClass="bm-label-total">Total Contrato: </asp:TableCell>
					<asp:TableCell CssClass="bm-total bm-number"></asp:TableCell>
				</asp:TableFooterRow>
			</asp:Table>
		</div>
		<div style="width:28%;padding:0.5em" class="ui-widget-content ui-corner-all bm-navigator">
			<asp:UpdatePanel ID="pnlMain" runat="server">
				<ContentTemplate>
					<uc1:NavProjectToItem ID="navProjectToItem" runat="server" />
					<asp:HiddenField ID="hdnDataRef" runat="server" EnableViewState="false" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
	</fieldset>
	<fieldset>
		<legend><asp:Literal ID="litTabPaidWork" runat="server" /></legend>
		<asp:CheckBox ID="chkIsPaidWork" runat="server" Checked="false" />
		<mp:SiteLabel ID="lblIsPaidWork" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldIsPaidWork" ResourceFile="BuildingMonitorResources"/>
		<div class="settingrow" id="paidwork-container">
			<mp:SiteLabel ID="lblAdvance" runat="server" CssClass="settinglabel" ConfigKey="PaidWorkAdvance" ResourceFile="BuildingMonitorResources" />	
			<asp:TextBox ID="txtAdvance" runat="server" CssClass="forminput" /> <% Response.Write(ddlCurrency.SelectedValue); %>
			<fieldset>
				<legend>Plan de pagos</legend>
				<div class="settingrow">
					<a href="javascript:bmContractRemoveRow('<% Response.Write(tblPayment.ClientID); %>');bmContractPaidWorkSetBalance('<% Response.Write(tblPayment.ClientID); %>', '<% Response.Write(tblContractDetail.ClientID); %>', 'bm-total', '<% Response.Write(txtAdvance.ClientID); %>')">Borrar Seleccionados</a> | 
					<a href="javascript:bmContractPaidWorkAddRow('<% Response.Write(tblPayment.ClientID); %>', '<% Response.Write(tblContractDetail.ClientID); %>', 'bm-total', '<% Response.Write(txtAdvance.ClientID); %>');">Nueva Fila</a>
				</div>
				<asp:Table ID="tblPayment" runat="server" CssClass="bm-table" EnableViewState="false">
					<asp:TableHeaderRow TableSection="TableHeader">
						<asp:TableHeaderCell Width="1%"><input type="checkbox" onclick="selectCheckBox(this,'<% Response.Write(tblPayment.ClientID); %>')" /></asp:TableHeaderCell>
						<asp:TableHeaderCell>Avance Requerido</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="20%">Fecha</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="20%">Monto <% Response.Write(ddlCurrency.SelectedValue); %></asp:TableHeaderCell>
					</asp:TableHeaderRow>
				</asp:Table>
				<br />
			</fieldset>
		</div>
	</fieldset>

	<div style="clear:both;margin-top:0.5em">
		<asp:Button ID="btnSave" runat="server" OnClientClick="return bmContractValidate('trace-error');" />
		<div style='padding:0.2em 1em;margin:0.5em 0'>
			<div id="trace-error" class="ui-state-error ui-corner-all"><asp:Literal ID="litTraceError" runat="server" EnableViewState="false" /></div>
			<div id="trace-success" class="ui-state-highlight ui-corner-all"><asp:Literal ID="litTraceSuccess" runat="server" EnableViewState="false" /></div>
		</div>
	</div>
	
	<div id="progress-animation" style="display:none;z-index:1000;background:url(../Data/style/img/default/ajax-loader.gif) no-repeat center center">&nbsp;</div>
	
	<script type="text/javascript">
		function onBeginRequest(sender, args) {
			bmLoadingAnimation('progress-animation', 'contract-detail', true);
		}
		function onEndRequest(sender, args) {
			bmContractFillSubItemsDetail('<% Response.Write(hdnDataRef.ClientID); %>', '<% Response.Write(tblContractDetail.ClientID); %>')
			bmContractSumTotal('<% Response.Write(tblContractDetail.ClientID); %>', 'subitem-price-total', 'bm-total', true);
			bmContractPaidWorkSetBalance('<% Response.Write(tblPayment.ClientID); %>', '<% Response.Write(tblContractDetail.ClientID); %>', 'bm-total', '<% Response.Write(txtAdvance.ClientID); %>');
			bmLoadingAnimation('progress-animation', 'contract-detail', false);
		}
		function updateCheckIsPaidWork() {
			if (this.checked)
				jQuery('#paidwork-container').show();
			else
				jQuery('#paidwork-container').hide();
		}
		function beforeRemoveRows() {
			var checked = jQuery('#<% Response.Write(tblContractDetail.ClientID); %> input.bm-row-check:checked');

			checked.each(function() {
				var subitemsIds = jQuery(this).next();
				this.value = subitemsIds.val();
				this.name = 'bmRowChecked';
			});
		}
		function selectCheckBox(checkbox, containerId) {
			bmSelectCheckBoxes(containerId, checkbox.checked ? 'all' : 'none');
		}
		jQuery(document).ready(function() {
			var checkIsPaidWork = jQuery('#<% Response.Write(chkIsPaidWork.ClientID); %>');

			jQuery.datepicker.setDefaults(jQuery.datepicker.regional['<% Response.Write(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName); %>']);
			jQuery.datepicker.setDefaults({ duration: 'fast', changeMonth: true, changeYear: true });
			jQuery('#<% Response.Write(txtDateStart.ClientID); %>').datepicker();
			jQuery('#<% Response.Write(txtDateEnd.ClientID); %>').datepicker();
			bmContractFillSubItemsDetail('<% Response.Write(hdnDataRef.ClientID); %>', '<% Response.Write(tblContractDetail.ClientID); %>')
			bmContractSumTotal('<% Response.Write(tblContractDetail.ClientID); %>', 'subitem-price-total', 'bm-total', true);
			checkIsPaidWork.each(updateCheckIsPaidWork);
			checkIsPaidWork.click(updateCheckIsPaidWork);
			bmContractRestorePaidWork('<% Response.Write(tblPayment.ClientID); %>', '<% Response.Write(tblContractDetail.ClientID); %>', 'bm-total', '<% Response.Write(txtAdvance.ClientID); %>');
			bmContractPaidWorkSetBalance('<% Response.Write(tblPayment.ClientID); %>', '<% Response.Write(tblContractDetail.ClientID); %>', 'bm-total', '<% Response.Write(txtAdvance.ClientID); %>');
			bmInputDecimal('<% Response.Write(txtCurrencyRate.ClientID); %>');
			bmInputDecimal('<% Response.Write(txtAdvance.ClientID); %>');
			bmInputDecimalByObj(jQuery('#<% Response.Write(tblContractDetail.ClientID); %> input[name="subitem-price-total"]'));
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest);
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onBeginRequest);
		});
		function bmContractValidate(output) {
			var html = '';
			var dateStart = jQuery('#<% Response.Write(txtDateStart.ClientID); %>').datepicker('getDate');
			var dateEnd = jQuery('#<% Response.Write(txtDateEnd.ClientID); %>').datepicker('getDate');
			var contractor = jQuery('#<% Response.Write(ddlContractor.ClientID); %>').val();
			var currencyRate = jQuery('#<% Response.Write(txtCurrencyRate.ClientID); %>').val();
			var contractTotal = jQuery('#<% Response.Write(tblContractDetail.ClientID); %> .bm-total').text();
			var isPaidWork = jQuery('#<% Response.Write(chkIsPaidWork.ClientID); %>:checked');
			var errorHead = '&nbsp;<span class="ui-icon ui-icon-alert" style="float:left;margin-right:0.3em;"></span>Se encontraron los siguientes errores:';

			if (!contractor)
				html += '<li>Debe seleccionar contratista</li>';
				
			if (!dateStart)
				html += '<li>Fecha Inicio es requerida</li>';
			
			if (!dateEnd)
				html += '<li>Fecha Fin es requerida</li>';

			if (dateStart && dateEnd && dateStart >= dateEnd)
				html += '<li>Fecha Inicio debe ocurrir antes de Fecha Fin</li>';

			if (!currencyRate)
				html += '<li>Tasa de cambio es requerida</li>';

			if (Number(contractTotal) <= 0)
				html += '<li>Debe introducir el detalle del contrato o monto total no es válido</li>';

			if (isPaidWork.length == 1) {
				var advance = jQuery('#<% Response.Write(txtAdvance.ClientID); %>').val();
				var paymentPlan = jQuery('#<% Response.Write(tblPayment.ClientID); %>');
				var paymentDates = paymentPlan.find('input[name="payment-date"]');
				var paymentAmounts = paymentPlan.find('input[name="payment-amount"]');
				var paymentConditions = paymentPlan.find('input[name="payment-condition"]');
				
				if (Number(advance) <= 0)
					html += '<li>Anticipo es requerido</li>';

				if (paymentAmounts.length == 0)
					html += '<li>Debe definir un plan de pagos</li>';
				else {
					paymentDates.each(function(i) {
						var curDate = jQuery(this).datepicker('getDate');

						if (!curDate)
							html += '<li>Plan de pagos, fila ' + (i+1) + ' no tiene fecha</li>';
					});

					paymentAmounts.each(function(i) {
						var curAmount = jQuery(this).val();

						if (Number(curAmount) <= 0)
							html += '<li>Plan de pagos, fila ' + (i+1) + ' no tiene monto</li>';
					});

					if (Number(paymentConditions[paymentConditions.length - 1].value) != 100)
						html += '<li>Plan de pagos, debe acabar con avance de 100%</li>'
				}
			}
			
			if (html) {
				jQuery('#' + output).html(errorHead + '<ul>' + html + '</ul>');
				return false;
			}
			
			return true;
		}
	</script>
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" >
 </asp:Content>

 