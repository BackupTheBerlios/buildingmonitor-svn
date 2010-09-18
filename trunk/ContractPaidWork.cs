using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public class ContractPaidWork: Contract
	{
		#region Private Fields

		private double _advance;
		private List<ContractPaymentPlan> _paymentPlan = new List<ContractPaymentPlan>();

		#endregion

		#region Constructors

		public ContractPaidWork()
		{
		}

		public ContractPaidWork(int id): base(id)
		{
		}

		#endregion

		#region Public Properties

		public double Advance
		{
			get
			{
				return _advance;
			}
			set
			{
				_advance = value;
			}
		}

		public List<ContractPaymentPlan> PaymentPlan
		{
			get
			{
				return _paymentPlan;
			}
		}

		#endregion

		public override int Save()
		{
			if (Id < 0)
			{
				DBContractDetail[] dbContractDetail = new DBContractDetail[Detail.Count];

				for (int i = 0; i < Detail.Count; i++)
				{
					dbContractDetail[i] = ContractDetail.ToDBContractDetail(Detail[i]);
				}
				
				List<Dictionary<string, object>> listPaymentPlan = new List<Dictionary<string,object>>();
				foreach (ContractPaymentPlan paymentPlan in PaymentPlan)
				{
					Dictionary<string, object> plan = new Dictionary<string,object>();
					
					plan.Add("Amount", paymentPlan.Amount);
					plan.Add("Date", paymentPlan.Date);
					plan.Add("Condition", paymentPlan.Condition);
					listPaymentPlan.Add(plan);
				}


				return DBContract.AddPaidWork(ContractorId, Currency, CreatedBy, Description, ToString(ContractStatus.Active), Start, End, ExchangeRate, Amount, _advance, dbContractDetail, listPaymentPlan);
			}

			return -1;
		}
	}
}
