using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;

namespace BuildingMonitor.Data
{
	public class DBProject
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}
		
		public static IDataReader Get(int projectId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id AS ProjectId,");
			sb.Append("Nombre AS Name,");
			sb.Append("FechaCreacion AS DateCreated,");
			sb.Append("FechaModificacion AS DateModified,");
			sb.Append("Usuario AS CreatedBy,");
			sb.Append("UsuarioModificador AS ModifiedBy ");
			sb.AppendFormat("FROM {0}Proyecto ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE Id={0}", projectId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetAll()
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString,
				"SELECT Id AS ProjectId," +
				"Nombre AS Name," +
				"FechaCreacion AS DateCreated," +
				"FechaModificacion AS DateModified," +
				"Usuario AS CreatedBy," +
				"UsuarioModificador AS ModifiedBy " +
				"FROM uProyecto " +
				"ORDER BY Nombre",
				CommandType.Text, 0);

			return sph.ExecuteReader();
		}
		
		public static DataSet GetAllDataSet()
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString,
				"SELECT Nombre AS Name," +
				"FechaCreacion AS DateCreated," +
				"FechaModificacion AS DateModified," +
				"Usuario AS CreatedBy," +
				"UsuarioModificador AS ModifiedBy " +
				"FROM uProyecto",
				CommandType.Text, 0);

			return sph.ExecuteDataset();
		}
	}
}
