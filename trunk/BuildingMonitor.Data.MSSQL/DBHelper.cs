using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace BuildingMonitor.Data
{
	internal sealed class DBHelper
	{
		private string _connection = null;
		
		DBHelper()
		{
		}

		public static DBHelper Instance
		{
			get
			{
				return Nested.instance;
			}
		}
		
		public string ConnectionString
		{
			get
			{
				if (_connection == null)
				{
					if (ConfigurationManager.AppSettings["BuildingMonitorMSSQLConnectionString"] != null)
						_connection = ConfigurationManager.AppSettings["BuildingMonitorMSSQLConnectionString"];
					else
						_connection = ConfigurationManager.AppSettings["MSSQLConnectionString"];
				}
				
				return _connection;
			}
		}

		public string TablePrefix
		{
			get
			{
				return "u";
			}
		}

		public string ViewPrefix
		{
			get
			{
				return "uv";
			}
		}

		class Nested
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested()
			{
			}

			internal static readonly DBHelper instance = new DBHelper();
		}
	}

}
