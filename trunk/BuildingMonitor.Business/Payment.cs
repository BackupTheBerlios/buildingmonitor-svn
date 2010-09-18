using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public class Payment
	{
		private int _id = -1;
		private string _createdBy;
		private string _currency;
		private string _check;
		private string _bankAccount;
		private string _contractorPaymentType;
		private decimal _amount;
		private decimal _exchangeRate;
		private DateTime _date;
		private int _contractorAccount = -1;
		private List<int> _progressIds = new List<int>();

		public Payment()
		{
		}

		public Payment(int id)
		{
			_id = id;
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

		public string Check
		{
			get
			{
				return _check;
			}
			set
			{
				_check = value;
			}
		}

		public string BankAccount
		{
			get
			{
				return _bankAccount;
			}
			set
			{
				_bankAccount = value;
			}
		}

		public string ContractorPaymentType
		{
			get
			{
				return _contractorPaymentType;
			}
			set
			{
				_contractorPaymentType = value;
			}
		}

		public decimal ExchangeRate
		{
			get
			{
				return _exchangeRate;
			}
			set
			{
				_exchangeRate = value;
			}
		}

		public decimal Amount
		{
			get
			{
				return _amount;
			}
			set
			{
				_amount = value;
			}
		}

		public DateTime Date
		{
			get
			{
				return _date;
			}
			set
			{
				_date = value;
			}
		}

		public int ContractorAccount
		{
			get
			{
				return _contractorAccount;
			}
			set
			{
				_contractorAccount = value;
			}
		}

		public List<int> ProgressIds
		{
			get
			{
				return _progressIds;
			}
		}

		public virtual int Save()
		{
			if (_contractorPaymentType == "cash")
				return DBPayment.AddCash(_createdBy, _currency, _amount, _exchangeRate, _progressIds);
			else if (_contractorPaymentType == "check")
				return DBPayment.AddCheck(_createdBy, _currency, _bankAccount, _check, _amount, _exchangeRate, _progressIds);
			else
				return DBPayment.AddBankAccount(_createdBy, _currency, _bankAccount, _amount, _exchangeRate, _contractorAccount, _progressIds);			
		}

		public static IDataReader GetAllAccounts()
		{
			return DBPayment.GetAllAccounts();
		}

		public static Payment Create(int id)
		{
			Payment payment = new Payment(id);

			using (IDataReader reader = DBPayment.Get(id))
			{
				if (reader.Read())
				{
					payment.Currency = (string)reader["Currency"];

					if (payment.Currency.StartsWith("$"))
						payment.Amount = (decimal)reader["AmountSus"];
					else if (payment.Currency.StartsWith("Bs"))
						payment.Amount = (decimal)reader["AmountBs"];
					else
						payment.Amount = (decimal)reader["Amount"];

					payment.Check = reader["CheckNumber"].ToString();
					payment.BankAccount = reader["BankAccount"].ToString();
					payment.Date = (DateTime)reader["Date"];
					payment.ContractorAccount = reader["ContractorAccount"] == DBNull.Value ? -1 : (int)reader["ContractorAccount"];

					if (!string.IsNullOrEmpty(payment.Check))
						payment.ContractorPaymentType = "check";
					else if (payment.ContractorAccount > 0)
						payment.ContractorPaymentType = "bank";
					else
						payment.ContractorPaymentType = "cash";
				}
			}

			return payment;
		}
	}

	public class PaymentPaidWork : Payment
	{
		public int ProgressRequired
		{
			get;
			set;
		}

		public int ContractId
		{
			get;
			set;
		}

		public override int Save()
		{
			if (ContractorPaymentType == "cash")
				return DBPayment.AddCash(CreatedBy, Currency, Amount, ExchangeRate, ProgressRequired, ContractId);
			else if (ContractorPaymentType == "check")
			    return DBPayment.AddCheck(CreatedBy, Currency, BankAccount, Check, Amount, ExchangeRate, ProgressRequired, ContractId);
			else
			    return DBPayment.AddBankAccount(CreatedBy, Currency, BankAccount, Amount, ExchangeRate, ContractorAccount, ProgressRequired, ContractId);
		}

		public static IDataReader GetPaidPlan(int contractId, int paymentId)
		{
			return DBPayment.GetPaidPlan(contractId, paymentId);
		}
	}
}
