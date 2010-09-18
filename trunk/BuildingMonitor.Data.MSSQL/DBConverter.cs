using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingMonitor.Data
{
	internal class DBConverter
	{
		public static string ToSafeDate(DateTime date)
		{
			return date.ToString("yyyyMMdd");
		}
		
		public static string ToSafeDecimal(decimal arg)
		{
			return arg.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}
		public static string ToSafeDecimal(double arg)
		{
			return arg.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
