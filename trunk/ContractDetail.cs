using System;
using System.Data;
using BuildingMonitor.Data;


namespace BuildingMonitor.Business
{
	public class ContractDetail
	{
		#region Constructors
		
		public ContractDetail()
		{
		}
		
		#endregion
		
		#region Public Properties
		
		public int ContractId
		{
			get;
			set;
		}
		
		public int ProjectId
		{
			get;
			set;
		}
		
		public int BlockId
		{
			get;
			set;
		}
		
		public int WorkId
		{
			get;
			set;
		}
		
		public int GroupId
		{
			get;
			set;
		}
		
		public int ItemId
		{
			get;
			set;
		}
		
		public int SubItemId
		{
			get;
			set;
		}

		public decimal Price
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Unit
		{
			get;
			set;
		}

		public decimal Quantity
		{
			get;
			set;
		}

		public int InitialProgress
		{
			get;
			set;
		}

		#endregion
		
		#region Public Methods
		
		public static IDataReader GetAll(int projectId, int blockId, int workId, int groupId, int itemId)
		{
			return DBContract.GetAllDetails(projectId, blockId, workId, groupId, itemId);
		}
		
		#endregion
		
		internal static DBContractDetail ToDBContractDetail(ContractDetail cd)
		{
			return new DBContractDetail(cd.ProjectId, cd.BlockId, cd.WorkId, cd.GroupId, cd.ItemId, cd.SubItemId, cd.InitialProgress, cd.Price);
		}
	}
}
