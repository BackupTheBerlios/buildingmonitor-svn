using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;

namespace BuildingMonitor.Data
{
	public class DBSubItem
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}

		public static IDataReader GetAll(int projectId, int blockId, int workId, int groupId, int itemId)
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString,
				"SELECT Id AS SubItemId," +
				"Nombre AS Name, " +
				"Cantidad AS Quantity " +
				"FROM uSubItem " +
				"WHERE IdProyecto=" + projectId.ToString() + " AND " +
				"IdBloque=" + blockId.ToString() + " AND " +
				"IdObra=" + workId.ToString() + " AND " +
				"IdGrupo=" + groupId.ToString() + " AND " +
				"IdItem=" + itemId.ToString(),
				CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader Get(int projectId, int blockId, int workId, int groupId, int itemId, int subItemId)
		{
			StringBuilder sb = new StringBuilder();
			string prefix = DBHelper.Instance.TablePrefix;

			sb.Append("SELECT s.IdProyecto ProjectId, ");
			sb.Append("s.IdBloque BlockId, ");
			sb.Append("s.IdObra WorkId, ");
			sb.Append("s.IdGrupo GroupId, ");
			sb.Append("s.IdItem ItemId, ");
			sb.Append("s.Id SubItemId, ");
			sb.Append("s.Nombre Name,");
			sb.Append("s.Cantidad Quantity ");
			sb.AppendFormat("FROM {0}SubItem s ", prefix);
			sb.AppendFormat("WHERE s.IdProyecto={0} ", projectId);
			sb.AppendFormat("AND s.IdBloque={0} ", blockId);
			sb.AppendFormat("AND s.IdObra={0} ", workId);
			sb.AppendFormat("AND s.IdGrupo={0} ", groupId);
			sb.AppendFormat("AND s.IdItem={0} ", itemId);
			sb.AppendFormat("AND s.Id={0} ", subItemId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader Get(int projectId, int blockId, int workId, int groupId, int itemId, int subItemId, string currencyCode)
		{
			StringBuilder sb = new StringBuilder();
			string prefix = DBHelper.Instance.TablePrefix;

			sb.Append("SELECT s.IdProyecto ProjectId, ");
			sb.Append("s.IdBloque BlockId, ");
			sb.Append("s.IdObra WorkId, ");
			sb.Append("s.IdGrupo GroupId, ");
			sb.Append("s.IdItem ItemId, ");
			sb.Append("s.Id SubItemId, ");
			sb.Append("s.Nombre Name, ");
			sb.Append("l.Unidad Unit, ");
			sb.Append("s.Cantidad Quantity, ");

			if (currencyCode.Contains("$us"))
			{
				sb.Append("l.PrecioSus UnitPrice, ");
				sb.Append("l.PrecioSus*s.Cantidad TotalPrice ");
			}
			else
			{
				sb.Append("l.PrecioBs UnitPrice, ");
				sb.Append("l.PrecioBs*s.Cantidad TotalPrice ");
			}

			sb.AppendFormat("FROM {0}SubItem s INNER JOIN ", prefix);
			sb.AppendFormat("({0}Item i INNER JOIN {0}ListaItem l ON i.IdListaItem=l.Id) ", prefix);
			sb.AppendFormat("ON (s.IdProyecto=i.IdProyecto AND s.IdBloque=i.IdBloque AND s.IdObra=i.IdObra AND s.IdGrupo=i.IdGrupo AND s.IdItem=i.Id) ");
			sb.AppendFormat("WHERE s.IdProyecto={0} ", projectId);
			sb.AppendFormat("AND s.IdBloque={0} ", blockId);
			sb.AppendFormat("AND s.IdObra={0} ", workId);
			sb.AppendFormat("AND s.IdGrupo={0} ", groupId);
			sb.AppendFormat("AND s.IdItem={0} ", itemId);
			sb.AppendFormat("AND s.Id={0} ", subItemId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetAllForContract(int projectId, int blockId, int workId, int groupId, int itemId, string currencyCode)
		{
			SqlParameter[] parameters = new SqlParameter[6];

			parameters[0] = new SqlParameter("@IdProyecto", projectId);
			parameters[1] = new SqlParameter("@IdBloque", blockId);
			parameters[2] = new SqlParameter("@IdObra", workId);
			parameters[3] = new SqlParameter("@IdGrupo", groupId);
			parameters[4] = new SqlParameter("@IdItem", itemId);
			parameters[5] = new SqlParameter("@Moneda", currencyCode);

			return SqlHelper.ExecuteReader(DBHelper.Instance.ConnectionString, CommandType.StoredProcedure, "bm_SubItemsForNewContract", parameters);
		}

		public static IDataReader GetAllPayable(int contractId, int projectId)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT a.IdProyecto ProjectId,");
			sb.Append("a.IdBloque BlockId,");
			sb.Append("a.IdObra WorkId,");
			sb.Append("a.IdGrupo GroupId,");
			sb.Append("a.IdItem ItemId,");
			sb.Append("a.IdSubItem SubItemId,");
			sb.Append("a.Id ProgressId,");
			sb.Append("a.Fecha Date,");
			sb.Append("a.AvanceInicial InitialProgress,");
			sb.Append("a.AvanceActual CurrentProgress,");
			sb.Append("s.Nombre Name,");
			sb.Append("d.Precio PriceTotal,");
			sb.Append("CAST((a.AvanceActual-a.AvanceInicial)*d.Precio/100 as decimal(16,2)) Amount ");
			sb.AppendFormat("FROM {0}Avance a LEFT JOIN {0}ContratoDetalle d ", prefix);
			sb.Append("ON a.IdContrato=d.IdContrato ");
			sb.Append("AND a.IdProyecto=d.IdProyecto ");
			sb.Append("AND a.IdBloque=d.IdBloque ");
			sb.Append("AND a.IdObra=d.IdObra ");
			sb.Append("AND a.IdGrupo=d.IdGrupo ");
			sb.Append("AND a.IdItem=d.IdItem ");
			sb.Append("AND a.IdSubItem=d.IdSubItem ");
			sb.AppendFormat("INNER JOIN {0}SubItem s ", prefix);
			sb.Append("ON d.IdProyecto=s.IdProyecto ");
			sb.Append("AND d.IdBloque=s.IdBloque ");
			sb.Append("AND d.IdObra=s.IdObra ");
			sb.Append("AND d.IdGrupo=s.IdGrupo ");
			sb.Append("AND d.IdItem=s.IdItem ");
			sb.Append("AND d.IdSubItem=s.Id ");
			sb.Append("WHERE a.IdPago IS NULL ");

			if (contractId > 0)
			{
				sb.AppendFormat("AND a.IdContrato={0} ", contractId);

				if (projectId > 0)
					sb.AppendFormat("AND a.IdProyecto={0} ", projectId);
			}

			sb.Append("ORDER BY ProjectId,BlockId,WorkId,GroupId,ItemId,SubItemId");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static DataSet GetAllPayable(int contractId)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT a.IdProyecto ProjectId,");
			sb.Append("a.IdBloque BlockId,");
			sb.Append("a.IdObra WorkId,");
			sb.Append("a.IdGrupo GroupId,");
			sb.Append("a.IdItem ItemId,");
			sb.Append("a.IdSubItem SubItemId,");
			sb.Append("a.Id ProgressId,");
			sb.Append("a.Fecha Date,");
			sb.Append("a.AvanceInicial InitialProgress,");
			sb.Append("a.AvanceActual CurrentProgress,");
			sb.Append("s.Nombre Name,");
			sb.Append("d.Precio PriceTotal,");
			sb.Append("CAST((a.AvanceActual-a.AvanceInicial)*d.Precio/100 as decimal(16,2)) Amount ");
			sb.AppendFormat("FROM {0}Avance a LEFT JOIN {0}ContratoDetalle d ", prefix);
			sb.Append("ON a.IdContrato=d.IdContrato ");
			sb.Append("AND a.IdProyecto=d.IdProyecto ");
			sb.Append("AND a.IdBloque=d.IdBloque ");
			sb.Append("AND a.IdObra=d.IdObra ");
			sb.Append("AND a.IdGrupo=d.IdGrupo ");
			sb.Append("AND a.IdItem=d.IdItem ");
			sb.Append("AND a.IdSubItem=d.IdSubItem ");
			sb.AppendFormat("INNER JOIN {0}SubItem s ", prefix);
			sb.Append("ON d.IdProyecto=s.IdProyecto ");
			sb.Append("AND d.IdBloque=s.IdBloque ");
			sb.Append("AND d.IdObra=s.IdObra ");
			sb.Append("AND d.IdGrupo=s.IdGrupo ");
			sb.Append("AND d.IdItem=s.IdItem ");
			sb.Append("AND d.IdSubItem=s.Id ");
			sb.Append("WHERE a.IdPago IS NULL ");

			if (contractId > 0)
				sb.AppendFormat("AND a.IdContrato={0} ", contractId);

			sb.Append("ORDER BY ProjectId,BlockId,WorkId,GroupId,ItemId,SubItemId");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteDataset();
		}

		public static IDataReader GetAllPayablePaidWork(int contractId)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			string viewPrefix = DBHelper.Instance.ViewPrefix;
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT a.IdProyecto ProjectId,");
			sb.Append("a.IdBloque BlockId,");
			sb.Append("a.IdObra WorkId,");
			sb.Append("a.IdGrupo GroupId,");
			sb.Append("a.IdItem ItemId,");
			sb.Append("a.IdSubItem SubItemId,");
			sb.Append("s.Nombre Name,");
			sb.Append("a.AvanceMayor Progress,");
			sb.Append("Precio SubItemPrice,");
			sb.Append("c.Monto AmountTotal,");
			sb.Append("(AvanceMayor*Precio)/c.Monto AbsoluteProgress ");
			sb.AppendFormat("FROM {0}ContratoDetalle d INNER JOIN {1}AvanceMayor a ON ", prefix, viewPrefix);
			sb.Append("d.IdProyecto=a.IdProyecto AND d.IdBloque=a.IdBloque AND d.IdObra=a.IdObra AND d.IdGrupo=a.IdGrupo AND d.IdItem=a.IdItem AND d.IdSubItem=a.IdSubItem ");
			sb.AppendFormat("INNER JOIN {0}Contrato c ON c.Id=d.IdContrato ", prefix);
			sb.AppendFormat("INNER JOIN {0}SubItem s ON ", prefix);
			sb.Append("d.IdProyecto=s.IdProyecto AND d.IdBloque=s.IdBloque AND d.IdObra=s.IdObra AND d.IdGrupo=s.IdGrupo AND d.IdItem=s.IdItem AND d.IdSubItem=s.Id ");
			sb.AppendFormat("WHERE c.Id={0}", contractId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}
	}
}
