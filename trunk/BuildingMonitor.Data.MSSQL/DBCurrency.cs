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
	public class DBCurrency
	{
		public static IDataReader GetAllCodes()
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, "SELECT TOP 2 Codigo AS Code FROM uMoneda", CommandType.Text, 0);

			return sph.ExecuteReader();
		}
	}
}
