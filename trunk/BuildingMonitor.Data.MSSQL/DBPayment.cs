using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;

namespace BuildingMonitor.Data
{
	public class DBPayment
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}

		public static IDataReader Get(int paymentId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT CodigoMoneda Currency,");
			sb.Append("Fecha Date,");
			sb.Append("Monto Amount,");
			sb.Append("MontoBs AmountBs,");
			sb.Append("MontoSus AmountSus,");
			sb.Append("NumCheque CheckNumber,");
			sb.Append("CuentaBancaria BankAccount,");
			sb.Append("IdCuentaContratista ContractorAccount ");
			sb.AppendFormat("FROM {0}Pago ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE Id={0}", paymentId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetAllAccounts()
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, "SELECT NumCuenta Account,Banco Bank,Descripcion Description FROM tCuentaBancaria", CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetPaidPlan(int contractId, int paymentId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT TOP 1 ");
			sb.Append("CondicionAvance ProgressRequired,");
			sb.Append("Monto Amount,");
			sb.Append("Fecha Date ");
			sb.AppendFormat("FROM {0}PlanPagoContrato ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE IdContrato={0} AND IdPago={1}", contractId, paymentId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static int AddBankAccount(
			string createdBy,
			string currency,
			string bankAccount,
			decimal amount,
			decimal exchangeRate,
			int contractorAccount,
			List<int> progressIds)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			int paymentId = -1;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			StringBuilder sb = new StringBuilder();

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;
			
			sb.AppendFormat("INSERT INTO {0}Pago ", prefix);
			sb.Append("(CodigoMoneda,Fecha,Monto,TipoCambio,CuentaBancaria,Usuario,IdCuentaContratista)");
			sb.Append(" VALUES ");
			sb.AppendFormat("('{0}',GETDATE(),{1},{2},'{3}','{4}',{5})", currency, DBConverter.ToSafeDecimal(amount), DBConverter.ToSafeDecimal(exchangeRate), bankAccount, createdBy, contractorAccount);
			sb.AppendFormat(";SELECT Id FROM {0}Pago WHERE Id=@@IDENTITY", prefix);

			try
			{
				command.CommandText = sb.ToString();
				paymentId = (int)command.ExecuteScalar();
				
				foreach (int progressId in progressIds)
				{
					command.CommandText = string.Format("UPDATE {0}Avance SET IdPago={1} WHERE Id={2}", prefix, paymentId, progressId);
					command.ExecuteNonQuery();
				}

				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return paymentId;
		}


		public static int AddBankAccount(
			string createdBy,
			string currency,
			string bankAccount,
			decimal amount,
			decimal exchangeRate,
			int contractorAccount,
			int progressRequired,
			int contractId)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			int paymentId = -1;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			StringBuilder sb = new StringBuilder();

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			sb.AppendFormat("INSERT INTO {0}Pago ", prefix);
			sb.Append("(CodigoMoneda,Fecha,Monto,TipoCambio,CuentaBancaria,Usuario,IdCuentaContratista)");
			sb.Append(" VALUES ");
			sb.AppendFormat("('{0}',GETDATE(),{1},{2},'{3}','{4}',{5})", currency, DBConverter.ToSafeDecimal(amount), DBConverter.ToSafeDecimal(exchangeRate), bankAccount, createdBy, contractorAccount);
			sb.AppendFormat(";SELECT Id FROM {0}Pago WHERE Id=@@IDENTITY", prefix);

			try
			{
				command.CommandText = sb.ToString();
				paymentId = (int)command.ExecuteScalar();
				command.CommandText = string.Format("UPDATE {0}PlanPagoContrato SET IdPago={1} WHERE IdContrato={2} AND CondicionAvance={3}", prefix, paymentId, contractId, progressRequired);
				command.ExecuteNonQuery();
				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return paymentId;
		}


		public static int AddCheck(
			string createdBy,
			string currency,
			string bankAccount,
			string checkNumber,
			decimal amount,
			decimal exchangeRate,
			List<int> progressIds)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			int paymentId = -1;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			StringBuilder sb = new StringBuilder();

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			sb.AppendFormat("INSERT INTO {0}Pago ", prefix);
			sb.Append("(CodigoMoneda,Fecha,Monto,TipoCambio,CuentaBancaria,Usuario,NumCheque)");
			sb.Append(" VALUES ");
			sb.AppendFormat("('{0}',GETDATE(),{1},{2},'{3}','4',{5})", currency, DBConverter.ToSafeDecimal(amount), DBConverter.ToSafeDecimal(exchangeRate), bankAccount, createdBy, checkNumber);
			sb.AppendFormat(";SELECT Id FROM {0}Pago WHERE Id=@@IDENTITY", prefix);

			try
			{
				command.CommandText = sb.ToString();
				paymentId = (int)command.ExecuteScalar();

				foreach (int progressId in progressIds)
				{
					command.CommandText = string.Format("UPDATE {0}Avance SET IdPago={1} WHERE Id={2}", prefix, paymentId, progressId);
					command.ExecuteNonQuery();
				}

				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return paymentId;
		}

		public static int AddCheck(
			string createdBy,
			string currency,
			string bankAccount,
			string checkNumber,
			decimal amount,
			decimal exchangeRate,
			int progressRequired,
			int contractId)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			int paymentId = -1;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			StringBuilder sb = new StringBuilder();

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			sb.AppendFormat("INSERT INTO {0}Pago ", prefix);
			sb.Append("(CodigoMoneda,Fecha,Monto,TipoCambio,CuentaBancaria,Usuario,NumCheque)");
			sb.Append(" VALUES ");
			sb.AppendFormat("('{0}',GETDATE(),{1},{2},'{3}','4',{5})", currency, DBConverter.ToSafeDecimal(amount), DBConverter.ToSafeDecimal(exchangeRate), bankAccount, createdBy, checkNumber);
			sb.AppendFormat(";SELECT Id FROM {0}Pago WHERE Id=@@IDENTITY", prefix);

			try
			{
				command.CommandText = sb.ToString();
				paymentId = (int)command.ExecuteScalar();
				command.CommandText = string.Format("UPDATE {0}PlanPagoContrato SET IdPago={1} WHERE IdContrato={2} AND CondicionAvance={3}", prefix, paymentId, contractId, progressRequired);
				command.ExecuteNonQuery();
				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return paymentId;
		}

		public static int AddCash(
			string createdBy,
			string currency,
			decimal amount,
			decimal exchangeRate,
			List<int> progressIds)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			int paymentId = -1;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			StringBuilder sb = new StringBuilder();

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			sb.AppendFormat("INSERT INTO {0}Pago ", prefix);
			sb.Append("(CodigoMoneda,Fecha,Monto,TipoCambio,Usuario)");
			sb.Append(" VALUES ");
			sb.AppendFormat("('{0}',GETDATE(),{1},{2},'{3}')", currency, DBConverter.ToSafeDecimal(amount), DBConverter.ToSafeDecimal(exchangeRate), createdBy);
			sb.AppendFormat(";SELECT Id FROM {0}Pago WHERE Id=@@IDENTITY", prefix);

			try
			{
				command.CommandText = sb.ToString();
				paymentId = (int)command.ExecuteScalar();

				foreach (int progressId in progressIds)
				{
					command.CommandText = string.Format("UPDATE {0}Avance SET IdPago={1} WHERE Id={2}", prefix, paymentId, progressId);
					command.ExecuteNonQuery();
				}

				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return paymentId;
		}


		public static int AddCash(
			string createdBy,
			string currency,
			decimal amount,
			decimal exchangeRate,
			int progressRequired,
			int contractId)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			int paymentId = -1;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			StringBuilder sb = new StringBuilder();

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			sb.AppendFormat("INSERT INTO {0}Pago ", prefix);
			sb.Append("(CodigoMoneda,Fecha,Monto,TipoCambio,Usuario)");
			sb.Append(" VALUES ");
			sb.AppendFormat("('{0}',GETDATE(),{1},{2},'{3}')", currency, DBConverter.ToSafeDecimal(amount), DBConverter.ToSafeDecimal(exchangeRate), createdBy);
			sb.AppendFormat(";SELECT Id FROM {0}Pago WHERE Id=@@IDENTITY", prefix);

			try
			{
				command.CommandText = sb.ToString();
				paymentId = (int)command.ExecuteScalar();
				
				command.CommandText = string.Format("UPDATE {0}PlanPagoContrato SET IdPago={1} WHERE IdContrato={2} AND CondicionAvance={3}", prefix, paymentId, contractId, progressRequired);
				command.ExecuteNonQuery();
				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return paymentId;
		}
	}
}
