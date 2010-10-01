<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ResumenRiesgo.aspx.cs" Inherits="BuildingMonitor.UI.ResumenRiesgo" %>

<%@ Register src="Controls/NavProjectToItem.ascx" tagname="NavProjectToItem" tagprefix="uc1" %>


<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<script type="text/javascript" src="../ClientScript/jquery/i18n/ui.datepicker-<% Response.Write(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName); %>.js"></script>
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
    
    <table width="100%" > 
            <tr style ="background:cyan"> 
               <td class="style1" align="left"  style="width:25%">
                    <asp:Label ID="Label1" runat="server" Text="Proyecto"></asp:Label>
                    <asp:DropDownList ID="m_cmbProyecto" runat="server" AutoPostBack="True" DataValueField = "ProjectId" DataTextField = "Name" 
                        Height="24px" Width="100%"  BackColor="#CCFFFF" style="margin-right: 0px" 
						onselectedindexchanged="m_cmbProyecto_SelectedIndexChanged"></asp:DropDownList>
               </td>                               
               <td align="left" style="width:35%" >
                    <asp:Label ID="Label13" runat="server" Text="Grupo"></asp:Label>
                    <asp:DropDownList ID="m_cmbGrupo" runat="server" AutoPostBack="False" DataValueField = "Id" DataTextField = "Nombre" 
                        Height="24px" Width="100%" BackColor="#CCFFFF" ></asp:DropDownList>               
               </td>
               <td align="left" style="width:40%" >
               <div class="settingrow">
                    <mp:SiteLabel ID="lblDateStart" runat="server" CssClass="settinglabel" ConfigKey="ContractFieldDateStart" ResourceFile="BuildingMonitorResources" />
					<asp:TextBox ID="txtDateStart" runat="server" />   
					</div>        
               </td>                            
               
            </tr> 
        </table>
        
        
    
    </div>
    <div>
    <asp:Button ID="Button1" runat="server" Text="Aceptar" onclick="Button1_Click" />              
    </div>         
    <div style="margin:0.5em 0.5em">

    <asp:Label ID="lblGeneral" runat = "server"  ></asp:Label><br /><br />
    <div align="center" style="font-weight: bold">
    <asp:Label ID="Label2" runat = "server" Text="Porcentaje de Avance por Bloque"></asp:Label><br /><br />
    </div>
    <img src="Img/Rojo.jpg" style="vertical-align:middle" /><asp:Label ID="lblRojo" runat = "server"  ></asp:Label> 
    <img src="Img/Amarillo.jpg" style="vertical-align:middle" /><asp:Label ID="lblAmarillo" runat = "server"  ></asp:Label>
    <img src="Img/Verde.jpg" style="vertical-align:middle" /><asp:Label ID="lblVerde" runat = "server"  ></asp:Label>
    </div>

    <table class="bm-table" id="tbl">
    <asp:Repeater ID = "gridView" runat ="server">
    <HeaderTemplate >
    <thead>
    <tr>
    <th><asp:CheckBox ID="chkBloque" runat="server" />Bloque</th><th>Supervisor</th><th><asp:CheckBox ID="chkPorcentajeAvance" runat="server" />Porcentaje Avance</th><th>Peso Especifico</th><th style="width:16px" >&nbsp;</th>
    </tr>
    </thead>
    </HeaderTemplate>
    <ItemTemplate>
    <tr  >
    <td>
    <%# Eval("Bloque") %>
    </td>
    <td>
    <%# Eval("Supervisor") %>
    </td>
    <td class="bm-number">
    <%# BuildingMonitor.UI.Helpers.Formatter.Decimal(Eval("PorcentajeAvance")) %>%
    </td>
    <td class="bm-number">
    <%# BuildingMonitor.UI.Helpers.Formatter.Decimal(Eval("PesoEspecifico"))%>
    </td>
    <td><img src="img/<%# algo(Eval("PorcentajeAvance"))%>" alt="" /></td>
    </tr>
    </ItemTemplate>
    </asp:Repeater>
    </table>
    
    <br /><br />
    <div align="center" style="font-weight: bold">
    <asp:Label ID="Label3" runat = "server" Text="An&aacute;lisis de Riesgo por Bloque"   ></asp:Label><br /><br />
    </div>
    <div style="margin:0.5em 0.5em">
    <img src="Img/Rojo.jpg" style="vertical-align:middle" /><asp:Label ID="lblRojoResumen" runat = "server"  ></asp:Label> 
    <img src="Img/Amarillo.jpg" style="vertical-align:middle" /><asp:Label ID="lblAmarilloResumen" runat = "server"  ></asp:Label>
    <img src="Img/Verde.jpg" style="vertical-align:middle" /><asp:Label ID="lblVerdeResumen" runat = "server"  ></asp:Label>
    </div>
    <table class="bm-table" id="tblResumen">
    <asp:Repeater ID = "gridViewResumen" runat ="server">
    <HeaderTemplate >
    <thead>
    <tr>
    <th>Bloque</th><th><img src="img/Rojo.jpg" alt="" /></th><th><img src="img/Amarillo.jpg" alt="" /></th><th><img src="img/Verde.jpg" alt="" /></th>
    </thead>
    </HeaderTemplate>
    <ItemTemplate>
    <tr  >
    <td>
    <%# Eval("Bloque") %>
    </td>
    <td class="bm-number">
    <%# Eval("Rojo") %>
    </td>
    <td class="bm-number">
    <%# Eval("Amarillo") %>
    </td>
    <td class="bm-number">
    <%# Eval("Verde") %>    
    </ItemTemplate>
    </asp:Repeater>
    </table>
    
        <script type="text/javascript">
        	jQuery(document).ready(function() {
        	jQuery('#tbl tbody tr:odd').addClass('alternate');
        	jQuery('#tblResumen tbody tr:odd').addClass('alternate');

        	jQuery.datepicker.setDefaults(jQuery.datepicker.regional['<% Response.Write(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName); %>']);
        	jQuery.datepicker.setDefaults({ duration: 'fast', changeMonth: true, changeYear: true });
        	jQuery('#<% Response.Write(txtDateStart.ClientID); %>').datepicker();
        		
        	})
        </script>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
	runat="server" >
	
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" 
	runat="server" >
</asp:Content>

