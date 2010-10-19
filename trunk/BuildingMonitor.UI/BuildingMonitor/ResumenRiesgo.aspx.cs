using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using BuildingMonitor.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace BuildingMonitor.UI
{

	public partial class ResumenRiesgo : mojoBasePage
	{
		decimal _PromedioGralAvance = -1;
		private int _pageId = -1;
		private int _moduleId = -1;
		private int[] _colCantidad = new int[4] {0,0,0,0};
		
		protected void Page_Load(object sender, EventArgs e)
		{
			LoadParams();

			// one of these may be usefull
			if (!UserCanViewPage(_moduleId))
			{
				SiteUtils.RedirectToAccessDeniedPage();
				return;
			}
			if (!UserCanEditModule(_moduleId))
			{
				SiteUtils.RedirectToAccessDeniedPage();
				return;
			}

			LoadSettings();
			PopulateLabels();
			PopulateControls();
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			Load += new EventHandler(Page_Load);
		}

		private void LoadParams()
		{
			_pageId = WebUtils.ParseInt32FromQueryString("pageid", _pageId);
			_moduleId = WebUtils.ParseInt32FromQueryString("mid", _moduleId);
			//_contractorId = WebUtils.ParseInt32FromQueryString("contractorid", _contractorId);
		}

		private void LoadSettings()
		{

		}

		private void PopulateControls()
		{
			ComboProyecto();
		}

		private void PopulateLabels()
		{
			//this.litHeading.Text = BuildingMonitorResources.Contractor;
			//this.litSettingsTab.Text = BuildingMonitorResources.Contractor;
			//this.litSpecialtiesTab.Text = BuildingMonitorResources.Specialties;
			//this.litStatusTab.Text = BuildingMonitorResources.Status;
			//this.btnSave.Text = BuildingMonitorResources.LabelSave;
			//this.btnDelete.Text = BuildingMonitorResources.LabelDelete;
		}

		private void ComboProyecto()
		{
			if (!IsPostBack)
			{
				using (IDataReader dr = Project.GetAll())
				{
					m_cmbProyecto.DataSource = dr;//  m_bd.FillCombo(, "select '', -1 union select Convert(varchar(10), Id) + ' - ' + Nombre,Id from uProyecto", 0, 1);
					m_cmbProyecto.DataBind();
				}
				
				m_cmbProyecto.Items.Insert(0,new ListItem("","-1"));
			}
		}

		protected void m_cmbProyecto_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboGrupo();
		}

		private void ComboGrupo()
		{
			if (IsPostBack)
			{
				using (IDataReader dr = ListGroup.GetAll())
				{
					m_cmbGrupo.DataSource = dr;//  m_bd.FillCombo(, "select '', -1 union select Convert(varchar(10), Id) + ' - ' + Nombre,Id from uProyecto", 0, 1);
					m_cmbGrupo.DataBind();
				}

				m_cmbGrupo.Items.Insert(0, new ListItem("", "-1"));
			}
		}
		
		protected void Button1_Click(object sender, EventArgs e)
		{
			int IdProyecto = Convert.ToInt32(m_cmbProyecto.SelectedValue);
			int IdGrupo    = Convert.ToInt32(m_cmbGrupo.SelectedValue);
			int nTotal     = 1;
			//bool  bSortBloque = ((CheckBox)gridView.FindControl("chkBloque")).Checked;
			//bool  bSortPorcentajeAvance = ((CheckBox)gridView.FindControl("chkPorcentajeAvance")).Checked;
			using (DataSet ds = ProgressReport.AvancePorcentual_Bloque(IdProyecto, IdGrupo))
			{
				_PromedioGralAvance = 100 * Convert.ToDecimal(ds.Tables[2].Rows[0]["MontoEjecutado"]) / Convert.ToDecimal(ds.Tables[3].Rows[0]["MontoPresupuestado"]);
				
				ds.Tables[0].DefaultView.Sort = "PorcentajeAvance desc ";
				gridView.DataSource = ds.Tables[0].DefaultView;//  m_bd.FillCombo(, "select '', -1 union select Convert(varchar(10), Id) + ' - ' + Nombre,Id from uProyecto", 0, 1);
				gridView.DataBind();

				nTotal = Math.Max(_colCantidad[0] + _colCantidad[1] + _colCantidad[2] + _colCantidad[3], 1);
				lblGeneral.Text = string.Format("Avance General: {0}%", Helpers.Formatter.Decimal(_PromedioGralAvance));
				lblCritico.Text = string.Format(" Total: {0}  Porcentaje: {1}%", _colCantidad[0], Helpers.Formatter.Decimal((decimal)100 * _colCantidad[0] / nTotal));
				lblRojo.Text = string.Format(" Total: {0}  Porcentaje: {1}%", _colCantidad[1], Helpers.Formatter.Decimal((decimal)100 * _colCantidad[1] / nTotal));
				lblAmarillo.Text = string.Format(" Total: {0}  Porcentaje: {1}%", _colCantidad[2], Helpers.Formatter.Decimal((decimal)100 * _colCantidad[2] / nTotal));
				lblVerde.Text = string.Format(" Total: {0}  Porcentaje: {1}%", _colCantidad[3], Helpers.Formatter.Decimal((decimal)100 * _colCantidad[3] / nTotal));
				
				CargarResumen();
			}			
		}
		
		private void CargarResumen()
		{
			int IdProyecto = Convert.ToInt32(m_cmbProyecto.SelectedValue);
			int IdGrupo    = Convert.ToInt32(m_cmbGrupo.SelectedValue);
			string strBloque = string.Empty;
			int[] colCantidad = new int[4] {0,0,0,0};
			int[] colCantidadSum = new int[4] { 0, 0, 0, 0 };
			DataTable dt = new DataTable();
			DataSet ds = ProgressReport.AvancePorcentual(IdProyecto,-1,-1, IdGrupo);
			
			dt.Columns.Add("Bloque", System.Type.GetType("System.String"));
			dt.Columns.Add("Critico", System.Type.GetType("System.Int32"));
			dt.Columns.Add("Rojo", System.Type.GetType("System.Int32"));
			dt.Columns.Add("Amarillo", System.Type.GetType("System.Int32"));
			dt.Columns.Add("Verde", System.Type.GetType("System.Int32"));

			ds.Tables[0].DefaultView.Sort = "Bloque";
			
			
				
			int nIndex = 0;	
			while (nIndex < ds.Tables[0].Rows.Count)
			{
				DataRow row = ds.Tables[0].DefaultView[nIndex].Row;//ds.Tables[0].Rows[nIndex];
				
				if ( strBloque != row["Bloque"].ToString() )
				{
					colCantidad[0] = 0; colCantidad[1] = 0; colCantidad[2] = 0; colCantidad[3] = 0;
					strBloque = row["Bloque"].ToString();
				}
				
				while (strBloque == row["Bloque"].ToString() && nIndex < ds.Tables[0].Rows.Count)
				{
					decimal dPorcentajeAvance =	Convert.ToDecimal(row["PorcentajeAvance"]);

					if (dPorcentajeAvance < _PromedioGralAvance - 10M)//rojo
					{
						colCantidad[0]++;
						colCantidadSum[0]++;
					}
					else if ( dPorcentajeAvance < _PromedioGralAvance - 5M )//rojo
					{
						colCantidad[1]++;
						colCantidadSum[1]++;
					}
					else if (dPorcentajeAvance < _PromedioGralAvance + 5M)//naranja
					{
						colCantidad[2]++;
						colCantidadSum[2]++;
					}
					else 
					{
						colCantidad[3]++;
						colCantidadSum[3]++;
					}

					nIndex++;
					
					if ( nIndex < ds.Tables[0].Rows.Count )
						row = ds.Tables[0].DefaultView[nIndex].Row; //ds.Tables[0].Rows[nIndex];
				}
				
				DataRow rowNew = dt.NewRow();
				rowNew["Bloque"] = strBloque;
				rowNew["Critico"] = colCantidad[0];
				rowNew["Rojo"] = colCantidad[1];
				rowNew["Amarillo"] = colCantidad[2];
				rowNew["Verde"] = colCantidad[3];
				dt.Rows.Add(rowNew);
											
			}

			dt.DefaultView.Sort = "Critico,Rojo,Amarillo,Verde desc ";
			gridViewResumen.DataSource = dt.DefaultView;
			gridViewResumen.DataBind();

			int nTotal = Math.Max(colCantidadSum[0] + colCantidadSum[1] + colCantidadSum[2] + colCantidadSum[3], 1);
			lblCriticoResumen.Text = string.Format(" Total: {0}  Porcentaje: {1}%", colCantidadSum[0], Helpers.Formatter.Decimal((decimal)100 * colCantidadSum[0] / nTotal));
			lblRojoResumen.Text = string.Format(" Total: {0}  Porcentaje: {1}%", colCantidadSum[1], Helpers.Formatter.Decimal((decimal)100 * colCantidadSum[1] / nTotal));
			lblAmarilloResumen.Text = string.Format(" Total: {0}  Porcentaje: {1}%", colCantidadSum[2], Helpers.Formatter.Decimal((decimal)100 * colCantidadSum[2] / nTotal));
			lblVerdeResumen.Text = string.Format(" Total: {0}  Porcentaje: {1}%", colCantidadSum[3], Helpers.Formatter.Decimal((decimal)100 * colCantidadSum[3] / nTotal));
			
			//
			dt.Dispose();
			ds.Dispose();
		}
		
		protected string algo(object obj)
		{
			decimal dPorcentajeAvance = Convert.ToDecimal(obj);

			if (dPorcentajeAvance < _PromedioGralAvance - 5M)//rojo
			{
				_colCantidad[0]++;
				return "Critico.jpg";
			}
			if ( dPorcentajeAvance < _PromedioGralAvance - 3.5M )//rojo
			{
				_colCantidad[1]++;
				return "Rojo.jpg";
			}
			if (dPorcentajeAvance < _PromedioGralAvance + 4.5M)//naranja
			{
				_colCantidad[2]++;
				return "Amarillo.jpg";
			}

			_colCantidad[3]++;
			return "Verde.jpg";
		}

	}
}
