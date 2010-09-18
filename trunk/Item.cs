using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public class Item
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

		public static Item Create(int projectId, int blockId, int workId, int groupId, int itemId)
		{
			Item item = new Item();

			using (IDataReader reader = DBItem.Get(projectId, blockId, workId, groupId, itemId))
			{
				if (reader.Read())
				{
					item.Name = (string)reader["Name"];
				}
			}

			return item;
		}

		public static IDataReader GetAll(int projectId, int blockId, int workId, int groupId)
		{
			return DBItem.GetAll(projectId, blockId, workId, groupId);
		}
	}
}
