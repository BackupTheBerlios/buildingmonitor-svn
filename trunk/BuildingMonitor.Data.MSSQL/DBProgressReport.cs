using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using mojoPortal.Data;

namespace BuildingMonitor.Data
{
	public class DBProgressReport
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}

		public static DataSet AvancePorcentual_Bloque(int IdProyecto, int IdGrupo)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT  pr.Nombre Proyecto,b.Id IdBloque, b.Nombre Bloque,b.Encargado Supervisor,sum((a.AvanceActual - a.AvanceInicial)*cd.Precio/100) MontoEjecutado, convert(decimal(15,2),0) AS MontoPresupuestado,convert(decimal(15,2),0) AS PorcentajeAvance,convert(decimal(15,2),0) AS PesoEspecifico  ");
			sb.Append("FROM uAvance a,uContratoDetalle cd,uProyecto pr,uBloque b,uObra o,uGrupo g,uListaGrupo lg,uItem i,uListaItem li,uSubItem si ");
			sb.Append("WHERE a.IdSubItem = cd.IdSubItem and a.IdContrato = cd.IdContrato and o.Plantilla = 0 and a.IdProyecto = pr.Id and a.IdBloque = b.Id and ");
			sb.Append("a.IdObra = o.Id and a.IdGrupo = g.Id and g.IdListaGrupo = lg.Id and a.IdItem = i.Id and i.IdListaItem = li.Id and a.IdSubItem = si.Id ");
			sb.AppendFormat("and pr.Id ={0} ", IdProyecto);
			if (IdGrupo > 0) sb.AppendFormat("and g.IdListaGrupo ={0} ", IdGrupo);
			sb.Append("GROUP by pr.Nombre,b.Id, b.Nombre,b.Encargado ");
			sb.Append("ORDER by b.Nombre;");

			sb.Append("select p.Nombre Proyecto,b.Id IdBloque, b.Nombre Bloque,sum(i.MontoGlobalBs * si.Cantidad) MontoPresupuestado ");
			sb.Append("FROM  uProyecto p, uBloque b, uObra o, uGrupo g, uItem i, uSubItem si,uListaGrupo lg ");
			sb.Append("WHERE  i.IdProyecto = p.Id and i.IdBloque = b.Id and i.IdObra = o.Id and i.IdGrupo = g.Id and i.Id = si.IdItem and o.Plantilla = 0 and o.Id = i.IdObra and g.IdListaGrupo = lg.id ");
			sb.AppendFormat("and p.Id ={0} ", IdProyecto);
			if (IdGrupo > 0) sb.AppendFormat("and g.IdListaGrupo ={0} ", IdGrupo);
			sb.Append("GROUP by p.Nombre,b.Id, b.Nombre ");
			sb.Append("ORDER by b.Nombre DESC;");

			sb.Append("select  pr.Nombre Proyecto,sum((a.AvanceActual - a.AvanceInicial)*cd.Precio/100) MontoEjecutado ");
			sb.Append("from uAvance a,uContratoDetalle cd,uProyecto pr,uBloque b,uObra o,uGrupo g,uListaGrupo lg,uItem i,uListaItem li,uSubItem si ");
			sb.Append("where a.IdSubItem = cd.IdSubItem and a.IdContrato = cd.IdContrato and o.Plantilla = 0 and  ");
			sb.Append("a.IdProyecto = pr.Id and a.IdBloque = b.Id and a.IdObra = o.Id and a.IdGrupo = g.Id and g.IdListaGrupo = lg.Id and ");
			sb.Append("a.IdItem = i.Id and i.IdListaItem = li.Id and a.IdSubItem = si.Id ");
			sb.AppendFormat("and pr.Id ={0} ", IdProyecto);
			if (IdGrupo > 0) sb.AppendFormat("and g.IdListaGrupo ={0} ", IdGrupo);
			sb.Append("group by pr.Nombre; ");

			sb.Append("select p.Nombre Proyecto,sum(i.MontoGlobalBs * si.Cantidad) MontoPresupuestado ");
			sb.Append("FROM  uProyecto p, uBloque b, uObra o, uGrupo g, uItem i, uSubItem si,uListaGrupo lg ");
			sb.Append("WHERE  i.IdProyecto = p.Id and i.IdBloque = b.Id and i.IdObra = o.Id and i.IdGrupo = g.Id and i.Id = si.IdItem and o.Plantilla = 0 and o.Id = i.IdObra and g.IdListaGrupo = lg.id ");
			sb.AppendFormat("and p.Id ={0} ", IdProyecto);
			if (IdGrupo > 0) sb.AppendFormat("and g.IdListaGrupo ={0} ", IdGrupo);
			sb.Append("GROUP by p.Nombre ");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			DataSet ds = sph.ExecuteDataset();

			decimal dSumMontoPresupuestado = 0;
			foreach (DataRow row in ds.Tables[1].Rows)
				dSumMontoPresupuestado += Convert.ToDecimal(row["MontoPresupuestado"]);

			foreach (DataRow row in ds.Tables[0].Rows)
			{
				DataRow r = null;
				for (int nIndex = 0; nIndex < ds.Tables[1].Rows.Count && r == null; nIndex++)
				{
					if (Convert.ToInt32(row["IdBloque"]) == Convert.ToInt32(ds.Tables[1].Rows[nIndex]["IdBloque"]))
						r = ds.Tables[1].Rows[nIndex];
				}

				if (r != null)
				{
					//PorcentajeAvance,0 AS PesoEspecifico
					row["MontoPresupuestado"] = r["MontoPresupuestado"];
					row["PorcentajeAvance"] = Math.Round(100 * Convert.ToDecimal(row["MontoEjecutado"]) / Convert.ToDecimal(r["MontoPresupuestado"]), 2);
					row["PesoEspecifico"] = Math.Round(100 * Convert.ToDecimal(r["MontoPresupuestado"]) / dSumMontoPresupuestado, 2);
				}
			}

			return ds;
		}

		public static DataSet AvancePorcentual(int IdProyecto, int IdBloque, int IdObra, int IdGrupo)
		{
			StringBuilder sb = new StringBuilder();
			
			sb.Append("SELECT  pr.Nombre Proyecto,b.Nombre Bloque,b.Encargado Supervisor, o.Id IdObra, o.Nombre Obra,sum((a.AvanceActual - a.AvanceInicial)*cd.Precio/100) MontoEjecutado, convert(decimal(15,2),0) AS MontoPresupuestado,convert(decimal(15,2),0) AS PorcentajeAvance,convert(decimal(15,2),0) AS PesoEspecifico  ");
			sb.Append("FROM uAvance a,uContratoDetalle cd,uProyecto pr,uBloque b,uObra o,uGrupo g,uListaGrupo lg,uItem i,uListaItem li,uSubItem si ");
			sb.Append("WHERE  a.IdSubItem = cd.IdSubItem and a.IdContrato = cd.IdContrato and o.Plantilla = 0 and a.IdProyecto = pr.Id and a.IdBloque = b.Id and ");
			sb.Append("a.IdObra = o.Id and a.IdGrupo = g.Id and g.IdListaGrupo = lg.Id and a.IdItem = i.Id and i.IdListaItem = li.Id and a.IdSubItem = si.Id ");
			sb.AppendFormat("and pr.Id ={0} ", IdProyecto);
			if ( IdBloque > 0 ) sb.AppendFormat("and b.Id ={0} ", IdBloque);
			if ( IdObra > 0 ) sb.AppendFormat("and o.Id ={0} ", IdObra);
			if ( IdGrupo > 0 ) sb.AppendFormat("and g.IdListaGrupo ={0} ", IdGrupo);
			sb.Append("GROUP by pr.Nombre,b.Nombre,b.Encargado,o.Id,o.Nombre ");
			sb.Append("ORDER by o.Nombre DESC;");
			
			sb.Append("select p.Nombre Proyecto, b.Nombre Bloque,o.Id IdObra , o.Nombre Obra,sum(i.MontoGlobalBs * si.Cantidad) MontoPresupuestado ");
			sb.Append("FROM  uProyecto p, uBloque b, uObra o, uGrupo g, uItem i, uSubItem si,uListaGrupo lg ");
			sb.Append("WHERE  i.IdProyecto = p.Id and i.IdBloque = b.Id and i.IdObra = o.Id and i.IdGrupo = g.Id and i.Id = si.IdItem and o.Plantilla = 0 and o.Id = i.IdObra and g.IdListaGrupo = lg.id ");
			sb.AppendFormat("and p.Id ={0} ", IdProyecto);
			if ( IdBloque > 0 ) sb.AppendFormat("and b.Id ={0} ", IdBloque);
			if ( IdObra > 0 ) sb.AppendFormat("and o.Id ={0} ", IdObra);
			if ( IdGrupo > 0 ) sb.AppendFormat("and g.IdListaGrupo ={0} ", IdGrupo);
			sb.Append("GROUP by p.Nombre, b.Nombre,o.Id, o.Nombre ");
			sb.Append("ORDER by o.Nombre DESC;");

			sb.Append("select  pr.Nombre Proyecto,sum((a.AvanceActual - a.AvanceInicial)*cd.Precio/100) MontoEjecutado ");
			sb.Append("from uAvance a,uContratoDetalle cd,uProyecto pr,uBloque b,uObra o,uGrupo g,uListaGrupo lg,uItem i,uListaItem li,uSubItem si ");
			sb.Append("where a.IdSubItem = cd.IdSubItem and a.IdContrato = cd.IdContrato and o.Plantilla = 0 and  ");
			sb.Append("a.IdProyecto = pr.Id and a.IdBloque = b.Id and a.IdObra = o.Id and a.IdGrupo = g.Id and g.IdListaGrupo = lg.Id and ");
			sb.Append("a.IdItem = i.Id and i.IdListaItem = li.Id and a.IdSubItem = si.Id ");
			sb.AppendFormat("and pr.Id ={0} ", IdProyecto);
			if ( IdGrupo > 0 ) sb.AppendFormat("and g.IdListaGrupo ={0} ", IdGrupo);
			sb.Append("group by pr.Nombre; ");

			sb.Append("select p.Nombre Proyecto,sum(i.MontoGlobalBs * si.Cantidad) MontoPresupuestado ");
			sb.Append("FROM  uProyecto p, uBloque b, uObra o, uGrupo g, uItem i, uSubItem si,uListaGrupo lg ");
			sb.Append("WHERE  i.IdProyecto = p.Id and i.IdBloque = b.Id and i.IdObra = o.Id and i.IdGrupo = g.Id and i.Id = si.IdItem and o.Plantilla = 0 and o.Id = i.IdObra and g.IdListaGrupo = lg.id ");
			sb.AppendFormat("and p.Id ={0} ", IdProyecto);
			if (IdGrupo > 0) sb.AppendFormat("and g.IdListaGrupo ={0} ", IdGrupo);
			sb.Append("GROUP by p.Nombre ");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);
			
			DataSet ds = sph.ExecuteDataset();

			decimal dSumMontoPresupuestado = 0;
			foreach (DataRow row in ds.Tables[1].Rows)
				dSumMontoPresupuestado += Convert.ToDecimal(row["MontoPresupuestado"]);
			
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				DataRow r = null;
				for (int nIndex = 0; nIndex < ds.Tables[1].Rows.Count && r == null; nIndex++)
				{
					if ( Convert.ToInt32(row["IdObra"]) == Convert.ToInt32(ds.Tables[1].Rows[nIndex]["IdObra"]) )
						r = ds.Tables[1].Rows[nIndex];
				}
				
				if ( r != null )
				{
					//PorcentajeAvance,0 AS PesoEspecifico
					row["MontoPresupuestado"] = r["MontoPresupuestado"];
					row["PorcentajeAvance"] = Math.Round(100 * Convert.ToDecimal(row["MontoEjecutado"]) / Convert.ToDecimal(r["MontoPresupuestado"]), 2);
					row["PesoEspecifico"] = Math.Round(100 * Convert.ToDecimal(r["MontoPresupuestado"]) / dSumMontoPresupuestado,2);
				}
			}
			
			return ds;
		}

		public static DataSet DetalleAvance(int IdProyecto, int IdBloque, int IdObra, int IdGrupo, int IdContratista, int IdContrato,bool bFiltrarFecha, DateTime dtFInicio, DateTime dtFechaFin)
		{
			StringBuilder sb = new StringBuilder();
			
			sb.Append("SELECT  ");
			sb.Append("convert(nvarchar(5),a.IdProyecto) + ' - ' + pr.Nombre Proyecto ");
			sb.Append(",convert(nvarchar(5),co.Id) + ' - ' + co.Nombre Contratista ");
			sb.Append(",convert(nvarchar(5),a.IdBloque) + ' - ' + b.Nombre Bloque ");
			sb.Append(",convert(nvarchar(5),a.IdObra) + ' - ' + o.Nombre Obra ");
			sb.Append(",convert(nvarchar(5),a.IdGrupo) + ' - ' + lg.Nombre Grupo ");
			sb.Append(",convert(nvarchar(5),a.IdItem) + ' - ' + li.Nombre Item ");
			sb.Append(",convert(nvarchar(5),a.IdSubItem) + ' - ' + si.Nombre SubItem ");
			sb.Append(",convert(nvarchar(10),a.Fecha,103) Fecha ");
			sb.Append(",a.AvanceActual - a.AvanceInicial Avance ");
			sb.Append(",convert(nvarchar(10),convert(decimal(15,2),si.Cantidad*(a.AvanceActual - a.AvanceInicial)/100)) + li.Unidad Cantidad ");
			sb.Append(",(a.AvanceActual - a.AvanceInicial)*cd.Precio/100 SubTotal ");
			
			sb.Append("FROM uAvance a, uContrato c, uContratista co, uProyecto pr,uBloque b, uObra o, uGrupo g, uListaGrupo lg, uItem i, uListaItem li, uSubItem si, uContratoDetalle cd  ");
			sb.Append("WHERE a.IdContrato = c.Id and c.IdContratista = co.Id and pr.Id = a.IdProyecto and b.Id = a.IdBloque and o.Id = a.IdObra and g.Id = a.IdGrupo and lg.Id = g.IdListaGrupo and i.Id = a.IdItem and i.IdListaItem = li.Id and si.Id = a.IdSubItem and cd.IdProyecto = a.IdProyecto and cd.IdBloque = a.IdBloque and cd.IdObra = a.IdObra and cd.IdGrupo = a.IdGrupo and cd.IdItem = a.IdItem and cd.IdSubItem = a.IdSubItem and cd.IdContrato = c.Id and c.IdContratista = co.Id ");
			sb.AppendFormat("and pr.Id ={0} ", IdProyecto);
			if (bFiltrarFecha) sb.AppendFormat("and a.Fecha between '{0}' and '{1}' ", dtFInicio.ToString("yyyyMMdd"), dtFechaFin.ToString("yyyyMMdd"));
			if (IdBloque > 0) sb.AppendFormat("and b.Id ={0} ", IdBloque);
			if (IdObra > 0) sb.AppendFormat("and o.Id ={0} ", IdObra);
			if (IdGrupo > 0) sb.AppendFormat("and g.IdListaGrupo ={0} ", IdGrupo);
			if (IdContratista > 0) sb.AppendFormat("and co.Id ={0} ", IdContratista);
			if (IdContrato > 0) sb.AppendFormat("and c.Id ={0} ", IdContrato);

			sb.Append("ORDER by a.fecha,convert(nvarchar(5),a.IdProyecto) + ' - ' + pr.Nombre,");
			sb.Append("convert(nvarchar(5),a.IdBloque) + ' - ' + b.Nombre,");
			sb.Append("convert(nvarchar(5),a.IdObra) + ' - ' + o.Nombre,");
			sb.Append("convert(nvarchar(5),a.IdGrupo) + ' - ' + lg.Nombre,");
			sb.Append("convert(nvarchar(5),a.IdItem) + ' - ' + li.Nombre,");
			sb.Append("a.Id,");
			sb.Append("convert(nvarchar(5),a.IdSubItem) + ' - ' + si.Nombre");
			
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);			
			DataSet ds = sph.ExecuteDataset();
			return ds;
		}


		public static DataSet GetPaid(int paymentId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id ProgressId,");
			sb.Append("IdProyecto ProjectId,");
			sb.Append("IdBloque BlockId,");
			sb.Append("IdObra WorkId,");
			sb.Append("IdGrupo GroupId,");
			sb.Append("IdItem ItemId,");
			sb.Append("IdSubItem SubItemId,");
			sb.Append("IdContrato ContractId,");
			sb.Append("Fecha Date,");
			sb.Append("AvanceInicial InitialProgress,");
			sb.Append("AvanceActual CurrentProgress ");
			sb.AppendFormat("FROM {0}Avance ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE IdPago={0} ", paymentId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteDataset();
		}

		public static bool Add(int contractId,
			int projectId,
			int blockId,
			int workId,
			int groupId,
			int itemId,
			int subItemId,
			int initialProgress,
			int currentProgress,
			string user)
		{
			StringBuilder sb = new StringBuilder();
			
			sb.Append("INSERT INTO uAvance ");
			sb.Append("(IdContrato,IdProyecto,IdBloque,IdObra,IdGrupo,IdItem,IdSubItem,AvanceInicial,AvanceActual,Usuario,Fecha)");
			sb.Append(" VALUES ");
			sb.AppendFormat("({0},{1},{2},{3},{4},{5},{6},{7},{8},'{9}',GETDATE())", contractId, projectId, blockId, workId, groupId, itemId, subItemId, initialProgress, currentProgress, user);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);
			int rows = 0;
			
			try
			{
				rows = sph.ExecuteNonQuery();
			}
			catch(Exception)
			{
				rows = 0;
			}
			
			return rows > 0;
		}

		public static bool Add(Dictionary<string, object>[] batchProgress)
		{
			StringBuilder sb = null;
			string prefix = DBHelper.Instance.TablePrefix;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			bool isOk = false;

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			try
			{
				foreach (Dictionary<string, object> progress in batchProgress)
				{
					sb = new StringBuilder();
					sb.AppendFormat("INSERT INTO {0}Avance ", prefix);
					sb.Append("(IdContrato,IdProyecto,IdBloque,IdObra,IdGrupo,IdItem,IdSubItem,AvanceInicial,AvanceActual,Usuario,Fecha)");
					sb.Append(" VALUES ");
					sb.AppendFormat("({0},{1},{2},{3},{4},{5},{6},{7},{8},'{9}',GETDATE())",
						progress["ContractId"],
						progress["ProjectId"],
						progress["BlockId"],
						progress["WorkId"],
						progress["GroupId"],
						progress["ItemId"],
						progress["SubItemId"],
						progress["Initial"],
						progress["Current"],
						progress["User"]);

					command.CommandText = sb.ToString();
					command.ExecuteNonQuery();
				}

				transaction.Commit();
				isOk = true;
			}
			catch (Exception)
			{
				transaction.Rollback();
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return isOk;
		}
	}
}
