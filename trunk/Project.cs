using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public class Project
	{
		private int _id = -1;
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

		public Project(int id)
		{
			_id = id;
		}

		public static Project Create(int projectId)
		{
			Project project = new Project(projectId);

			using (IDataReader reader = DBProject.Get(projectId))
			{
				if (reader.Read())
				{
					project.Name = (string)reader["Name"];
				}
			}

			return project;
		}

		public static IDataReader GetAll()
		{
			return DBProject.GetAll();
		}
		
		public static DataSet GetAllDataSet()
		{
			return DBProject.GetAllDataSet();
		}
	}
}
