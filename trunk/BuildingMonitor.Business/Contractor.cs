using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using BuildingMonitor.Data;


namespace BuildingMonitor.Business
{
	public class Contractor
	{
		#region Constructors
		
		public Contractor()
		{
		}

		public Contractor(int id)
		{
			_id = id;
		}

		#endregion

		#region Private Fields

		private int _id = -1;
		private string _name = string.Empty;
		private string _phone = string.Empty;
		private string _movile = string.Empty;
		private string _status = string.Empty;
		private List<string> _specialties = new List<string>();
		private List<ContractorBankAccount> _bankAccounts = new List<ContractorBankAccount>();

		#endregion

		#region Public Properties

		public int Id
		{
			get
			{
				return _id;
			}
		}

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

		public string Phone
		{
			get
			{
				return _phone;
			}
			set
			{
				_phone = value;
			}
		}

		public string Movile
		{
			get
			{
				return _movile;
			}
			set
			{
				_movile = value;
			}
		}

		public string StatusCode
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}

		public List<string> Specialties
		{
			get
			{
				return _specialties;
			}
		}

		public List<ContractorBankAccount> BankAccounts
		{
			get
			{
				return _bankAccounts;
			}
		}

		#endregion

		#region Public Methods

		public bool Save()
		{
			int affectedRows = 0;
			List<ListDictionary> bankAccountsAdd = new List<ListDictionary>();
			List<ListDictionary> bankAccountsUpdate = new List<ListDictionary>();
			
			if (_id > 0)
			{
				affectedRows = DBContractor.Update(_id, _name, _phone, _movile, _status, _specialties.ToArray());
			}
			else
			{
				affectedRows = DBContractor.Add(_name, _phone, _movile, _status, _specialties.ToArray());
			}

			foreach (ContractorBankAccount cba in _bankAccounts)
			{
				if (string.IsNullOrEmpty(cba.Bank) || string.IsNullOrEmpty(cba.AccountNumber))
					continue;

				ListDictionary bankAccount = new ListDictionary();

				bankAccount.Add("Id", cba.Id);
				bankAccount.Add("AccountNumber", cba.AccountNumber);
				bankAccount.Add("Bank", cba.Bank);

				if (cba.Id < 0)
					bankAccountsAdd.Add(bankAccount);
				else
					bankAccountsUpdate.Add(bankAccount);
			}

			DBContractor.AddBankAccount(_id, bankAccountsAdd);
			DBContractor.UpdateBankAccount(_id, bankAccountsUpdate);

			return affectedRows > 0;
		}

		#endregion

		#region Public Static Methods

		public static IDataReader GetAll()
		{
			return DBContractor.GetAll();
		}
		
		public static IDataReader GetAllSpecialties()
		{
			return DBContractor.GetAllSpecialties();
		}

		public static IDataReader GetAllStatus()
		{
			return DBContractor.GetAllStatus();
		}

		public static DataSet GetAllDataSet()
		{
			return DBContractor.GetAllDataSet();
		}

		public static IDataReader GetAllAccounts(int contractorId)
		{
			return DBContractor.GetBankAccounts(contractorId);
		}

		public static int GetIdFromContract(int contractId)
		{
			return DBContractor.GetIdFromContract(contractId);
		}

		public static Contractor Empty
		{
			get
			{
				return new Contractor();
			}
		}
		
		public static Contractor Create(int id)
		{
			Contractor contractor = null;
			
			using (IDataReader reader = DBContractor.Get(id))
			{
				if(reader.Read())
				{
					contractor = new Contractor(id);
					contractor.Name = reader["Name"].ToString();
					contractor.Phone = reader["Phone"].ToString();
					contractor.Movile = reader["Movile"].ToString();
					contractor.StatusCode = reader["Status"].ToString();
				}
			}
			
			if (contractor == null)
				return Contractor.Empty;
			
			using (IDataReader reader = DBContractor.GetAllSpecialties(id))
			{
				while (reader.Read())
				{
					contractor.Specialties.Add(reader["SpecialtyId"].ToString());
				}
			}

			using (IDataReader reader = DBContractor.GetBankAccounts(id))
			{
				while (reader.Read())
				{
					ContractorBankAccount bankAccount = new ContractorBankAccount(Convert.ToInt32(reader["ContractorAccountId"]));

					bankAccount.AccountNumber = reader["Account"].ToString();
					bankAccount.Bank = reader["Bank"].ToString();
					contractor.BankAccounts.Add(bankAccount);
				}
			}

			return contractor;
		}

		
		#endregion
	}

	public class ContractorBankAccount
	{
		private int _id;

		public int Id
		{
			get
			{
				return _id;
			}
		}

		public string Bank
		{
			get;
			set;
		}

		public string AccountNumber
		{
			get;
			set;
		}

		public ContractorBankAccount(int id)
		{
			_id = id;
		}

		public static ContractorBankAccount Create(int contractorId, int contractorBankAccountId)
		{
			ContractorBankAccount bankAccount = new ContractorBankAccount(contractorId);

			using (IDataReader reader = DBContractor.GetBankAccount(contractorId, contractorBankAccountId))
			{
				if (reader.Read())
				{
					bankAccount.Bank = (string)reader["Bank"];
					bankAccount.AccountNumber = (string)reader["Account"];
				}
			}

			return bankAccount;
		}
	}
}
