using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public enum ContractStatus
	{
		None = 0,
		Active = 1,
		Canceled = 2,
		Finished = 3
	}

	public class Contract
	{
		#region Constructors
		
		public Contract()
		{
		}
		
		public Contract(int id)
		{
			_id = id;
		}
		
		#endregion
		
		#region Private Fields
		
		private int _id = -1;
		private int _contractorId;
		private decimal _rate;
		private string _currency;
		private string _createdBy;
		private string _modifiedBy;
		private string _description;
		private DateTime _start;
		private DateTime _end;
		private DateTime _createdOn;
		private DateTime _modifiedOn;
		private List<ContractDetail> _detail = new List<ContractDetail>();
		
		#endregion

		public int Id
		{
			get
			{
				return _id;
			}
		}

		#region Public Properties

		public ContractStatus Status
		{
			get;
			set;
		}

		public List<ContractDetail> Detail
		{
			get
			{
				return _detail;
			}
			set
			{
				if (value == null)
					return;

				_detail = value;
			}
		}
		
		public int ContractorId
		{
			get
			{
				return _contractorId;
			}
			set
			{
				_contractorId = value;
			}
		}
		public decimal Amount
		{
			get
			{
				decimal amount = 0;
				
				foreach(ContractDetail detail in _detail)
				{
					amount += detail.Price;
				}
				
				return amount;
			}
		}
		
		public decimal ExchangeRate
		{
			get
			{
				return _rate;
			}
			set
			{
				_rate = value;
			}
		}
		
		public string Currency
		{
			get
			{
				return _currency;
			}
			set
			{
				_currency = value;
			}
		}
		
		public string CreatedBy
		{
			get
			{
				return _createdBy;
			}
			set
			{
				_createdBy = value;
			}
		}
		
		public string ModifiedBy
		{
			get
			{
				return _modifiedBy;
			}
			set
			{
				_modifiedBy = value;
			}
		}
		
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}
		
		public DateTime CreatedOn
		{
			get
			{
				return _createdOn;
			}
			set
			{
				_createdOn = value;
			}
		}
		
		public DateTime ModifiedOn
		{
			get
			{
				return _modifiedOn;
			}
			set
			{
				_modifiedOn = value;
			}
		}
		
		public DateTime Start
		{
			get
			{
				return _start;
			}
			set
			{
				_start = value;
			}
		}
		
		public DateTime End
		{
			get
			{
				return _end;
			}
			set
			{
				_end = value;
			}
		}
		
		#endregion
		
		#region Public Methods
		
		public virtual int Save()
		{
			if (_id < 0)
			{
				DBContractDetail[] dbContractDetail = new DBContractDetail[Detail.Count];
				
				for(int i=0; i < _detail.Count; i++)
				{
					dbContractDetail[i] = ContractDetail.ToDBContractDetail(_detail[i]);
				}
				
				return DBContract.Add(_contractorId, _currency, _createdBy, _description, ToString(ContractStatus.Active), _start, _end, _rate, Amount, dbContractDetail);
			}

			return -1;
		}

		public bool UpdateStatus(ContractStatus status)
		{
			return DBContract.UpdateStatus(_id, ToString(status));
		}

		public DataSet GetPayableSubItems()
		{
			return DBSubItem.GetAllPayable(Id);
		}

		#endregion

		protected static string ToString(ContractStatus status)
		{
			if ((int)status <= 0)
				return string.Empty;

			return status.ToString("g").ToLower();
		}

		protected static ContractStatus ToContractStatus(string status)
		{
			if (string.IsNullOrEmpty(status))
				return ContractStatus.None;

			switch (status.ToLower())
			{
				case "active":
					return ContractStatus.Active;
				case "canceled":
					return ContractStatus.Canceled;
				case "finished":
					return ContractStatus.Finished;
			}

			return ContractStatus.None;
		}

		public static DataSet GetAll(int contractorId, bool isPaidWork, ContractStatus status)
		{
			return DBContract.GetAll(contractorId, isPaidWork, ToString(status));
		}

		public static DataSet GetAllContract(int contractorId)
		{
			return DBContract.GetAllContract(contractorId);
		}

		public static IDataReader PayablePaidWorkPlan(int contractId)
		{
			return DBContract.PayablePaidWorkPlan(contractId);
		}

		public static IDataReader Get(int contractId)
		{
			return DBContract.Get(contractId);
		}

		public static IDataReader GetDetail(int contractId)
		{
			return DBContract.GetDetail(contractId);
		}

		public static Contract Create(int contractId)
		{
			Contract contract = new Contract(contractId);

			using (IDataReader reader = DBContract.Get(contractId))
			{
				if (reader.Read())
				{
					contract.Status = ToContractStatus(reader["Status"].ToString());
					contract.Currency = reader["Currency"].ToString();
					contract.Description = reader["Gloss"].ToString();
					contract.ExchangeRate = Convert.ToDecimal(reader["CurrencyRate"]);
					contract.Start = Convert.ToDateTime(reader["DateStart"]);
					contract.End = Convert.ToDateTime(reader["DateEnd"]);
					contract.CreatedOn = Convert.ToDateTime(reader["DateCreated"]);
					contract.ContractorId = Convert.ToInt32(reader["ContractorId"]);
					//contract.Amount = Convert.ToDecimal(reader["Amount"]);
				}
			}

			return contract;
		}

		public static Contract Create(int contractId, bool fillDetail)
		{
			Contract contract = Create(contractId);

			if (!fillDetail)
				return contract;

			using (IDataReader reader = DBContract.GetDetail(contractId))
			{
				while (reader.Read())
				{
					ContractDetail detail = new ContractDetail();

					detail.ProjectId = Convert.ToInt32(reader["ProjectId"]);
					detail.BlockId = Convert.ToInt32(reader["BlockId"]);
					detail.WorkId = Convert.ToInt32(reader["WorkId"]);
					detail.GroupId = Convert.ToInt32(reader["GroupId"]);
					detail.ItemId = Convert.ToInt32(reader["ItemId"]);
					detail.SubItemId = Convert.ToInt32(reader["SubItemId"]);
					detail.Price = Convert.ToDecimal(reader["Price"]);
					detail.Name = reader["Name"].ToString();
					detail.Quantity = Convert.ToDecimal(reader["Quantity"]);
					detail.Unit = reader["Unit"].ToString();
					detail.InitialProgress = Convert.ToInt32(reader["Progress"]);
					contract.Detail.Add(detail);
				}
			}

			return contract;
		}
	}
}
