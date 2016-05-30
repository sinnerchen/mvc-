/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;

namespace IronMan.Demo.Data
{
  public class ParameterizedSqlExpressionParser : SqlExpressionParser
	{
		#region 构造函数
		public ParameterizedSqlExpressionParser(String propertyName)
			: this(propertyName, SqlComparisonType.Contains, false)
		{
		}
		public ParameterizedSqlExpressionParser(String propertyName, bool ignoreCase)
			: this(propertyName, SqlComparisonType.Contains, ignoreCase)
		{
		}
		public ParameterizedSqlExpressionParser(String propertyName, SqlComparisonType comparisonType)
			: this(propertyName, comparisonType, false)
		{
		}
		public ParameterizedSqlExpressionParser(String propertyName, SqlComparisonType comparisonType, bool ignoreCase)
			: base(propertyName, comparisonType, ignoreCase)
		{
		}
		#endregion

		#region SqlExpressionParser 成员override
		protected override string Contains(string column, string value, bool ignoreCase)
		{
			value = SqlUtil.Contains(value);
			return SqlUtil.Like(column, Parameters.GetParameter(value), ignoreCase, false);
		}
		protected override string StartsWith(string column, string value, bool ignoreCase)
		{
			value = SqlUtil.StartsWith(value);
			return SqlUtil.Like(column, Parameters.GetParameter(value), ignoreCase, false);
		}
		protected override string EndsWith(string column, string value, bool ignoreCase)
		{
			value = SqlUtil.EndsWith(value);
			return SqlUtil.Like(column, Parameters.GetParameter(value), ignoreCase, false);
		}
		protected override string Like(string column, string value, bool ignoreCase)
		{
			value = SqlUtil.Like(value);
			return SqlUtil.Like(column, Parameters.GetParameter(value), ignoreCase, false);
		}
		protected override string Equals(string column, string value, bool ignoreCase)
		{
			value = SqlUtil.Equals(value);
			return SqlUtil.Equals(column, Parameters.GetParameter(value), ignoreCase, false);
		}
		#endregion SqlExpressionParser 成员重写

		#region 属性
		private SqlFilterParameterCollection parameters;
		public virtual SqlFilterParameterCollection Parameters
		{
			get
			{
				if (parameters == null) {
					parameters = new SqlFilterParameterCollection();
				}
				return parameters;
			}
			set { parameters = value; }
		}
		#endregion 属性
	}
}