/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IronMan.Demo.Data;
namespace IronMan.Demo.Data.SqlClient
{
  internal class SqlCommandHelper
	{
		/// <summary>
		/// 将某个表对应的分页查询转变为DBCommand对象
		/// </summary>
		/// <param name="database"></param>
		/// <param name="queryFormat">分页的查询串，注{0}为where clause,{1}为Order By,{2}为start,{3}为start+pageLength</param>
		/// <param name="columnEnum"></param>
		/// <param name="parameters">如果本参数不为空，将自动构建where语句</param>
		/// <param name="timeOut"></param>
		/// <param name="orderBy">排序列，如果为空，则以表的第一个列为排序参考列</param>
		/// <param name="start"></param>
		/// <param name="pageLength"></param>
		/// <returns></returns>
		public static DbCommand GetCommandWrapper(Database database, String queryFormat, Type columnEnum, SqlFilterParameterCollection parameters, int timeOut,String orderBy, int start, int pageLength)
		{
			//query = query.Replace(SqlUtil.PAGE_INDEX, string.Concat(SqlUtil.PAGE_INDEX, Guid.NewGuid().ToString("N").Substring(0,8)));
			String sortExpression = Utility.ParseSortExpression(columnEnum, orderBy);
			String whereClause = String.Empty;
			if (parameters != null && !String.IsNullOrEmpty(parameters.FilterExpression)) {
				whereClause = String.Format("where {0}", parameters.FilterExpression);
			}
			// 格式化
			queryFormat = String.Format(queryFormat, whereClause, sortExpression, start, (start + pageLength));
			DbCommand command = database.GetSqlStringCommand(queryFormat);
      if (parameters != null) {
				SqlFilterParameter param;
				for (int i = 0; i < parameters.Count; i++) {
					param = parameters[i];
					database.AddInParameter(command, param.Name, param.DbType, param.GetValue());
				}
			}
			command.CommandTimeout = timeOut;
			return command;
		}
	}
}