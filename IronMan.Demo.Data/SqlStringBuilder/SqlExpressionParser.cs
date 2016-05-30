/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Text;

namespace IronMan.Demo.Data
{
  public class SqlExpressionParser : ExpressionParserBase
	{
		#region 申明
		private StringBuilder sql;
		#endregion 申明

		#region 构造函数
		public SqlExpressionParser(String propertyName)
			: this(propertyName, SqlComparisonType.Contains, false)
		{
		}
		public SqlExpressionParser(String propertyName, bool ignoreCase)
			: this(propertyName, SqlComparisonType.Contains, ignoreCase)
		{
		}
		public SqlExpressionParser(String propertyName, SqlComparisonType comparisonType)
			: this(propertyName, comparisonType, false)
		{
		}
		public SqlExpressionParser(String propertyName, SqlComparisonType comparisonType, bool ignoreCase)
			: base(propertyName, comparisonType, ignoreCase)
		{
		}
		#endregion

		#region ExpressionParserBase 成员override
		protected override void AppendOr()
		{
			sql.AppendFormat(" {0} ", SqlUtil.OR);
		}
		protected override void AppendAnd()
		{
			sql.AppendFormat(" {0} ", SqlUtil.AND);
		}
		protected override void AppendSpace()
		{
			sql.Append(" ");
		}
		protected override void OpenGrouping()
		{
			sql.Append(SqlUtil.LEFT);
		}
		protected override void CloseGrouping()
		{
			sql.Append(SqlUtil.RIGHT);
		}
		protected override void AppendSearchText(string searchText)
		{
			sql.Append(WrapWithSQL(PropertyName, searchText, IgnoreCase));
		}
		#endregion ExpressionParserBase 成员override

		#region Protected 方法
		protected virtual String WrapWithSQL(String propertyName, String value, bool ignoreCase)
		{
			SqlComparisonType compare = ComparisonType;
			String sql = String.Empty;
			if (String.IsNullOrEmpty(value)) {
				return sql;
			} else if (value.Equals(SqlUtil.STAR)) {
				compare = SqlComparisonType.Like;
				value = SqlUtil.WILD;
			} else if (value.StartsWith(SqlUtil.STAR) && value.EndsWith(SqlUtil.STAR)) {
				compare = SqlComparisonType.Contains;
				value = value.Substring(1, value.Length - 2);
			} else if (value.EndsWith(SqlUtil.STAR)) {
				compare = SqlComparisonType.StartsWith;
				value = value.Substring(0, value.Length - 1);
			} else if (value.StartsWith(SqlUtil.STAR)) {
				compare = SqlComparisonType.EndsWith;
				value = value.Substring(1, value.Length - 1);
			} else {
				compare = SqlComparisonType.Equals;
			}
			if (value.IndexOf(SqlUtil.STAR) > -1) {
				value = value.Replace(SqlUtil.STAR, SqlUtil.WILD);
				if (compare == SqlComparisonType.Equals) {
					compare = SqlComparisonType.Like;
				}
			}
			if (compare == SqlComparisonType.Equals && value.IndexOf(SqlUtil.WILD) > -1) {
				compare = SqlComparisonType.Like;
			}
			switch (compare) {
				case SqlComparisonType.Contains:
					sql = Contains(propertyName, value, ignoreCase);
					break;
				case SqlComparisonType.StartsWith:
					sql = StartsWith(propertyName, value, ignoreCase);
					break;
				case SqlComparisonType.EndsWith:
					sql = EndsWith(propertyName, value, ignoreCase);
					break;
				case SqlComparisonType.Like:
					sql = Like(propertyName, value, ignoreCase);
					break;
				default:
					sql = Equals(propertyName, value, ignoreCase);
					break;
			}
			return sql;
		}

		protected virtual String Contains(String column, String value, bool ignoreCase)
		{
			return SqlUtil.Contains(column, value, ignoreCase);
		}

		protected virtual String StartsWith(String column, String value, bool ignoreCase)
		{
			return SqlUtil.StartsWith(column, value, ignoreCase);
		}

		protected virtual String EndsWith(String column, String value, bool ignoreCase)
		{
			return SqlUtil.EndsWith(column, value, ignoreCase);
		}

		protected virtual String Like(String column, String value, bool ignoreCase)
		{
			return SqlUtil.Like(column, value, ignoreCase);
		}

		protected virtual String Equals(String column, String value, bool ignoreCase)
		{
			return SqlUtil.Equals(column, value, ignoreCase);
		}

		#endregion

		#region Parse 方法
		public virtual String Parse(String value)
		{
			sql = new StringBuilder();
			ParseCore(value);
			return sql.ToString();
		}
		#endregion Parse 方法
	}
}