/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace IronMan.Demo.Data
{
	/// <summary>
	/// 事务管理类
	/// </summary>
	public interface ITransactionManager
	{
		/// <summary>
		/// 开启事务
		/// </summary>
		void BeginTransaction();

		/// <summary>
		/// 开启事务
		/// </summary>
		/// <param name="isolationLevel">隔离级别</param>
		void BeginTransaction(IsolationLevel isolationLevel);

		/// <summary>
		/// 提交实例
		/// </summary>
		void Commit();

		/// <summary>
		/// 获取或设置连接字符串
		/// </summary>
		/// <value>连接字符串</value>
		string ConnectionString { get; set; }

		/// <summary>
		/// 获取数据库
		/// </summary>
		/// <value>The database.</value>
		Database Database { get; }

		/// <summary>
		/// 施放实例
		/// </summary>
		void Dispose();

		/// <summary>
		/// 获取或设置相关的稳定的提供程序
		/// </summary>
		/// <value>稳定的provider名.</value>
		string InvariantProviderName { get; set; }

		/// <summary>
		/// 事务是否开启标志读取
		/// </summary>
		/// <value><c>true</c>已开启, <c>false</c>,未开启.</value>
		bool IsOpen { get; }

		/// <summary>
		/// 事务回滚
		/// </summary>
		void Rollback();

		/// <summary>
		/// 获取事务实例
		/// </summary>
		/// <value>事务实例.</value>
		DbTransaction TransactionObject { get; }
	}
}