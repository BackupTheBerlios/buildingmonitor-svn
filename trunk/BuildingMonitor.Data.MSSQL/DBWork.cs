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
	public class DBWork
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}

		public static IDataReader Get(int projectId, int blockId, int workId)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id AS WorkId,");
			sb.Append("Nombre Name ");
			sb.AppendFormat("FROM {0}Obra ", prefix);
			sb.AppendFormat("WHERE IdProyecto={0} AND IdBloque={1} AND Id={2}", projectId, blockId, workId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetAll(int projectId, int blockId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id AS WorkId,");
			sb.Append("Nombre AS Name ");
			sb.AppendFormat("FROM {0}Obra ", DBHelper.Instance.TablePrefix); 
			sb.AppendFormat("WHERE IdProyecto={0} AND ", projectId);
			sb.AppendFormat("IdBloque={0} ", blockId);
			sb.Append("ORDER BY Nombre");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}
	}
}
