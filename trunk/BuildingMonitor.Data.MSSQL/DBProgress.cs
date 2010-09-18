using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using mojoPortal.Data;

namespace BuildingMonitor.Data
{
	public class DBProgress
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}
		
		public static IDataReader GetLast(int contractId,
			int projectId, 
			int blockId, 
			int workId, 
			int groupId, 
			int itemId,
			int subItemId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT TOP 1 ");
			sb.Append("Id ProgressId,");
			sb.Append("Fecha Date,");
			sb.Append("AvanceInicial InitialProgress,");
			sb.Append("AvanceActual CurrentProgress ");
			sb.Append("FROM uAvance ");
			sb.Append("WHERE ");
			//sb.AppendFormat("idContrato={0} AND ", contractId);
			sb.AppendFormat("idProyecto={0} AND ", projectId);
			sb.AppendFormat("idBloque={0} AND ", blockId);
			sb.AppendFormat("idObra={0} AND ", workId);
			sb.AppendFormat("idGrupo={0} AND ", groupId);
			sb.AppendFormat("idItem={0} AND ", itemId);
			sb.AppendFormat("idSubItem={0} ", subItemId);
			sb.Append("ORDER BY Fecha DESC");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);
			
			return sph.ExecuteReader();
		}


		public static DataSet GetPaid(int paymentId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id ProgressId,");
			sb.Append("IdProyecto ProjectId,");
			sb.Append("IdBloque BlockId,");
			sb.Append("IdObra WorkId,");
			sb.Append("IdGrupo GroupId,");
			sb.Append("IdItem ItemId,");
			sb.Append("IdSubItem SubItemId,");
			sb.Append("IdContrato ContractId,");
			sb.Append("Fecha Date,");
			sb.Append("AvanceInicial InitialProgress,");
			sb.Append("AvanceActual CurrentProgress ");
			sb.AppendFormat("FROM {0}Avance ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE IdPago={0} ", paymentId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteDataset();
		}

		public static bool Add(int contractId,
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
			StringBuilder sb = new StringBuilder();
			
			sb.Append("INSERT INTO uAvance ");
			sb.Append("(IdContrato,IdProyecto,IdBloque,IdObra,IdGrupo,IdItem,IdSubItem,AvanceInicial,AvanceActual,Usuario,Fecha)");
			sb.Append(" VALUES ");
			sb.AppendFormat("({0},{1},{2},{3},{4},{5},{6},{7},{8},'{9}',GETDATE())", contractId, projectId, blockId, workId, groupId, itemId, subItemId, initialProgress, currentProgress, user);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);
			int rows = 0;
			
			try
			{
				rows = sph.ExecuteNonQuery();
			}
			catch(Exception)
			{
				rows = 0;
			}
			
			return rows > 0;
		}

		public static bool Add(Dictionary<string, object>[] batchProgress)
		{
			StringBuilder sb = null;
			string prefix = DBHelper.Instance.TablePrefix;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			bool isOk = false;

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			try
			{
				foreach (Dictionary<string, object> progress in batchProgress)
				{
					sb = new StringBuilder();
					sb.AppendFormat("INSERT INTO {0}Avance ", prefix);
					sb.Append("(IdContrato,IdProyecto,IdBloque,IdObra,IdGrupo,IdItem,IdSubItem,AvanceInicial,AvanceActual,Usuario,Fecha)");
					sb.Append(" VALUES ");
					sb.AppendFormat("({0},{1},{2},{3},{4},{5},{6},{7},{8},'{9}',GETDATE())",
						progress["ContractId"],
						progress["ProjectId"],
						progress["BlockId"],
						progress["WorkId"],
						progress["GroupId"],
						progress["ItemId"],
						progress["SubItemId"],
						progress["Initial"],
						progress["Current"],
						progress["User"]);

					command.CommandText = sb.ToString();
					command.ExecuteNonQuery();
				}

				transaction.Commit();
				isOk = true;
			}
			catch (Exception)
			{
				transaction.Rollback();
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return isOk;
		}
	}
}
