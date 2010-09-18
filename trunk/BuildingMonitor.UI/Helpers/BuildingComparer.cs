using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildingMonitor.UI.Helpers
{
	public struct BuildingComparer
	{
		private int _pId;
		private int _bId;
		private int _wId;
		private int _gId;
		private int _iId;
		private int _sId;

		public BuildingComparer(int pId, int bId, int wId, int gId, int iId, int sId)
		{
			_pId = pId;
			_bId = bId;
			_wId = wId;
			_gId = gId;
			_iId = iId;
			_sId = sId;
		}

		public BuildingComparer(string[] ids)
		{
			_pId = Convert.ToInt32(ids[0]);
			_bId = Convert.ToInt32(ids[1]);
			_wId = Convert.ToInt32(ids[2]);
			_gId = Convert.ToInt32(ids[3]);
			_iId = Convert.ToInt32(ids[4]);
			_sId = Convert.ToInt32(ids[5]);
		}

		public int ProjectId
		{
			get
			{
				return _pId;
			}
		}
		public int BlockId
		{
			get
			{
				return _bId;
			}
		}
		public int WorkId
		{
			get
			{
				return _wId;
			}
		}
		public int GroupId
		{
			get
			{
				return _gId;
			}
		}
		public int ItemId
		{
			get
			{
				return _iId;
			}
		}
		public int SubItemId
		{
			get
			{
				return _sId;
			}
		}

		public static BuildingComparer Empty
		{
			get
			{
				return new BuildingComparer(-1, -1, -1, -1, -1, -1);
			}
		}
		public static bool IsEmpty(BuildingComparer o)
		{
			return o.ProjectId < 0 && o.BlockId < 0 && o.WorkId < 0 && o.GroupId < 0 && o.ItemId < 0 && o.SubItemId < 0;
		}

		public void Set(int pId, int bId, int wId, int gId, int iId, int sId)
		{
			_pId = pId;
			_bId = bId;
			_wId = wId;
			_gId = gId;
			_iId = iId;
			_sId = sId;
		}
		public void Set(string[] ids)
		{
			_pId = Convert.ToInt32(ids[0]);
			_bId = Convert.ToInt32(ids[1]);
			_wId = Convert.ToInt32(ids[2]);
			_gId = Convert.ToInt32(ids[3]);
			_iId = Convert.ToInt32(ids[4]);
			_sId = Convert.ToInt32(ids[5]);
		}

		public void CopyFrom(BuildingComparer o)
		{
			_pId = o.ProjectId;
			_bId = o.BlockId;
			_wId = o.WorkId;
			_gId = o.GroupId;
			_iId = o.ItemId;
			_sId = o.SubItemId;
		}
		public bool IsProject(BuildingComparer o)
		{
			return _pId == o.ProjectId;
		}
		public bool IsBlock(BuildingComparer o)
		{
			return _bId == o.BlockId && IsProject(o);
		}
		public bool IsWork(BuildingComparer o)
		{
			return _wId == o.WorkId && IsBlock(o);
		}
		public bool IsGroup(BuildingComparer o)
		{
			return _gId == o.GroupId && IsWork(o);
		}
		public bool IsItem(BuildingComparer o)
		{
			return _iId == o.ItemId && IsGroup(o);
		}
	}
}
