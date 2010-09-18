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
	public class DBConfiguration
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}

		public static IDataReader Get()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT TOP 1 ");
			sb.Append("TipoCambioVenta ExchangeRateSell,");
			sb.Append("TipoCambioCompra ExchangeRateBuy,");
			sb.Append("NombreEmpresa CompanyName,");
			sb.Append("DireccionEmpresa CompanyAddress ");
			sb.AppendFormat("FROM {0}Configuracion ", DBHelper.Instance.TablePrefix);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}
	}
}
