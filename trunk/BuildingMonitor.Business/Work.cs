using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public class Work
	{
		private string _name = string.Empty;

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public static Work Create(int projectId, int blockId, int workId)
		{
			Work w = new Work();

			using (IDataReader reader = DBWork.Get(projectId, blockId, workId))
			{
				if (reader.Read())
				{
					w.Name = (string)reader["Name"];
				}
			}

			return w;
		}
		
		public static IDataReader GetAll(int projectId, int blockId)
		{
			return DBWork.GetAll(projectId, blockId);
		}
	}
}
