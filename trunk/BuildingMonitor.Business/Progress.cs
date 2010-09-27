using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
    public class Progress
    {
		private ContractDetail _detail = new ContractDetail();
		private int _initial = -1;
		private int _current = -1;
		private DateTime _date;

		#region Public Properties
		
		public ContractDetail ContractDetail
		{
			get
			{
				return _detail;
			}
		}
		
		public int Initial
		{
			get
			{
				return _initial;
			}
		}
		
		public int Current
		{
			get
			{
				return _current;
			}
		}

		#endregion

		public void Set(int initial, int current)
		{
			_initial = initial;
			_current = current;
		}

		public static IDataReader GetLast(int contractId,
			int projectId, 
			int blockId, 
			int workId, 
			int groupId, 
			int itemId,
			int subItemId)
		{
			return DBProgress.GetLast(contractId, projectId, blockId, workId, groupId, itemId, subItemId);	
		}


		public static Progress Save(int contractId,
			int projectId, 
			int blockId, 
			int workId, 
			int groupId, 
			int itemId,
			int subItemId,
			int initialProgress,
			int currentProgress,
			string user)
		{
			Progress progress = new Progress();

			using (IDataReader reader = DBProgress.Add(contractId, projectId, blockId, workId, groupId, itemId, subItemId, initialProgress, currentProgress, user))
			{
				if (reader.Read())
				{
					progress.Set(Convert.ToInt32(reader["InitialProgress"]), Convert.ToInt32(reader["CurrentProgress"]));
				}
			}
			 
			return progress;
		}

		public static bool Save(List<Progress> batchProgress, string user)
		{
			Dictionary<string, object>[] batch = new Dictionary<string,object>[batchProgress.Count];
			int i = 0;

			foreach (Progress progress in batchProgress)
			{
				Dictionary<string, object> p = new Dictionary<string, object>();
				
				p.Add("ContractId", progress.ContractDetail.ContractId);
				p.Add("ProjectId", progress.ContractDetail.ProjectId);
				p.Add("BlockId", progress.ContractDetail.BlockId);
				p.Add("WorkId", progress.ContractDetail.WorkId);
				p.Add("GroupId", progress.ContractDetail.GroupId);
				p.Add("ItemId", progress.ContractDetail.ItemId);
				p.Add("SubItemId", progress.ContractDetail.SubItemId);
				p.Add("Initial", progress.Initial);
				p.Add("Current", progress.Current);
				p.Add("User", user);
				batch[i] = p;
				i++;
			}

			return DBProgress.Add(batch);
		}

		public static DataSet GetPaid(int paymentId)
		{
			return DBProgress.GetPaid(paymentId);
		}
    }
}
