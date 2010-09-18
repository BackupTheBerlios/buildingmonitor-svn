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
	public class DBBlock
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}

		public static IDataReader Get(int projectId, int blockId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Nombre Name ");
			sb.AppendFormat("FROM {0}Bloque ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE IdProyecto={0} AND Id={1}", projectId, blockId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetAll(int projectId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id BlockId,");
			sb.Append("Nombre Name ");
			sb.AppendFormat("FROM {0}Bloque ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE IdProyecto={0} ", projectId);
			sb.Append("ORDER BY Nombre");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}
		
	}
}
