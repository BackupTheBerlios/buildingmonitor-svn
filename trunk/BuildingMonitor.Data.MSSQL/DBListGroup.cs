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
	public class DBListGroup
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}

		//public static IDataReader Get(int projectId, int blockId, int workId, int groupId)
		//{
		//    string prefix = DBHelper.Instance.TablePrefix;
		//    StringBuilder sb = new StringBuilder();

		//    sb.Append("SELECT g.Id GroupId,");
		//    sb.Append("l.Nombre Name ");
		//    sb.AppendFormat("FROM {0}Grupo g INNER JOIN {0}ListaGrupo l ON g.IdListaGrupo=l.Id ", prefix);
		//    sb.AppendFormat("WHERE g.IdProyecto={0} ", projectId);
		//    sb.AppendFormat("AND g.IdBloque={0} ", blockId);
		//    sb.AppendFormat("AND g.IdObra={0} ", workId);
		//    sb.AppendFormat("AND g.Id={0}", groupId);

		//    SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

		//    return sph.ExecuteReader();
		//}

		public static IDataReader GetAll()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT * ");
			sb.AppendFormat("FROM uListaGrupo ");			
			sb.Append("ORDER BY Nombre");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}
	}
}
