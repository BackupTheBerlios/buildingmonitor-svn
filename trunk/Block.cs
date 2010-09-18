using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public class Block
	{
		private string _name;

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

		public static Block Create(int projectId, int blockId)
		{
			Block block = new Block();

			using (IDataReader reader = DBBlock.Get(projectId, blockId))
			{
				if (reader.Read())
				{
					block.Name = (string)reader["Name"];
				}
			}

			return block;
		}

	
		public static IDataReader GetAll(int projectId)
		{
			return DBBlock.GetAll(projectId);
		}
	}
}
