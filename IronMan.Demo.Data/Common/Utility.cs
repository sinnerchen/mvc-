/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IronMan.Demo.Entities;

namespace IronMan.Demo.Data
{
	public sealed class Utility
	{

		#region 值或类型转换
		public static Object GetDefaultByType(DbType dataType)
		{
			switch (dataType) {
				case DbType.AnsiString: return string.Empty;
				case DbType.AnsiStringFixedLength: return string.Empty;
				case DbType.Binary: return new byte[] { };
				case DbType.Boolean: return false;
				case DbType.Byte: return (byte)0;
				case DbType.Currency: return 0m;
				case DbType.Date: return DateTime.MinValue;
				case DbType.DateTime: return DateTime.MinValue;
				case DbType.Decimal: return 0m;
				case DbType.Double: return 0f;
				case DbType.Guid: return Guid.Empty;
				case DbType.Int16: return (short)0;
				case DbType.Int32: return 0;
				case DbType.Int64: return (long)0;
				case DbType.Object: return null;
				case DbType.Single: return 0F;
				case DbType.String: return String.Empty;
				case DbType.StringFixedLength: return string.Empty;
				case DbType.Time: return DateTime.MinValue;
				case DbType.VarNumeric: return 0;
				default: return null;
			}
		}

		public static Object GetDataValue(IDataParameter p)
		{
			if (p.Value != DBNull.Value)
				return p.Value;
			else
				return GetDefaultByType(p.DbType);
		}

		public static object DefaultToDBNull(object val, DbType dbtype)
		{
			if (val == null || Object.Equals(val, GetDefaultByType(dbtype)))
				return System.DBNull.Value;
			else
				return val;
		}

		public static T GetParameterValue<T>(IDataParameter parameter)
		{
			if (parameter.Value == System.DBNull.Value) {
				return default(T);
			} else {
				return (T)parameter.Value;
			}
		}

		public static DataSet ConvertDataReaderToDataSet(IDataReader reader)
		{
			DataSet dataSet = new DataSet();
			do {
				DataTable schemaTable = reader.GetSchemaTable();
				DataTable dataTable = new DataTable();
				if (schemaTable != null) {
					for (int i = 0; i < schemaTable.Rows.Count; i++) {
						DataRow dataRow = schemaTable.Rows[i];
						string columnName = (string)dataRow["ColumnName"];
						DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
						dataTable.Columns.Add(column);
					}
					dataSet.Tables.Add(dataTable);
					while (reader.Read()) {
						DataRow dataRow = dataTable.NewRow();
						for (int i = 0; i < reader.FieldCount; i++)
							dataRow[i] = reader.GetValue(i);
						dataTable.Rows.Add(dataRow);
					}
				} else {
					DataColumn column = new DataColumn("RowsAffected");
					dataTable.Columns.Add(column);
					dataSet.Tables.Add(dataTable);
					DataRow dataRow = dataTable.NewRow();
					dataRow[0] = reader.RecordsAffected;
					dataTable.Rows.Add(dataRow);
				}
			}
			while (reader.NextResult());
			return dataSet;
		}
    
    public static List<T> ConvertFromDataReader<T>(IDataReader reader) where T : new()
		{
			if (reader == null) return null;
			List<T> list = null;
			DataTable schemaTable = reader.GetSchemaTable();
			if (schemaTable != null) {
				T obj = new T();
				Type entityType = obj.GetType();
				PropertyInfo[] properties = entityType.GetProperties();
				while (reader.Read()) {
					T entity = new T();
					for (int i = 0; i < properties.Length; i++) {
						for (int j = 0; j < schemaTable.Rows.Count; j++) {
							if (properties[i].Name.ToLower().Equals(schemaTable.Rows[j]["ColumnName"].ToString().ToLower())) {
								if (reader.GetValue(j).GetType() != typeof(System.DBNull)) {
									properties[i].SetValue(entity, reader.GetValue(j), null);
								}
							}
						}
					}
					list.Add(entity);
				}
				return list;
			} else {
				throw new Exception("Can not convert from no structural dataReader.");
			}
		}

		#endregion

		#region SQL注入检测
		private static readonly System.Text.RegularExpressions.Regex regSystemThreats =
				new System.Text.RegularExpressions.Regex(@"\s?;\s?|\s?drop\s|\s?grant\s|^'|\s?--|\s?union\s|\s?delete\s|\s?truncate\s|\s?sysobjects\s?|\s?xp_.*?|\s?syslogins\s?|\s?sysremote\s?|\s?sysusers\s?|\s?sysxlogins\s?|\s?sysdatabases\s?|\s?aspnet_.*?|\s?exec\s?",
										System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

		public static bool DetectSqlInjection(string whereClause)
		{
			return regSystemThreats.IsMatch(whereClause);
		}

		public static bool DetectSqlInjection(string whereClause, string orderBy)
		{
			return regSystemThreats.IsMatch(whereClause) || regSystemThreats.IsMatch(orderBy);
		}
		#endregion

		#region 排序串规范化
		/// <summary>
		/// 如果指定了SortExpression，则以其规范化的结果排序
		/// 否则以表的第一个列排序
		/// </summary>
		/// <param name="columnEnum"></param>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		public static String ParseSortExpression(Type columnEnum, String sortExpression)
		{
			String sort = String.Empty;

			if (!String.IsNullOrEmpty(sortExpression)) {
				String[] expressions = sortExpression.Split(',');
				String[] pair;
				StringBuilder sb = new StringBuilder();
				String column, name;
				Enum col;
				foreach (String orderBy in expressions) {
					pair = orderBy.Trim().Split(' ');
					if (pair.Length == 1 || pair.Length == 2) {
						column = pair[0];
						try {
							col = (Enum)Enum.Parse(columnEnum, column, true);
							name = EntityHelper.GetEnumTextValue(col);
							if (sb.Length > 0) {
								sb.Append(", ");
							}
							sb.AppendFormat("[{0}]", name);
							if (pair.Length > 1 && SqlUtil.DESC.Equals(pair[1], StringComparison.OrdinalIgnoreCase)) {
								sb.AppendFormat(" {0}", SqlUtil.DESC);
							}
						}
						catch (Exception) { /* 忽略 */ }
					}
				}
				sort = sb.ToString();
			}
			if (String.IsNullOrEmpty(sort)) {
				sort = String.Format("[{0}]", EntityHelper.GetEnumTextValue((Enum)Enum.Parse(columnEnum, Enum.GetName(columnEnum, 0), true)));
			}
			return sort;
		}
		#endregion 排序串规范化

		#region ExecuteReader
		public static IDataReader ExecuteReader(TransactionManager transactionManager, DbCommand dbCommand)
		{
			if (!transactionManager.IsOpen) throw new DataException("Transaction must be open before executing a query.");
			IDataReader results = null;
			try {
				results = transactionManager.Database.ExecuteReader(dbCommand, transactionManager.TransactionObject);
			}
			catch (Exception) {
				throw;
			}
			return results;
		}

		public static IDataReader ExecuteReader(Database database, DbCommand dbCommand)
		{
			IDataReader results = null;
			try {
				results = database.ExecuteReader(dbCommand);
			}
			catch (Exception) {
				throw;
			}
			return results;
		}

		#endregion

		#region ExecuteNonQuery
		public static int ExecuteNonQuery(TransactionManager transactionManager, DbCommand dbCommand)
		{
			if (!transactionManager.IsOpen) throw new DataException("Transaction must be open before executing a query.");
			int results = 0;
			try {
				results = transactionManager.Database.ExecuteNonQuery(dbCommand, transactionManager.TransactionObject);
			}
			catch (Exception /*ex*/) {
				throw;
			}
			return results;
		}

		public static int ExecuteNonQuery(Database database, DbCommand dbCommand)
		{
			int results = 0;
			try {
				results = database.ExecuteNonQuery(dbCommand);
			}
			catch (Exception /*ex*/) {
				throw;
			}
			return results;
		}
		#endregion

		#region ExecuteDataSet
		public static DataSet ExecuteDataSet(TransactionManager transactionManager, DbCommand dbCommand)
		{
			if (!transactionManager.IsOpen) throw new DataException("Transaction must be open before executing a query.");
			DataSet results = null;
			try {
				results = transactionManager.Database.ExecuteDataSet(dbCommand, transactionManager.TransactionObject);
			}
			catch (Exception /*ex*/) {
				throw;
			}
			return results;
		}

		public static DataSet ExecuteDataSet(Database database, DbCommand dbCommand)
		{
			DataSet results = null;
			try {
				results = database.ExecuteDataSet(dbCommand);
			}
			catch (Exception /*ex*/) {
				throw;
			}
			return results;
		}
		#endregion

		#region ExecuteScalar
		public static object ExecuteScalar(TransactionManager transactionManager, DbCommand dbCommand)
		{
			if (!transactionManager.IsOpen) throw new DataException("Transaction must be open before executing a query.");
			Object results = null;
			try {
				results = transactionManager.Database.ExecuteScalar(dbCommand, transactionManager.TransactionObject);
			}
			catch (Exception /*ex*/) {
				throw;
			}
			return results;
		}

		public static object ExecuteScalar(Database database, DbCommand dbCommand)
		{
			Object results = null;
			try {
				results = database.ExecuteScalar(dbCommand);
			}
			catch (Exception /*ex*/) {
				throw;
			}
			return results;
		}
		#endregion

	}
}