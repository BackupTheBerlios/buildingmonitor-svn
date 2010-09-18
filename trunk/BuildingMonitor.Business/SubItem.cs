using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public class SubItem
	{
		private string _name = string.Empty;
		private decimal _quantity = 0;

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

		public decimal Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				_quantity = value;
			}
		}

		public static SubItem Create(int projectid, int blockId, int workId, int groupId, int itemId, int subItemId)
		{
			SubItem subItem = new SubItem();

			using (IDataReader reader = DBSubItem.Get(projectid, blockId, workId, groupId, itemId, subItemId, string.Empty))
			{
				if (reader.Read())
				{
					subItem.Name = (string)reader["Name"];
					subItem.Quantity = (decimal)reader["Quantity"];
				}

				return subItem;
			}
		}

		public static IDataReader Get(int projectId, int blockId, int workId, int groupId, int itemId, int subItemId, string currencyCode)
		{
			return DBSubItem.Get(projectId, blockId, workId, groupId, itemId, subItemId, currencyCode);
		}

		public static IDataReader GetAll(int projectId, int blockId, int workId, int groupId, int itemId)
		{
			return DBSubItem.GetAll(projectId, blockId, workId, groupId, itemId);
		}

		public static IDataReader GetAllForContract(int projectId, int blockId, int workId, int groupId, int itemId, string currencyCode)
		{
			return DBSubItem.GetAllForContract(projectId, blockId, workId, groupId, itemId, currencyCode);
		}

		public static IDataReader GetAllPayable(int contractId, int projectId)
		{
			return DBSubItem.GetAllPayable(contractId, projectId);
		}

		public static IDataReader GetAllPayablePaidWork(int contractId)
		{
			return DBSubItem.GetAllPayablePaidWork(contractId);
		}
	}
}
