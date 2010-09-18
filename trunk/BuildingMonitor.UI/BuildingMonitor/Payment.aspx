<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Payment.aspx.cs" Inherits="BuildingMonitor.UI.PaymentPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">

<div class="settingrow">
	<mp:SiteLabel ID="lblPayFrom" runat="server" CssClass="settinglabel" ConfigKey="PaymentPayFrom" ResourceFile="BuildingMonitorResources" />
	<asp:DropDownList ID="ddlPaymentFrom" runat="server"></asp:DropDownList>
</div>
<div class="settingrow">
	<mp:SiteLabel ID="lblPayTo" runat="server" CssClass="settinglabel" ConfigKey="PaymentPayTo" ResourceFile="BuildingMonitorResources" />
	<asp:DropDownList ID="ddlPaymentTo" runat="server"></asp:DropDownList>
	<br />
	<span id="check-number" style="display:none">
	<mp:SiteLabel ID="lblCheckNumber" runat="server" CssClass="settinglabel" ConfigKey="PaymentCheckNumber" ResourceFile="BuildingMonitorResources" />
	<asp:TextBox ID="txtCheckNumber" runat="server" MaxLength="15" />
	</span>
</div>
<div class="settingrow">
	<mp:SiteLabel ID="lblExchangeRate" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldCurrencyRate" ResourceFile="BuildingMonitorResources" />
	<asp:TextBox ID="txtExchangeRate" runat="server"/>
</div>

<asp:Table ID="tblProgress" runat="server" CssClass="bm-table">
	<asp:TableHeaderRow TableSection="TableHeader">
		<asp:TableHeaderCell Width="1%"><input type="checkbox" id="check-all-none" checked="checked"/></asp:TableHeaderCell>
		<asp:TableHeaderCell>SubItem</asp:TableHeaderCell>
		<asp:TableHeaderCell>Fecha</asp:TableHeaderCell>
		<asp:TableHeaderCell>Avance</asp:TableHeaderCell>
		<asp:TableHeaderCell>Monto <asp:Literal ID="ltrCurrency" runat="server" /></asp:TableHeaderCell>
	</asp:TableHeaderRow>
</asp:Table>
<div style="margin:0.3em 1% 0 0;font-weight:bold;text-align:right;display:none" id="total-payment">
Total a pagar (<% Response.Write(ltrCurrency.Text); %>): <span class="total-payment"></span>
</div>

<div style="margin:0.5em 0;clear:both">
	<asp:Button ID="btnOk" runat="server" Text="Continuar" OnClick="btnOk_Click" OnClientClick="return verifyInput()" /> 
	<asp:Button ID="btnCancel" runat="server" Text="Volver" OnClick="btnCancel_Click" />
	<div style='padding:0.2em 1em;margin:0.5em 0'>
		<div id="trace-error" class="ui-state-error ui-corner-all"><asp:Literal ID="litTraceError" runat="server" EnableViewState="false" /></div>
	</div>
</div>

<script type="text/javascript">
	jQuery(document).ready(function() {
		jQuery('#<% Response.Write(tblProgress.ClientID); %> input:checkbox').click(function() {
			bmSetTotal();
		});
		jQuery('#check-all-none').click(function() {
			bmSelectCheckBoxes('#<% Response.Write(tblProgress.ClientID); %>', this.checked ? 'all' : 'none');
			bmSetTotal();
		});
		bmSelectCheckBoxes('#<% Response.Write(tblProgress.ClientID); %>', 'all');
		bmSetTotal();
		bmInputDecimal('<% Response.Write(txtExchangeRate.ClientID); %>');
		jQuery('#<% Response.Write(ddlPaymentTo.ClientID); %>').change(function() {
			jQuery('#check-number').css('display', this.value == 'check' ? '' : 'none');
		});
	});
	function bmSetTotal() {
		var total = bmCalcTotal();
		
		jQuery('#total-payment').css('display', total > 0 ? '' : 'none');
		jQuery('#<% Response.Write(btnOk.ClientID); %>').attr('disabled', total > 0 ? '' : 'disabled');
		jQuery('#total-payment .total-payment').text(total.toFixed(2));
	}
	function bmCalcTotal() {
		var total = 0;
		
		jQuery('#<% Response.Write(tblProgress.ClientID); %> input:checked').each(function() {
			total += Number(jQuery(this).parent().nextAll('.progress-payable').text());
		});

		return total;
	}
	function verifyInput() {
		var html = '';
		var excRate = jQuery('#<% Response.Write(txtExchangeRate.ClientID); %>').val();
		var paymentFrom = jQuery('#<% Response.Write(ddlPaymentFrom.ClientID); %>').val();
		var paymentTo = jQuery('#<% Response.Write(ddlPaymentTo.ClientID); %>').val();
		var checkNumber = jQuery('#<% Response.Write(txtCheckNumber.ClientID); %>').val();
		var errorHead = '&nbsp;<span class="ui-icon ui-icon-alert" style="float:left;margin-right:0.3em;"></span>Se encontraron los siguientes errores:';
			
		if (Number(excRate) <= 0)
			html += '<li>Debe introducir tasa de cambio</li>';

		if (paymentTo == '')
			html += '<li>Debe seleccionar una forma de pago</li>';
			
		if (paymentTo == 'check' && checkNumber.length < 5)
			html += '<li>Número de cheque no es correcto o es muy corto</li>';
		
		if (paymentTo != 'check' && paymentTo != 'cash' && paymentFrom == '')
			html += '<li>Debe seleccionar cuenta de origen para transferencias bancarias</li>';
			
		if (paymentTo != 'check' && paymentTo != 'cash' && paymentFrom == '')
			html += '<li>Debe seleccionar cuenta de origen para transferencias bancarias</li>';
			
		if (html) {
			jQuery('#trace-error').html(errorHead + '<ul>' + html + '</ul>');
			return false;
		}
		
		return true;
	}
</script>
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
	runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
