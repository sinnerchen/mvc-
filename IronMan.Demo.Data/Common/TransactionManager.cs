/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace  IronMan.Demo.Data
{
	public class TransactionManager : ITransactionManager, IDisposable
	{
		#region 私有成员
		private Database _database;
		private DbConnection _connection;
		private DbTransaction _transaction;
		private string _connectionString;
		private string _invariantProviderName;
		private bool _transactionOpen = false;
		private bool disposed;
		private static object syncRoot = new object();
		#endregion

		#region 属性区
		/// <summary>
		///	获取配置文件中的连接字符串
		/// </summary>
		/// <remark>在一个事务中不可更改这个串</remark>
		/// <exception cref="InvalidOperationException">
		///当在一个已打开的事务中更改连接字符串时会抛出异常.
		/// </exception>
		public string ConnectionString
		{
			get { return this._connectionString; }
			set
			{
				if (this.IsOpen) {
					throw new InvalidOperationException("Database cannot be changed during a transaction");
				}

				this._connectionString = value;
				if (this._connectionString.Length > 0 && this._invariantProviderName.Length > 0) {
					this._database = new GenericDatabase(_connectionString, DbProviderFactories.GetFactory(this._invariantProviderName));
					this._connection = this._database.CreateConnection();
				}
			}
		}

		/// <summary>
		/// 获取或设置相关的提供程序
		/// </summary>
		/// <value>提供程序名</value>
		public string InvariantProviderName
		{
			get { return this._invariantProviderName; }
			set
			{
				if (this.IsOpen) {
					throw new InvalidOperationException("Database cannot be changed during a transaction");
				}

				this._invariantProviderName = value;
				if (this._connectionString.Length > 0 && this._invariantProviderName.Length > 0) {
					this._database = new GenericDatabase(_connectionString, DbProviderFactories.GetFactory(this._invariantProviderName));
					this._connection = this._database.CreateConnection();
				}
			}
		}

		/// <summary>
		/// 获取数据库Database实例 <see cref="Database"/> .
		/// </summary>
		/// <value></value>
		public Database Database
		{
			get { return this._database; }
		}

		/// <summary>
		///	获取当前对象实例 <see cref="DbTransaction"/> .
		/// </summary>
		public DbTransaction TransactionObject
		{
			get { return this._transaction; }
		}

		/// <summary>
		///	获取当前事务是否处于开启或执行中
		/// </summary>
		/// <value></value>
		public bool IsOpen
		{
			get { return this._transactionOpen; }
		}
		#endregion Properties

		#region 构造函数
		/// <summary>
		///	构建一个新实例 <see cref="TransactionManager"/> .
		/// </summary>
		internal TransactionManager()
		{
		}

		/// <summary>
		///	从连接字符串构建一个新的实例 <see cref="TransactionManager"/> .
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		public TransactionManager(string connectionString)
			: this(connectionString, "System.Data.SqlClient")
		{
		}

		/// <summary>
		///	从连接字符串与提供程序获取一个事务管理对象实例<see cref="TransactionManager"/> .
		/// </summary>
		/// <param name="connectionString">数据库连接字符串.</param>
		/// <param name="providerInvariantName">提供程序名.</param>
		public TransactionManager(string connectionString, string providerInvariantName)
		{
			this._connectionString = connectionString;
			this._invariantProviderName = providerInvariantName;
			this._database = new GenericDatabase(_connectionString, DbProviderFactories.GetFactory(this._invariantProviderName));
			this._connection = this._database.CreateConnection();
		}
		#endregion Constructors

		#region 公用方法
		/// <summary>
		///	开启事务
		/// </summary>
		/// <remarks>默认的隔离级别 <see cref="IsolationLevel"/> 是ReadCommitted</remarks>
		/// <exception cref="InvalidOperationException">如果一个事务已打开,不可再设置</exception>
		public void BeginTransaction()
		{
			BeginTransaction(IsolationLevel.ReadCommitted);
		}

		/// <summary>
		///	开启一个事务
		/// </summary>
		/// <param name="isolationLevel"> <see cref="IsolationLevel"/>事务隔离级别</param>
		/// <exception cref="InvalidOperationException">如果事务已打开，不可设置</exception>
		/// <exception cref="DataException"></exception>
		/// <exception cref="DbException"></exception>
		public void BeginTransaction(IsolationLevel isolationLevel)
		{
			if (IsOpen) {
				throw new InvalidOperationException("Transaction already open.");
			}

			try {
				this._connection.Open();
				this._transaction = this._connection.BeginTransaction(isolationLevel);
				this._transactionOpen = true;
			}
			catch (Exception) {
				//出现错误时关闭连接，并销毁事务对象
				if (this._connection != null) {
					this._connection.Close();
				}

				if (this._transaction != null) {
					this._transaction.Dispose();
				}

				this._transactionOpen = false;
				throw;
			}
		}

		/// <summary>
		/// 提交事务更改
		/// </summary>
		/// <exception cref="InvalidOperationException">如果事务没有打开，则会异常</exception>
		public void Commit()
		{
			if (!this.IsOpen) {
				throw new InvalidOperationException("Transaction needs to begin first.");
			}

			try {
				this._transaction.Commit(); // SqlClient could throw Exception or InvalidOperationException
			}
			finally {
				//假定事务已成功执行
				this._connection.Close();
				this._transaction.Dispose();
				this._transactionOpen = false;
			}
		}

		/// <summary>
		///	回滚事务
		/// </summary>
		/// <exception cref="InvalidOperationException">如果事务没有处于打开状态，不可回滚</exception>
		public void Rollback()
		{
			if (!this.IsOpen) {
				throw new InvalidOperationException("Transaction needs to begin first.");
			}

			try {
				this._transaction.Rollback(); // SqlClient could throw Exception or InvalidOperationException
			}
			finally {
				this._connection.Close();
				this._transaction.Dispose();
				this._transactionOpen = false;
			}
		}
		#endregion 公有方法

		#region IDisposable 接口
		/// <summary>
		/// 销毁事务对象
		/// </summary>
		public void Dispose()
		{
			if (!disposed) {
				lock (syncRoot) {
					disposed = true;

					if (this.IsOpen) {
						this.Rollback();
					}
				}
			}
		}
		#endregion
	}
}