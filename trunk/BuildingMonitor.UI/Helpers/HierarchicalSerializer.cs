using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Web;
using System.Data;
using BuildingMonitor.Business;

namespace BuildingMonitor.UI.Helpers
{
    public class HierarchicalSerializer
    {
        private ListDictionary _data = new ListDictionary();
		private string _subItemUnit = "Unit";
		private string _subItemQuantity = "Quantity";
		private string _subItemPrice = "UnitPrice";
		private string _subItemTotal = "TotalPrice";
		
        #region Private Methods
		
		private void AddEntryProject(IDataReader reader)
		{
			Dictionary<string, object> project = null;
			List<Dictionary<string, object>> projects = FixType(_data["projects"]);

			_data["projects"] = projects;

			foreach (Dictionary<string, object> p in projects)
			{
				if ((int)reader["ProjectId"] != (int)p["pId"])
					continue;

				project = p;
			}

			if (project == null)
			{
				project = CreateProject(reader);
				projects.Add(project);
			}

			AddEntryBlock(reader, project);
		}

        private Dictionary<string, object> CreateProject(IDataReader reader)
        {
            int projectId = Convert.ToInt32(reader["ProjectId"]);
            Dictionary<string, object> project = new Dictionary<string, object>();

            project.Add("pId", projectId);
            project.Add("name", Project.Create(projectId).Name);
            project.Add("blocks", new List<Dictionary<string, object>>());

            return project;
        }

        private void AddEntryBlock(IDataReader reader, Dictionary<string, object> project)
        {
            Dictionary<string, object> block = null;
			List<Dictionary<string, object>> blocks = FixType(project["blocks"]);

			project["blocks"] = blocks;

            foreach (Dictionary<string, object> b in blocks)
            {
                if ((int)reader["BlockId"] != (int)b["bId"])
                    continue;

                block = b;
            }

            if (block == null)
            {
                block = CreateBlock(reader);
                blocks.Add(block);
            }

            AddEntryWork(reader, block);
        }

        private Dictionary<string, object> CreateBlock(IDataReader reader)
        {
            int projectId = Convert.ToInt32(reader["ProjectId"]);
            int blockId = Convert.ToInt32(reader["BlockId"]);
            Dictionary<string, object> block = new Dictionary<string, object>();

            block.Add("bId", blockId);
            block.Add("name", Block.Create(projectId, blockId).Name);
            block.Add("works", new List<Dictionary<string, object>>());

            return block;
        }

        private void AddEntryWork(IDataReader reader, Dictionary<string, object> block)
        {
            Dictionary<string, object> work = null;
			List<Dictionary<string, object>> works = FixType(block["works"]);

			block["works"] = works;

            foreach (Dictionary<string, object> w in works)
            {
                if ((int)reader["WorkId"] != (int)w["wId"])
                    continue;

                work = w;
            }

            if (work == null)
            {
                work = CreateWork(reader);
                works.Add(work);
            }

            AddEntryGroup(reader, work);
        }

        private Dictionary<string, object> CreateWork(IDataReader reader)
        {
            int projectId = Convert.ToInt32(reader["ProjectId"]);
            int blockId = Convert.ToInt32(reader["BlockId"]);
            int workId = Convert.ToInt32(reader["WorkId"]);
            Dictionary<string, object> work = new Dictionary<string, object>();

            work.Add("wId", workId);
            work.Add("name", Work.Create(projectId, blockId, workId).Name);
            work.Add("groups", new List<Dictionary<string, object>>());

            return work;
        }

        private void AddEntryGroup(IDataReader reader, Dictionary<string, object> work)
        {
            Dictionary<string, object> group = null;
			List<Dictionary<string, object>> groups = FixType(work["groups"]);

			work["groups"] = groups;

            foreach (Dictionary<string, object> g in groups)
            {
                if ((int)reader["GroupId"] != (int)g["gId"])
                    continue;

                group = g;
            }

            if (group == null)
            {
                group = CreateGroup(reader);
                groups.Add(group);
            }

            AddEntryItem(reader, group);
        }

        private Dictionary<string, object> CreateGroup(IDataReader reader)
        {
            int projectId = Convert.ToInt32(reader["ProjectId"]);
            int blockId = Convert.ToInt32(reader["BlockId"]);
            int workId = Convert.ToInt32(reader["WorkId"]);
            int groupId = Convert.ToInt32(reader["GroupId"]);
            Dictionary<string, object> group = new Dictionary<string, object>();

            group.Add("gId", groupId);
            group.Add("name", Group.Create(projectId, blockId, workId, groupId).Name);
            group.Add("items", new List<Dictionary<string, object>>());

            return group;
        }

        private void AddEntryItem(IDataReader reader, Dictionary<string, object> group)
        {
            Dictionary<string, object> item = null;
			List<Dictionary<string, object>> items = FixType(group["items"]);

			group["items"] = items;

            foreach (Dictionary<string, object> i in items)
            {
                if ((int)reader["ItemId"] != (int)i["iId"])
                    continue;

                item = i;
            }

            if (item == null)
            {
                item = CreateItem(reader);
                items.Add(item);
            }

            AddEntrySubItem(reader, item);
        }


        private Dictionary<string, object> CreateItem(IDataReader reader)
        {
            int projectId = Convert.ToInt32(reader["ProjectId"]);
            int blockId = Convert.ToInt32(reader["BlockId"]);
            int workId = Convert.ToInt32(reader["WorkId"]);
            int groupId = Convert.ToInt32(reader["GroupId"]);
            int itemId = Convert.ToInt32(reader["ItemId"]);
            Dictionary<string, object> item = new Dictionary<string, object>();

            item.Add("iId", itemId);
            item.Add("name", Item.Create(projectId, blockId, workId, groupId, itemId).Name);
            item.Add("subitems", new List<Dictionary<string, object>>());

            return item;
        }


        private void AddEntrySubItem(IDataReader reader, Dictionary<string, object> item)
        {
            Dictionary<string, object> subitem = null;
			List<Dictionary<string, object>> subitems = FixType(item["subitems"]);

			item["subitems"] = subitems;

            foreach (Dictionary<string, object> s in subitems)
            {
                if ((int)reader["SubItemId"] != (int)s["sId"])
                    continue;

                subitem = s;
            }

            if (subitem == null)
            {
                subitem = CreateSubItem(reader);
                subitems.Add(subitem);
            }
        }

        private Dictionary<string, object> CreateSubItem(IDataReader reader)
        {
            int subItemId = Convert.ToInt32(reader["SubItemId"]);
            Dictionary<string, object> subitem = new Dictionary<string, object>();

            subitem.Add("sId", subItemId);
            subitem.Add("name", reader["Name"]);
			subitem.Add("unit", reader[_subItemUnit]);
			subitem.Add("qty", reader[_subItemQuantity]);
			subitem.Add("price", reader[_subItemPrice]);
			subitem.Add("total", reader[_subItemTotal]);
			subitem.Add("progress", reader["CurrentProgress"]);

            return subitem;
        }

		private List<Dictionary<string, object>> FixType(object obj)
		{
			if (obj is List<Dictionary<string, object>>)
				return (List<Dictionary<string, object>>)obj;

			List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

			if (obj is object[])
			{
				foreach (object o in (object[])obj)
				{
					data.Add((Dictionary<string, object>)o);
				}
			}

			return data;
		}

		private void RemoveProject(BuildingComparer bc)
		{
			if (!_data.Contains("projects"))
				return;

			List<Dictionary<string, object>> projects = FixType(_data["projects"]);

			_data["projects"] = projects;
			
			for (int i = 0; i < projects.Count; i++)
			{
				Dictionary<string, object> project = (Dictionary<string, object>)projects[i];

				if ((int)project["pId"] != bc.ProjectId)
					continue;

				RemoveBlock(bc, project);

				if (((IList)project["blocks"]).Count == 0)
					projects.RemoveAt(i);
				break;
			}
		}

		private void RemoveBlock(BuildingComparer bc, Dictionary<string, object> project)
		{
			List<Dictionary<string, object>> blocks = FixType(project["blocks"]);

			project["blocks"] = blocks;

			for (int i = 0; i < blocks.Count; i++)
			{
				Dictionary<string, object> block = (Dictionary<string, object>)blocks[i];

				if ((int)block["bId"] != bc.BlockId)
					continue;
				
				RemoveWork(bc, block);

				if (((IList)block["works"]).Count == 0)
					blocks.RemoveAt(i);
				break;
			}
		}

		private void RemoveWork(BuildingComparer bc, Dictionary<string, object> block)
		{
			List<Dictionary<string, object>> works = FixType(block["works"]);

			block["works"] = works;

			for (int i = 0; i < works.Count; i++)
			{
				Dictionary<string, object> work = (Dictionary<string, object>)works[i];

				if ((int)work["wId"] != bc.WorkId)
					continue;

				RemoveGroup(bc, work);

				if (((IList)work["groups"]).Count == 0)
					works.RemoveAt(i);
				break;
			}
		}

		private void RemoveGroup(BuildingComparer bc, Dictionary<string, object> work)
		{
			List<Dictionary<string, object>> groups = FixType(work["groups"]);

			work["groups"] = groups;
			
			for (int i = 0; i < groups.Count; i++)
			{
				Dictionary<string, object> group = (Dictionary<string, object>)groups[i];

				if ((int)group["gId"] != bc.GroupId)
					continue;

				RemoveItem(bc, group);

				if (((IList)group["items"]).Count == 0)
					groups.RemoveAt(i);
				break;
			}
		}

		private void RemoveItem(BuildingComparer bc, Dictionary<string, object> group)
		{
			List<Dictionary<string, object>> items = FixType(group["items"]);

			group["items"] = items;
			
			for (int i = 0; i < items.Count; i++)
			{
				Dictionary<string, object> item = (Dictionary<string, object>)items[i];

				if ((int)item["iId"] != bc.ItemId)
					continue;

				RemoveSubitem(bc, item);
				
				if (((IList)item["subitems"]).Count == 0)
					items.RemoveAt(i);
				break;
			}
		}

		private void RemoveSubitem(BuildingComparer bc, Dictionary<string, object> item)
		{
			List<Dictionary<string, object>> subitems = FixType(item["subitems"]);

			item["subitems"] = subitems;

			for (int i=0; i<subitems.Count; i++)
			{
				Dictionary<string, object> subitem = (Dictionary<string, object>)subitems[i];

				if ((int)subitem["sId"] != bc.SubItemId)
					continue;
				
				subitems.RemoveAt(i);
				break;
			}
		}

        #endregion

		public void SetSubItemIndexers(string unit, string quantity, string price)	
		{
			_subItemUnit = unit;
			_subItemQuantity = quantity;
			_subItemPrice = price;
		}

        public void Read(string input)
        {
			JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

			if (string.IsNullOrEmpty(input))
				_data.Clear();
			else
				_data = jsSerializer.Deserialize<ListDictionary>(input);
        }

        public void AddEntry(IDataReader reader)
        {
			if (!_data.Contains("projects"))
				_data.Add("projects", new List<Dictionary<string, object>>());
			
			AddEntryProject(reader);
        }

		public void RemoveEntry(BuildingComparer bc)
		{
			RemoveProject(bc);
		}

		public override string ToString()
		{
			JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

			return jsSerializer.Serialize(_data);
		}
    }
}
