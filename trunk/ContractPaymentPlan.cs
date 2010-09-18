using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
	public class ContractPaymentPlan
	{
		private DateTime _date;
		private double _amount;
		private int _condition;

		public ContractPaymentPlan()
		{
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

		public double Amount
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

		public int Condition
		{
			get
			{
				return _condition;
			}
			set
			{
				_condition = value;
			}
		}
	}
}
