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
	public class DBItem
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}

		public static IDataReader Get(int projectId, int blockId, int workId, int groupId, int itemId)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT i.Id ItemId,");
			sb.Append("l.Nombre Name ");
			sb.AppendFormat("FROM {0}Item i INNER JOIN {0}ListaItem l ON i.IdListaItem=l.Id ", prefix);
			sb.AppendFormat("WHERE i.IdProyecto={0} ", projectId);
			sb.AppendFormat("AND i.IdBloque={0} ", blockId);
			sb.AppendFormat("AND i.IdObra={0} ", workId);
			sb.AppendFormat("AND i.IdGrupo={0} ", groupId);
			sb.AppendFormat("AND i.Id={0}", itemId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetAll(int projectId, int blockId, int workId, int groupId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT i.Id AS ItemId,");
			sb.Append("l.Nombre AS Name ");
			sb.AppendFormat("FROM {0}Item i INNER JOIN {0}ListaItem l ON i.IdListaItem=l.Id ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE i.IdProyecto={0} AND ", projectId);
			sb.AppendFormat("i.IdBloque={0} AND ", blockId);
			sb.AppendFormat("i.IdObra={0} AND ", workId);
			sb.AppendFormat("i.IdGrupo={0} ", groupId);
			sb.Append("ORDER BY l.Nombre");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}
	}
}
