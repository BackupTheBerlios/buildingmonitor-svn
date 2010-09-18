using System;
using System.Data;
using BuildingMonitor.Data;


namespace BuildingMonitor.Business
{
	public class Currency
	{
		public static IDataReader GetAllCodes()
		{
			return DBCurrency.GetAllCodes();
		}
	}
}
