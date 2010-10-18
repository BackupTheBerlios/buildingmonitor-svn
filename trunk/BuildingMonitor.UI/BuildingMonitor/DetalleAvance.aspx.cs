using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
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

	public partial class DetalleAvance : mojoBasePage
	{
		decimal _PromedioGralAvance = -1;
		private int _pageId = -1;
		private int _moduleId = -1;
		private int[] _totalCount = new int[3];
		
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
		}

		private void PopulateControls()
		{
			ComboProyecto();
			ComboContratista();
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

		private void ComboContratista()
		{
			if (!IsPostBack)
			{
				using (IDataReader dr = Contractor.GetAll())
				{
					m_cmbContratista.DataSource = dr;
					m_cmbContratista.DataBind();
				}

				m_cmbContratista.Items.Insert(0, new ListItem("", "-1"));
				m_cmbContrato.Items.Clear();
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

		protected void m_cmbContratista_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboContrato();
		}

		private void ComboContrato()
		{
			if (!IsPostBack)
				return;

			using (DataSet dr = Contract.GetAllContract(Convert.ToInt32(m_cmbContratista.SelectedValue)))
			{
				m_cmbContrato.DataSource = dr;
				m_cmbContrato.DataBind();
			}

			m_cmbContrato.Items.Insert(0, new ListItem("", "-1"));
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			int IdProyecto = -1;
			int IdBloque = -1;
			int IdObra = -1;
			int IdGrupo = -1;
			int IdContratista = -1;
			int IdContrato = -1;
			
			int.TryParse(m_cmbProyecto.SelectedValue, out IdProyecto);
			int.TryParse(m_cmbBloque.SelectedValue, out IdBloque);
			int.TryParse(m_cmbObra.SelectedValue, out IdObra);
			int.TryParse(m_cmbGrupo.SelectedValue, out IdGrupo);
			int.TryParse(m_cmbContratista.SelectedValue, out IdContratista);
			int.TryParse(m_cmbContrato.SelectedValue, out IdContrato);

			using (DataSet ds = ProgressReport.DetalleAvance(IdProyecto, IdBloque, IdObra, IdGrupo, IdContratista, IdContrato))
			{
				gridView.DataSource = ds.Tables[0].DefaultView;
				gridView.DataBind();
				//MostrarResumen();
				
				decimal dTotal = 0M;
				foreach (DataRow row in ds.Tables[0].Rows)
					dTotal += Convert.ToDecimal(row["SubTotal"]);
				
				litTotal.Text = Convert.ToString(Math.Round(dTotal,2),System.Globalization.CultureInfo.InvariantCulture);
			}			
		}

				
		protected decimal GetImgFilename(object obj)
		{

			return 34.57M;
		}

		

	}
}
