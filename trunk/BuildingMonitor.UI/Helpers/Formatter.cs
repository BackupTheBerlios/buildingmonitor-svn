using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildingMonitor.UI.Helpers
{
	public class Formatter
	{
		public static string Decimal(object value)
		{
			if (value == null)
				return "";
			
			decimal result = Convert.ToDecimal(value);
			
			return result.ToString("F02", System.Globalization.CultureInfo.InvariantCulture);
		}
		
		public static string ShortDate(object value)
		{
			if (value == null)
				return string.Empty;
				
			
			DateTime result = Convert.ToDateTime(value);
			
			
			return result.ToShortDateString();
		}
	}
}
