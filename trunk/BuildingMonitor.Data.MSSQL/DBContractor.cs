using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using mojoPortal.Data;

namespace BuildingMonitor.Data
{
	public class DBContractor
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}

		public static IDataReader Get(int id)
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, 
				"SELECT Nombre AS Name," +
                "TelefonoFijo AS Phone," +
                "Celular AS Movile," +
                "CodigoEstado AS Status " +
                "FROM uContratista WHERE Id=" + id.ToString(), 
                CommandType.Text, 0);
			
			return sph.ExecuteReader();
		}

		public static int GetIdFromContract(int contractId)
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString,
				"SELECT IdContratista IdContractor " +
				"FROM uContrato WHERE Id=" + contractId,
				CommandType.Text, 0);
			
			object result = sph.ExecuteScalar();

			return result == null ? -1 : (int)result;
		}

        public static IDataReader GetAll()
        {
            SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, 
                "SELECT Id AS IdContractor," +
                "Nombre AS Name," +
                "TelefonoFijo AS Phone," +
                "Celular AS Movile " +
                "FROM uContratista ORDER BY Nombre", 
                CommandType.Text, 0);

            return sph.ExecuteReader();
        }


		public static DataSet GetAllDataSet()
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString,
				"SELECT Id AS ContractorId," +
				"Nombre AS Name," +
				"TelefonoFijo AS Phone," +
				"Celular AS Movile," +
				"CodigoEstado AS Status " + 
				"FROM uContratista",
				CommandType.Text, 0);

			return sph.ExecuteDataset();
		}

		public static IDataReader GetBankAccounts(int contractorId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id ContractorAccountId,");
			sb.Append("Banco Bank,");
			sb.Append("NumCuenta Account ");
			sb.AppendFormat("FROM {0}CuentaContratista ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE IdContratista={0}", contractorId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetBankAccount(int contractorId, int contractorBankAccount)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id ContractorAccountId,");
			sb.Append("Banco Bank,");
			sb.Append("NumCuenta Account ");
			sb.AppendFormat("FROM {0}CuentaContratista ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE IdContratista={0} ", contractorId);
			sb.AppendFormat("AND Id={0}", contractorBankAccount);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetAllSpecialties()
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, "SELECT Id AS IdSpecialty, Nombre AS Name FROM uEspecialidad", CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetAllSpecialties(int id)
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, 
				"SELECT e.Id AS SpecialtyId, e.Nombre AS Name " +
				"FROM uEspecialidad e INNER JOIN uContratistaEspecialidad c ON e.id=c.idEspecialidad " +
				"WHERE c.idContratista=" + id.ToString(), 
				CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader GetAllStatus()
		{
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, "SELECT Codigo AS IdStatusContractor, Estado AS Name FROM uEstadoContratista", CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static int Add(
			string name,
			string phone,
			string movile,
			string status,
			string[] specialties)
		{
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			string sql = "INSERT INTO uContratista ";
			int rowsAffected = 0;

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			if (string.IsNullOrEmpty(status))
				sql += string.Format("(Nombre,TelefonoFijo,Celular) VALUES ('{0}','{1}','{2}')", name, phone, movile);
			else
				sql += string.Format("(Nombre,TelefonoFijo,Celular,CodigoEstado) VALUES ('{0}','{1}','{2}','{3}')", name, phone, movile, status);

			try
			{
				command.CommandText = sql;
				rowsAffected = command.ExecuteNonQuery();

				command.CommandText = "SELECT @@identity";
				string id = command.ExecuteScalar().ToString();

				foreach (string specialty in specialties)
				{
					command.CommandText = string.Format("INSERT INTO uContratistaEspecialidad VALUES ({0},{1})", id, specialty);
					command.ExecuteNonQuery();
				}

				transaction.Commit();
			}
			catch (SqlException)
			{
				transaction.Rollback();
				rowsAffected = 0;
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();
			
			return rowsAffected;
		}

		public static int Update(
			int id,
			string name,
			string phone,
			string movile,
			string status,
			string[] specialties)
		{
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			string sql = "UPDATE uContratista SET ";
			int rowsAffected = 0;

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			if (string.IsNullOrEmpty(status))
				sql += string.Format("Nombre='{0}',TelefonoFijo='{1}',Celular='{2}' WHERE Id={3}", name, phone, movile, id);
			else
				sql += string.Format("Nombre='{0}',TelefonoFijo='{1}',Celular='{2}',CodigoEstado='{3}' WHERE Id={4}", name, phone, movile, status, id);

			try
			{
				command.CommandText = sql;
				rowsAffected = command.ExecuteNonQuery();

				command.CommandText = "DELETE FROM uContratistaEspecialidad WHERE IdContratista=" + id.ToString();
				command.ExecuteNonQuery();

				foreach (string specialty in specialties)
				{
					command.CommandText = string.Format("INSERT INTO uContratistaEspecialidad VALUES ({0},{1})", id, specialty);
					command.ExecuteNonQuery();
				}

				transaction.Commit();
			}
			catch (SqlException)
			{
				transaction.Rollback();
				rowsAffected = 0;
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return rowsAffected;
		}

		public static int AddBankAccount(int contractorId, List<ListDictionary> bankAccounts)
		{
			if (bankAccounts.Count == 0)
				return 0;

			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlCommand command = new SqlCommand();
			StringBuilder sb = null;
			int rowsAffected = 0;

			connection.Open();
			command.Connection = connection;

			foreach (ListDictionary bankAccount in bankAccounts)
			{
				sb = new StringBuilder();
				sb.AppendFormat("INSERT INTO {0}CuentaContratista ", DBHelper.Instance.TablePrefix);
				sb.Append("(IdContratista,Banco,NumCuenta)");
				sb.Append(" VALUES ");
				sb.AppendFormat("({0},'{1}','{2}')", contractorId, bankAccount["Bank"], bankAccount["AccountNumber"]);

				command.CommandText = sb.ToString();
				rowsAffected += command.ExecuteNonQuery();
			}

			command.Dispose();
			connection.Dispose();

			return rowsAffected;
		}

		public static int UpdateBankAccount(int contractorId, List<ListDictionary> bankAccounts)
		{
			if (bankAccounts.Count == 0)
				return 0;

			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlCommand command = new SqlCommand();
			StringBuilder sb = null;
			int rowsAffected = 0;

			connection.Open();
			command.Connection = connection;

			foreach (ListDictionary bankAccount in bankAccounts)
			{
				sb = new StringBuilder();
				sb.AppendFormat("UPDATE {0}CuentaContratista ", DBHelper.Instance.TablePrefix);
				sb.AppendFormat("SET Banco='{0}',NumCuenta='{1}' ", bankAccount["Bank"], bankAccount["AccountNumber"]);
				sb.AppendFormat("WHERE IdContratista={0} AND Id={1}", contractorId, bankAccount["Id"]);
				command.CommandText = sb.ToString();
				rowsAffected += command.ExecuteNonQuery();
			}

			command.Dispose();
			connection.Dispose();

			return rowsAffected;
		}
	}
}
