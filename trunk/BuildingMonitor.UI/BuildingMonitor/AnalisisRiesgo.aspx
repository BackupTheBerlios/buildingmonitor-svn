<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AnalisisRiesgo.aspx.cs" Inherits="BuildingMonitor.UI.AnalisisRiesgo" %>

<%@ Register src="Controls/NavProjectToItem.ascx" tagname="NavProjectToItem" tagprefix="uc1" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
	<div class="no-print">
	<table class="bm-table"> 
	<tr class="bm-group-hd"> 
		<td style="width:25%">Proyecto:
			<asp:DropDownList ID="m_cmbProyecto" runat="server" AutoPostBack="True" DataValueField="ProjectId" DataTextField="Name" onselectedindexchanged="m_cmbProyecto_SelectedIndexChanged" />
		</td> 
		<td style="width:25%">Bloque:
			<asp:DropDownList ID="m_cmbBloque" runat="server" AutoPostBack="True" DataValueField="BlockId" DataTextField="Name" onselectedindexchanged="m_cmbBloque_SelectedIndexChanged" />
		</td> 
		<td style="width:25%">Obra:
			<asp:DropDownList ID="m_cmbObra" runat="server" AutoPostBack="True" DataValueField="WorkId" DataTextField="Name"/>
		</td>
		<td style="width:25%">Grupo:
			<asp:DropDownList ID="m_cmbGrupo" runat="server" AutoPostBack="False" DataValueField="Id" DataTextField="Nombre" />
		</td>
	</tr> 
	</table>
	<asp:Button ID="Button1" runat="server" Text="Aceptar" onclick="Button1_Click" />
    </div>
    <div style="margin:0.5em 0">
    <asp:Literal ID="litAvanceGral" runat="server" />
    <img src="Img/Critico.jpg" style="vertical-align:middle" /> <asp:Literal ID="litCritico" runat="server" />
    <img src="Img/Rojo.jpg" style="vertical-align:middle" /> <asp:Literal ID="litRed" runat="server" />
    <img src="Img/Amarillo.jpg" style="vertical-align:middle" /> <asp:Literal ID="litYellow" runat="server" />
    <img src="Img/Verde.jpg" style="vertical-align:middle" /> <asp:Literal ID="litGreen" runat="server" />
    </div>
    <table class="bm-table" id="tbl">
		<thead>
		<tr>
			<th><asp:CheckBox ID="chkBloque" runat="server" />Bloque</th>
			<th>Supervisor</th><th>Obra</th>
			<th><asp:CheckBox ID="chkPorcentajeAvance" runat="server" />Porcentaje Avance</th>
			<th>Peso Especifico</th>
			<th style="width:1%">&nbsp;</th>
		</tr>
		</thead>
		<tbody>
			<asp:Repeater ID = "gridView" runat="server">
			<ItemTemplate>
			<tr>
				<td><%# Eval("Bloque") %></td>
				<td><%# Eval("Supervisor") %></td>
				<td><%# Eval("Obra") %></td>
				<td class="bm-number"><%# BuildingMonitor.UI.Helpers.Formatter.Decimal(Eval("PorcentajeAvance")) %> %</td>
				<td class="bm-number"><%# BuildingMonitor.UI.Helpers.Formatter.Decimal(Eval("PesoEspecifico"))%></td>
				<td><img src="Img/<%# GetImgFilename(Eval("PorcentajeAvance"))%>" alt="" /></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
	<script type="text/javascript">
		jQuery(document).ready(function() {
			jQuery('#tbl tbody tr:odd').addClass('alternate');
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

