<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="DetalleAvance.aspx.cs" Inherits="BuildingMonitor.UI.DetalleAvance" %>

<%@ Register src="Controls/NavProjectToItem.ascx" tagname="NavProjectToItem" tagprefix="uc1" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
	<div>
	<table class="bm-table"> 
	<tr class="bm-group-hd"> 
		<td style="width:18%">Proyecto:
			<br /><asp:DropDownList ID="m_cmbProyecto" runat="server" AutoPostBack="True" DataValueField="ProjectId" DataTextField="Name" onselectedindexchanged="m_cmbProyecto_SelectedIndexChanged" />
		</td> 
		<td style="width:18%">Bloque:
			<br /><asp:DropDownList ID="m_cmbBloque" runat="server" AutoPostBack="True" DataValueField="BlockId" DataTextField="Name" onselectedindexchanged="m_cmbBloque_SelectedIndexChanged" />
		</td> 
		<td style="width:18%">Obra:
			<asp:DropDownList ID="m_cmbObra" runat="server" AutoPostBack="True" DataValueField="WorkId" DataTextField="Name"/>
		</td>
		<td style="width:18%">Grupo:
			<br /><asp:DropDownList ID="m_cmbGrupo" runat="server" AutoPostBack="False" DataValueField="Id" DataTextField="Nombre" />
		</td>		
	</tr> 
	
	</table>
	<table class="bm-table"> 
	<tr class="bm-group-hd"> 
	<td style="width:18%">Contratista:
			<br /><asp:DropDownList ID="m_cmbContratista" runat="server" 
			AutoPostBack="True" DataValueField="IdContractor" DataTextField="Name" 
			onselectedindexchanged="m_cmbContratista_SelectedIndexChanged" />
		</td>
		<td style="width:10%">Contrato:
			<br /><asp:DropDownList ID="m_cmbContrato" runat="server" AutoPostBack="False" DataValueField="Id" DataTextField="Glosa" />
		</td>
	</tr>
	</table>
	<asp:Button ID="Button1" runat="server" Text="Aceptar" onclick="Button1_Click" />
    </div>    
    <table class="bm-table" id="tbl">
		<thead>
		<tr>
			<th>Contratista</th><th>Obra</th><th>Grupo</th><th>Item</th><th>SubItem</th><th>&nbsp;Avance&nbsp;</th><th align="right">Cantidad</th><th>SubTotal</th>
		</tr>
		</thead>
		<tbody>
			<asp:Repeater ID = "gridView" runat="server">
			<ItemTemplate>
			<tr>
				<td><%# Eval("Contratista") %></td>
				<td><%# Eval("Obra") %></td>
				<td><%# Eval("Grupo") %></td>
				<td><%# Eval("Item") %></td>
				<td><%# Eval("SubItem") %></td>
				<td class="bm-number"><%# BuildingMonitor.UI.Helpers.Formatter.Decimal(Eval("Avance"))%> %</td>
				<td align="right"><%# Eval("Cantidad")%></td>
				<td class="bm-number"><%# BuildingMonitor.UI.Helpers.Formatter.Decimal(Eval("SubTotal"))%></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
			

		</tbody>
		<TFOOT>
		<tr>
			<td>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td align="right">Total.-</td>
			<td align="right"><asp:Literal ID="litTotal" runat="server" /></td>
		</tr>
		</TFOOT>
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

