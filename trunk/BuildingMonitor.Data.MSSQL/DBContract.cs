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
	public struct DBContractDetail
	{
		public int ProjectId;
		public int BlockId;
		public int WorkId;
		public int GroupId;
		public int ItemId;
		public int SubItemId;
		public int Progress;
		public decimal Price;
		
		
		public DBContractDetail(int projectId, int blockId, int workId, int groupId, int itemId, int subItemId, int progress, decimal price)
		{
			ProjectId = projectId;
			BlockId = blockId;
			WorkId = workId;
			GroupId = groupId;
			ItemId = itemId;
			SubItemId = subItemId;
			Progress = progress;
			Price = price;
		}
	}
	
	public class DBContract
	{
		public static String DBPlatform()
		{
			return "MSSQL";
		}
		
		public static int Add(int contractorId,
			string currency,
			string createdBy,
			string description,
			string status,
			DateTime start,
			DateTime end,
			decimal rate,
			decimal amount,
			DBContractDetail[] listDetail)
		{
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			int identity = -1;

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;
			
			try
			{
				command.CommandText = "INSERT INTO uContrato " +
					"(IdContratista,CodigoMoneda,Monto,FechaCreacion,UsuarioCreador,FechaInicio,FechaFin,TipoCambio,Glosa,Estado)" +
					" VALUES " +
					string.Format("({0},'{1}',{2},GETDATE(),'{3}','{4}','{5}',{6},'{7}','{8}')", contractorId, currency, DBConverter.ToSafeDecimal(amount), createdBy, DBConverter.ToSafeDate(start), DBConverter.ToSafeDate(end), DBConverter.ToSafeDecimal(rate), description, status) +
					";SELECT Id FROM uContrato WHERE Id=@@IDENTITY";
				
				identity = (int)command.ExecuteScalar();
				
				foreach(DBContractDetail detail in listDetail)
				{
					command.CommandText = "INSERT INTO uContratoDetalle " +
						"(IdProyecto,IdBloque,IdObra,IdGrupo,IdItem,IdSubItem,IdContrato,AvanceInicial,PrecioReal,Precio)" +
						" VALUES " +
						string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{8})", detail.ProjectId, detail.BlockId, detail.WorkId, detail.GroupId, detail.ItemId, detail.SubItemId, identity, detail.Progress, DBConverter.ToSafeDecimal(detail.Price));
					command.ExecuteNonQuery();
				}
				
				transaction.Commit();
			}
			catch(Exception)
			{
				transaction.Rollback();
				identity = -1;
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();
			
			return identity;
		}

		public static int AddPaidWork(int contractorId,
			string currency,
			string createdBy,
			string description,
			string status,
			DateTime start,
			DateTime end,
			decimal rate,
			decimal amount,
			double advance,
			DBContractDetail[] listDetail,
			List<Dictionary<string, object>> paymentPlan)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlTransaction transaction;
			SqlCommand command = new SqlCommand();
			int identity = -1;

			connection.Open();
			transaction = connection.BeginTransaction();
			command.Connection = connection;
			command.Transaction = transaction;

			try
			{
				command.CommandText = string.Format("INSERT INTO {0}Contrato ", prefix) +
					"(IdContratista,CodigoMoneda,Monto,FechaCreacion,UsuarioCreador,FechaInicio,FechaFin,TipoCambio,Glosa,Estado)" +
					" VALUES " +
					string.Format("({0},'{1}',{2},GETDATE(),'{3}','{4}','{5}',{6},'{7}','{8}')", contractorId, currency, DBConverter.ToSafeDecimal(amount), createdBy, DBConverter.ToSafeDate(start), DBConverter.ToSafeDate(end), DBConverter.ToSafeDecimal(rate), description, status) +
					";SELECT Id FROM uContrato WHERE Id=@@IDENTITY";

				identity = (int)command.ExecuteScalar();

				foreach (DBContractDetail detail in listDetail)
				{
					command.CommandText = "INSERT INTO uContratoDetalle " +
						"(IdProyecto,IdBloque,IdObra,IdGrupo,IdItem,IdSubItem,IdContrato,AvanceInicial,PrecioReal,Precio)" +
						" VALUES " +
						string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{8})", detail.ProjectId, detail.BlockId, detail.WorkId, detail.GroupId, detail.ItemId, detail.SubItemId, identity, detail.Progress, DBConverter.ToSafeDecimal(detail.Price));

					command.ExecuteNonQuery();
				}

				command.CommandText = string.Format("INSERT INTO {0}Pago ", prefix) +
					"(CodigoMoneda,Fecha,Monto,TipoCambio,Usuario)" +
					" VALUES " +
					string.Format("('{0}',GETDATE(),{1},{2},'{3}')", currency, DBConverter.ToSafeDecimal(advance), DBConverter.ToSafeDecimal(rate), createdBy) +
					string.Format(";SELECT Id FROM {0}Pago WHERE Id=@@IDENTITY", prefix);
				
				int identityPayment = (int)command.ExecuteScalar();

				command.CommandText = string.Format("INSERT INTO {0}ContratoObraVendida ", prefix) +
					"(IdContrato,IdPago)" +
					" VALUES " +
					string.Format("({0},{1})", identity, identityPayment);

				command.ExecuteNonQuery();

				foreach (Dictionary<string, object> itemPlan in paymentPlan)
				{
					command.CommandText = string.Format("INSERT INTO {0}PlanPagoContrato ", prefix) +
						"(IdContrato,Fecha,Monto,CondicionAvance)" +
						" VALUES " +
						string.Format("({0},'{1}',{2},{3})", identity, DBConverter.ToSafeDate((DateTime)itemPlan["Date"]), DBConverter.ToSafeDecimal((double)itemPlan["Amount"]), (int)itemPlan["Condition"]);
					command.ExecuteNonQuery();
				}

				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
				identity = -1;
			}

			connection.Close();
			command.Dispose();
			transaction.Dispose();
			connection.Dispose();

			return identity;
		}

		public static IDataReader GetAllDetails(int projectId, int blockId, int workId, int groupId, int itemId)
		{
			StringBuilder sb = new StringBuilder();
			string prefix = DBHelper.Instance.TablePrefix;
			
			sb.Append("SELECT s.Nombre Name,");
			sb.Append("s.Cantidad Quantity,");
			sb.Append("l.Unidad Unit,");
			sb.Append("d.Precio Price,");
			sb.Append("d.IdContrato ContractId,");
			sb.Append("d.IdProyecto ProjectId,");
			sb.Append("d.IdBloque BlockId,");
			sb.Append("d.IdObra WorkId,");
			sb.Append("d.IdGrupo GroupId,");
			sb.Append("d.IdItem ItemId,");
			sb.Append("d.IdSubItem SubItemId ");
			sb.AppendFormat("FROM {0}ContratoDetalle d INNER JOIN {0}Contrato c ON d.IdContrato=c.Id, {0}SubItem s, {0}Item i, {0}ListaItem l ", prefix);
			sb.Append("WHERE ");
			sb.Append("d.IdProyecto=s.IdProyecto AND d.IdBloque=s.IdBloque AND d.IdObra=s.IdObra AND d.IdGrupo=s.IdGrupo AND d.IdItem=s.IdItem AND d.IdSubItem=s.Id AND ");
			sb.Append("s.IdProyecto=i.IdProyecto AND s.IdBloque=i.IdBloque AND s.IdObra=i.IdObra AND s.IdGrupo=i.IdGrupo AND s.IdItem=i.Id AND ");
			sb.Append("i.IdListaItem=l.Id AND ");
			sb.Append("c.Estado='active' ");
			
			if (projectId > 0)
			{
				sb.AppendFormat("AND d.IdProyecto={0}", projectId);
				
				if (blockId > 0)
				{
					sb.AppendFormat(" AND d.IdBloque={0}", blockId);
					
					if (workId > 0)
					{
						sb.AppendFormat(" AND d.IdObra={0}", workId);
						
						if (groupId > 0)
						{
							sb.AppendFormat(" AND d.IdGrupo={0}", groupId);
							
							if (itemId > 0)
							{
								sb.AppendFormat(" AND d.IdItem={0}", itemId);
							}
						}
					}
				}
			}
			
			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static DataSet GetAllContract(int contractorId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id,");
			sb.Append("SUBSTRING(Glosa, 0, 50) Glosa ");
			sb.AppendFormat("FROM uContrato ");
			sb.AppendFormat("WHERE IdContratista = {0}", contractorId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteDataset();
		}

		public static DataSet GetAll(int contractorId, bool isPaidWork, string status)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT Id ContractId,");
			sb.Append("IdContratista ContractorId,");
			sb.Append("FechaCreacion DateCreated,");
			sb.Append("FechaInicio DateStart,");
			sb.Append("FechaFin DateEnd,");
			sb.Append("MontoGlobalBs AmountBs,");
			sb.Append("MontoGlobalSus AmountSus,");
			sb.Append("SUBSTRING(Glosa, 0, 50) Gloss ");
			sb.AppendFormat("FROM {0}Contrato ", prefix);
			sb.AppendFormat("WHERE Id {0} IN (SELECT IdContrato FROM {1}ContratoObraVendida) ", isPaidWork ? "" : "NOT", prefix);

			if (!string.IsNullOrEmpty(status))
				sb.AppendFormat("AND Estado='{0}' ", status);

			if (contractorId > 0)
				sb.AppendFormat("AND IdContratista={0} ", contractorId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteDataset();
		}

		public static IDataReader PayablePaidWorkPlan(int contractId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT TOP 1 ");
			sb.Append("Fecha Date,");
			sb.Append("IdPago PaymentId,");
			sb.Append("Monto Amount,");
			sb.Append("CondicionAvance ProgressCondition ");
			sb.AppendFormat("FROM {0}PlanPagoContrato ", DBHelper.Instance.TablePrefix);
			sb.AppendFormat("WHERE IdContrato={0} AND IdPago IS NULL ", contractId);
			sb.Append("ORDER BY CondicionAvance");

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);

			return sph.ExecuteReader();
		}

		public static IDataReader Get(int contractId)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT CodigoMoneda Currency,");
			sb.Append("Estado Status,");
			sb.Append("Monto Amount,");
			sb.Append("FechaCreacion DateCreated,");
			sb.Append("FechaInicio DateStart,");
			sb.Append("FechaFin DateEnd,");
			sb.Append("TipoCambio CurrencyRate,");
			sb.Append("Glosa Gloss,");
			sb.Append("IdContratista ContractorId ");
			sb.AppendFormat("FROM {0}Contrato ", prefix);
			sb.AppendFormat("WHERE Id={0}", contractId);

			SqlParameterHelper sph = new SqlParameterHelper(DBHelper.Instance.ConnectionString, sb.ToString(), CommandType.Text, 0);
			
			return sph.ExecuteReader();
		}

		public static IDataReader GetDetail(int contractId)
		{
			SqlParameter[] parameters = new SqlParameter[1];

			parameters[0] = new SqlParameter("@IdContrato", contractId);

			return SqlHelper.ExecuteReader(DBHelper.Instance.ConnectionString, CommandType.StoredProcedure, "bm_ContractDetail", parameters);
		}

		public static bool UpdateStatus(int contractId, string status)
		{
			string prefix = DBHelper.Instance.TablePrefix;
			SqlConnection connection = new SqlConnection(DBHelper.Instance.ConnectionString);
			SqlCommand command = new SqlCommand();
			int affectedRows;

			connection.Open();
			command.Connection = connection;
			command.CommandText = string.Format("UPDATE {0}Contrato SET Estado='{1}' WHERE Id={2}", prefix, status, contractId);
			affectedRows = command.ExecuteNonQuery();
			connection.Dispose();
			command.Dispose();

			return affectedRows == 1;
		}
	}
}
