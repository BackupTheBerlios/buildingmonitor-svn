using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Threading;
using System.Text;
using System.Configuration;
using System.Web.UI;
using mojoPortal.Web;

namespace BuildingMonitor.UI.Helpers
{
	internal class JQuery
	{		
		private const string ARRAY_SEPARATOR = ",";
		private const string ARRAY_START = "[";
		private const string ARRAY_END = "]";
		private const string QUOTE = "'";

		private static CultureInfo Culture
		{
			get
			{
				return Thread.CurrentThread.CurrentUICulture;
			}
		}

		public static string GetDayNamesMin()
		{
			return GetDayNamesMin(Culture);
		}

		public static string GetDayNamesMin(CultureInfo culture)
		{
			string value = string.Empty;

			foreach (string day in culture.DateTimeFormat.AbbreviatedDayNames)
			{
				value += QUOTE + (day.Length > 2 ? day.Remove(2) : day) + QUOTE + ARRAY_SEPARATOR;
			}

			return ARRAY_START + value.Remove(value.Length - ARRAY_SEPARATOR.Length) + ARRAY_END;
		}

		public static string GetMonthNames()
		{
			return GetMonthNames(Culture);
		}
		
		
		public static string GetMonthNames(CultureInfo culture)
		{
			string value = string.Empty;

			foreach (string monthName in culture.DateTimeFormat.MonthNames)
			{
				value += QUOTE + monthName + QUOTE + ARRAY_SEPARATOR;
			}

			return ARRAY_START + value.Remove(value.Length - ARRAY_SEPARATOR.Length) + ARRAY_END;
		}

		public static string GetShortDateFormat()
		{
			string format = Culture.DateTimeFormat.ShortDatePattern;
			string[] separator = { Culture.DateTimeFormat.DateSeparator };
			string[] splitFormat = format.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			string value = string.Empty;

			foreach (string netFormat in splitFormat)
			{
				switch (netFormat)
				{
					case "yyyy":
						value += "yy";
						break;
					case "yy":
					case "y":
						value += "y";
						break;
					case "MMMM":
						value += "MM";
						break;
					case "MMM":
						value += "M";
						break;
					case "MM":
						value += "mm";
						break;
					case "M":
						value += "m";
						break;
					case "dddd":
						value += "DD";
						break;
					case "ddd":
						value += "D";
						break;
					case "dd":
						value += "dd";
						break;
					case "d":
						value += "d";
						break;
				}

				value += separator;
			}

			return value.Remove(value.Length - separator.Length);
		}

		public static void Register(Page page)
		{
			if (page.ClientScript.IsClientScriptBlockRegistered(typeof(Page), "jquery"))
				return;
				
			string jQueryVersion = "1.3.2";
			string url = string.Empty;
			
				
			if (ConfigurationManager.AppSettings["GoogleCDNjQueryVersion"] != null)
			{
				jQueryVersion = ConfigurationManager.AppSettings["GoogleCDNjQueryVersion"];
			}

			if (WebConfigSettings.UseGoogleCDN)
			{
				url = "http://ajax.googleapis.com/ajax/libs/jquery/" + jQueryVersion + "/jquery.min.js";
			}
			else if (ConfigurationManager.AppSettings["jQueryBasePath"] != null)
			{
				url = page.ResolveUrl(ConfigurationManager.AppSettings["jQueryBasePath"]) + "jquery.min.js";
			}

			page.ClientScript.RegisterClientScriptBlock(typeof(Page), "jquery", "<script type='text/javascript' src=" + url + "></script>");
		}
	}
}
