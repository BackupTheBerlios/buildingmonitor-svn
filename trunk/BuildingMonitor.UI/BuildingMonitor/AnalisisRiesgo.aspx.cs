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

	public partial class AnalisisRiesgo : mojoBasePage
	{
		decimal _PromedioGralAvance = -1;
		private int _pageId = -1;
		private int _moduleId = -1;
		private int[] _totalCount = new int[4];
		
		protected void Page_Load(object sender, EventArgs e)
		{
			LoadParams();

			// one of these may be usefull
			if (!UserCanViewPage(_moduleId))
			{
				SiteUtils.RedirectToAccessDeniedPage();
				return;
			}
			//if (!UserCanEditModule(_moduleId))
			//{
			//    SiteUtils.RedirectToAccessDeniedPage();
			//    return;
			//}

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
		}

		private void LoadSettings()
		{
			_totalCount[0] = 0;
			_totalCount[1] = 0;
			_totalCount[2] = 0;
			_totalCount[3] = 0;
		}

		private void PopulateControls()
		{
			ComboProyecto();
		}

		private void PopulateLabels()
		{
		}

		private void ComboProyecto()
		{
			if (!IsPostBack)
			{
				using (IDataReader dr = Project.GetAll())
				{
					m_cmbProyecto.DataSource = dr;
					m_cmbProyecto.DataBind();
				}
				
				m_cmbProyecto.Items.Insert(0, new ListItem("", "-1"));
				m_cmbBloque.Items.Clear();
				m_cmbObra.Items.Clear();
			}
		}

		private void ComboBloque()
		{
			if (!IsPostBack)
				return;

			using (IDataReader dr = Block.GetAll(Convert.ToInt32(m_cmbProyecto.SelectedValue)))
			{
				m_cmbBloque.DataSource = dr;
				m_cmbBloque.DataBind();
			}

			m_cmbBloque.Items.Insert(0, new ListItem("", "-1"));
			m_cmbObra.Items.Clear();
		}

		protected void m_cmbProyecto_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBloque();
			ComboGrupo();
		}

		private void ComboObra()
		{
			if (!IsPostBack)
				return;

			int projectId = -1;
			int blockId = -1;

			int.TryParse(m_cmbProyecto.SelectedValue, out projectId);
			int.TryParse(m_cmbBloque.SelectedValue, out blockId);

			using (IDataReader dr = Work.GetAll(projectId, blockId))
			{
				m_cmbObra.DataSource = dr;
				m_cmbObra.DataBind();
			}

			m_cmbObra.Items.Insert(0, new ListItem("", "-1"));
		}

		private void ComboGrupo()
		{
			if (!IsPostBack)
				return;
			
			using (IDataReader dr = ListGroup.GetAll())
			{
				m_cmbGrupo.DataSource = dr;
				m_cmbGrupo.DataBind();
			}

			m_cmbGrupo.Items.Insert(0, new ListItem("", "-1"));
		}

		protected void m_cmbBloque_SelectedIndexChanged(object sender, EventArgs e)
		{
			 ComboObra();
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			int IdProyecto = -1;
			int IdBloque = -1;
			int IdObra = -1;
			int IdGrupo = -1;
			bool bSortBloque = chkBloque.Checked;
			bool bSortPorcentajeAvance = chkPorcentajeAvance.Checked;

			int.TryParse(m_cmbProyecto.SelectedValue, out IdProyecto);
			int.TryParse(m_cmbBloque.SelectedValue, out IdBloque);
			int.TryParse(m_cmbObra.SelectedValue, out IdObra);
			int.TryParse(m_cmbGrupo.SelectedValue, out IdGrupo);

			using (DataSet ds = ProgressReport.AvancePorcentual(IdProyecto, IdBloque, IdObra, IdGrupo))
			{
				if (ds.Tables[2].Rows.Count > 0 && ds.Tables[3].Rows.Count > 0 && Convert.ToDecimal(ds.Tables[3].Rows[0]["MontoPresupuestado"]) > 0)
					_PromedioGralAvance = 100 * Convert.ToDecimal(ds.Tables[2].Rows[0]["MontoEjecutado"]) / Convert.ToDecimal(ds.Tables[3].Rows[0]["MontoPresupuestado"]);
				else
					_PromedioGralAvance = 0;
				
				if (bSortBloque && bSortPorcentajeAvance)
					ds.Tables[0].DefaultView.Sort = "Bloque, PorcentajeAvance desc ";
				else if (bSortPorcentajeAvance)
					ds.Tables[0].DefaultView.Sort = "PorcentajeAvance desc ";
				else
					ds.Tables[0].DefaultView.Sort = "Bloque, PorcentajeAvance desc ";
					
				gridView.DataSource = ds.Tables[0].DefaultView;
				gridView.DataBind();
				MostrarResumen();
			}			
		}

		private void MostrarResumen()
		{
			int total = Math.Max(_totalCount[0] + _totalCount[1] + _totalCount[2] + _totalCount[3], 1);

			litCritico.Text = string.Format("Total: {0} &nbsp; Porcentaje: {1}% &nbsp; ", _totalCount[0], Helpers.Formatter.Decimal(Math.Round((decimal)100 * _totalCount[0] / total, 2)));
			litRed.Text = string.Format("Total: {0} &nbsp; Porcentaje: {1}% &nbsp; ", _totalCount[1], Helpers.Formatter.Decimal(Math.Round((decimal)100 * _totalCount[1] / total, 2)));
			litYellow.Text = string.Format("Total: {0} &nbsp; Porcentaje: {1}% &nbsp; ", _totalCount[2], Helpers.Formatter.Decimal(Math.Round((decimal)100 * _totalCount[2] / total, 2)));
			litGreen.Text = string.Format("Total: {0} &nbsp; Porcentaje: {1}% &nbsp; ", _totalCount[3], Helpers.Formatter.Decimal(Math.Round((decimal)100 * _totalCount[3] / total, 2)));
			litAvanceGral.Text = string.Format("Avance General: {0}%<br /><br />", Helpers.Formatter.Decimal(_PromedioGralAvance));
		}
		
		protected string GetImgFilename(object obj)
		{
			decimal dPorcentajeAvance = Convert.ToDecimal(obj);

			if (dPorcentajeAvance < _PromedioGralAvance - 10)
			{
				_totalCount[0]++;
				return "Critico.jpg";
			}

			if (dPorcentajeAvance < _PromedioGralAvance - 5)
			{
				_totalCount[1]++;
				return "Rojo.jpg";
			}

			if (dPorcentajeAvance < _PromedioGralAvance + 5)
			{
				_totalCount[2]++;
				return "Amarillo.jpg";
			}

			_totalCount[3]++;

			return "Verde.jpg";
		}

	}
}
