using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public sealed class Settings
	{
		private object[] _data = null;

		private void SetupData()
		{
			if (_data != null)
				return;

			_data = new object[4];

			using (IDataReader reader = DBConfiguration.Get())
			{
				if (reader.Read())
				{
					_data[0] = reader["ExchangeRateSell"];
					_data[1] = reader["ExchangeRateBuy"];
					_data[2] = reader["CompanyName"];
					_data[3] = reader["CompanyAddress"];
				}
			}
		}

		Settings()
		{
		}

		public decimal ExchangeRateSell
		{
			get
			{
				SetupData();

				return Convert.ToDecimal(_data[0]);
			}
		}

		public decimal ExchangeRateBuy
		{
			get
			{
				SetupData();

				return Convert.ToDecimal(_data[1]);
			}
		}

		public string CompanyName
		{
			get
			{
				SetupData();

				return _data[2].ToString();
			}
		}

		public string CompanyAddress
		{
			get
			{
				SetupData();

				return _data[3].ToString();
			}
		}

		public static Settings Instance
		{
			get
			{
				return Nested.instance;
			}
		}

		class Nested
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested()
			{
			}

			internal static readonly Settings instance = new Settings();
		}
	}
}
