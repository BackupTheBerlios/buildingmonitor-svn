using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public class ListGroup
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

		//public static Group Create(int projectId, int blockId, int workId, int groupId)
		//{
		//    Group g = new Group();

		//    using (IDataReader reader = DBGroup.Get(projectId, blockId, workId, groupId))
		//    {
		//        if (reader.Read())
		//        {
		//            g.Name = (string)reader["Name"];
		//        }
		//    }

		//    return g;
		//}

		public static IDataReader GetAll()
		{
			return DBListGroup.GetAll();
		}
	}
}
