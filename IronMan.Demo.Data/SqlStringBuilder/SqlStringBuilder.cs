/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Text;

namespace IronMan.Demo.Data
{
  public class SqlStringBuilder
	{
		#region 变量申明
		private StringBuilder sql = new StringBuilder();
		private int _groupCount = 0;
		#endregion 

		#region 属性
		private String junction;
		public virtual String Junction
		{
			get { return junction; }
			set { junction = value; }
		}
		private bool ignoreCase;
		public virtual bool IgnoreCase
		{
			get { return ignoreCase; }
			set { ignoreCase = value; }
		}
		public virtual int Length
		{
			get { return sql.Length; }
			set { sql.Length = value; }
		}
		#endregion 属性

		#region 构造函数
		public SqlStringBuilder()
		{
			this.junction = SqlUtil.AND;
			this.ignoreCase = false;
		}
		public SqlStringBuilder(bool ignoreCase)
		{
			this.junction = SqlUtil.AND;
			this.ignoreCase = ignoreCase;
		}
		public SqlStringBuilder(bool ignoreCase, bool useAnd)
		{
			this.junction = useAnd ? SqlUtil.AND : SqlUtil.OR;
			this.ignoreCase = ignoreCase;
		}
		#endregion

		#region 方法
		public virtual void Clear()
		{
			sql.Length = 0;
			_groupCount = 0;
		}
		public override string ToString()
		{
			return sql.ToString().TrimEnd();
		}
		public virtual string ToString(string junction)
		{
			if (sql.Length > 0) {
				return new StringBuilder(" ").Append(junction).Append(" ").Append(ToString()).ToString();
			}
			return String.Empty;
		}
		public virtual string Parse(string column, string searchText, bool ignoreCase)
		{
			return new SqlExpressionParser(column, ignoreCase).Parse(searchText);
		}
		public virtual String GetInQueryValues(string values, bool encode)
		{
			if (encode) {
				String[] split = values.Split(',');
				values = SqlUtil.Encode(split, encode);
			}
			return values;
		}
		public virtual String GetInQueryValues(string[] values, bool encode)
		{
			string inQuery = string.Empty;
			inQuery = SqlUtil.Encode(values, encode);
			return inQuery;
		}
		public virtual void BeginGroup()
		{
			BeginGroup(Junction);
		}
		public virtual void BeginGroup(string junction)
		{
			if (sql.Length > 0) {
				sql.AppendFormat("{0} (", junction);
			} else {
				sql.AppendFormat("(", junction);
			}
			_groupCount++;
		}
		public virtual void EndGroup()
		{
			if (_groupCount > 0) {
				sql.Append(")");
				_groupCount--;
			}
		}
		internal virtual void EnsureGroups()
		{
			/*while (_groupCount > 0)
			{
				 EndGroup();
			}*/
		}
		#endregion 方法

		#region Append
		public virtual SqlStringBuilder Append(string column, string searchText)
		{
			return Append(this.junction, column, searchText, this.ignoreCase);
		}
		public virtual SqlStringBuilder Append(string column, string searchText, bool ignoreCase)
		{
			return Append(this.junction, column, searchText, ignoreCase);
		}
		public virtual SqlStringBuilder Append(string junction, string column, string searchText, bool ignoreCase)
		{
			if (!string.IsNullOrEmpty(searchText)) {
				AppendInternal(junction, Parse(column, searchText, ignoreCase));
			}
			return this;
		}
		#endregion Append

		#region AppendEquals
		public virtual SqlStringBuilder AppendEquals(string column, string value)
		{
			return AppendEquals(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendEquals(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, column, "=", SqlUtil.Encode(value, true));
			}
			return this;
		}
		#endregion AppendEquals

		#region AppendNotEquals
		public virtual SqlStringBuilder AppendNotEquals(string column, string value)
		{
			return AppendNotEquals(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendNotEquals(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, column, "<>", SqlUtil.Encode(value, true));
			}
			return this;
		}
		#endregion AppendNotEquals

		#region AppendIn
		public virtual SqlStringBuilder AppendIn(string column, string[] values)
		{
			return AppendIn(this.junction, column, values, true);
		}
		public virtual SqlStringBuilder AppendIn(string column, string[] values, bool encode)
		{
			return AppendIn(this.junction, column, values, encode);
		}
		public virtual SqlStringBuilder AppendIn(string junction, string column, string[] values)
		{
			return AppendIn(junction, column, values, true);
		}
		public virtual SqlStringBuilder AppendIn(string junction, string column, string[] values, bool encode)
		{
			if (values != null && values.Length > 0) {
				string sqlString = GetInQueryValues(values, encode);
				if (!string.IsNullOrEmpty(sqlString)) {
					AppendInQuery(junction, column, sqlString);
				}
			}
			return this;
		}
		#endregion AppendIn

		#region AppendNotIn
		public virtual SqlStringBuilder AppendNotIn(string column, string[] values)
		{
			return AppendNotIn(this.junction, column, values, true);
		}
		public virtual SqlStringBuilder AppendNotIn(string column, string[] values, bool encode)
		{
			return AppendNotIn(this.junction, column, values, encode);
		}
		public virtual SqlStringBuilder AppendNotIn(string junction, string column, string[] values)
		{
			return AppendNotIn(junction, column, values, true);
		}
		public virtual SqlStringBuilder AppendNotIn(string junction, string column, string[] values, bool encode)
		{
			if (values != null && values.Length > 0) {
				string sqlString = GetInQueryValues(values, encode);
				if (!string.IsNullOrEmpty(sqlString)) {
					AppendNotInQuery(junction, column, sqlString);
				}
			}
			return this;
		}
		#endregion AppendNotIn

		#region AppendInQuery
		public virtual SqlStringBuilder AppendInQuery(string column, string query)
		{
			return AppendInQuery(this.junction, column, query);
		}
		public virtual SqlStringBuilder AppendInQuery(string junction, string column, string query)
		{
			if (!string.IsNullOrEmpty(query)) {
				AppendInternal(junction, column, "IN", "(" + query + ")");
			}
			return this;
		}
		#endregion AppendInQuery

		#region AppendNotInQuery
		public virtual SqlStringBuilder AppendNotInQuery(string column, string query)
		{
			return AppendNotInQuery(this.junction, column, query);
		}
		public virtual SqlStringBuilder AppendNotInQuery(string junction, string column, string query)
		{
			if (!string.IsNullOrEmpty(query)) {
				AppendInternal(junction, column, "NOT IN", "(" + query + ")");
			}
			return this;
		}
		#endregion AppendNotInQuery

		#region AppendRange
		public virtual SqlStringBuilder AppendRange(string column, string from, string to)
		{
			return AppendRange(this.junction, column, from, to);
		}
		public virtual SqlStringBuilder AppendRange(string junction, string column, string from, string to)
		{
			if (!string.IsNullOrEmpty(from) || !string.IsNullOrEmpty(to)) {
				StringBuilder sb = new StringBuilder();
				if (!string.IsNullOrEmpty(from)) {
					sb.AppendFormat("{0} >= {1}", column, SqlUtil.Encode(from, true));
				}
				if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to)) {
					sb.AppendFormat(" {0} ", SqlUtil.AND);
				}
				if (!string.IsNullOrEmpty(to)) {
					sb.AppendFormat("{0} <= {1}", column, SqlUtil.Encode(to, true));
				}
				AppendInternal(junction, sb.ToString());
			}
			return this;
		}
		#endregion AppendRange

		#region AppendNotRange
		public virtual SqlStringBuilder AppendNotRange(string column, string from, string to)
		{
			return AppendNotRange(this.junction, column, from, to);
		}
		public virtual SqlStringBuilder AppendNotRange(string junction, string column, string from, string to)
		{
			if (!string.IsNullOrEmpty(from) || !string.IsNullOrEmpty(to)) {
				StringBuilder sb = new StringBuilder();
				sb.Append("NOT (");
				if (!string.IsNullOrEmpty(from)) {
					sb.AppendFormat("{0} >= {1}", column, SqlUtil.Encode(from, true));
				}
				if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to)) {
					sb.AppendFormat(" {0} ", SqlUtil.AND);
				}
				if (!string.IsNullOrEmpty(to)) {
					sb.AppendFormat("{0} <= {1}", column, SqlUtil.Encode(to, true));
				}
				sb.Append(")");
				AppendInternal(junction, sb.ToString());
			}
			return this;
		}
		#endregion AppendNotRange

		#region AppendIsNull
		public virtual SqlStringBuilder AppendIsNull(string column)
		{
			return AppendIsNull(this.junction, column);
		}
		public virtual SqlStringBuilder AppendIsNull(string junction, string column)
		{
			AppendInternal(junction, SqlUtil.IsNull(column));
			return this;
		}
		#endregion AppendIsNull

		#region AppendIsNotNull
		public virtual SqlStringBuilder AppendIsNotNull(string column)
		{
			return AppendIsNotNull(this.junction, column);
		}
		public virtual SqlStringBuilder AppendIsNotNull(string junction, string column)
		{
			AppendInternal(junction, SqlUtil.IsNotNull(column));
			return this;
		}
		#endregion AppendIsNotNull

		#region AppendGreaterThan
		public virtual SqlStringBuilder AppendGreaterThan(string column, string value)
		{
			return AppendGreaterThan(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendGreaterThan(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, column, ">", SqlUtil.Encode(value, true));
			}
			return this;
		}
		#endregion AppendGreaterThan

		#region AppendGreaterThanOrEqual
		public virtual SqlStringBuilder AppendGreaterThanOrEqual(string column, string value)
		{
			return AppendGreaterThanOrEqual(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendGreaterThanOrEqual(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, column, ">=", SqlUtil.Encode(value, true));
			}
			return this;
		}
		#endregion AppendGreaterThanOrEqual

		#region AppendLessThan
		public virtual SqlStringBuilder AppendLessThan(string column, string value)
		{
			return AppendLessThan(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendLessThan(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, column, "<", SqlUtil.Encode(value, true));
			}
			return this;
		}
		#endregion AppendLessThan

		#region AppendLessThanOrEqual
		public virtual SqlStringBuilder AppendLessThanOrEqual(string column, string value)
		{
			return AppendLessThanOrEqual(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendLessThanOrEqual(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, column, "<=", SqlUtil.Encode(value, true));
			}
			return this;
		}
		#endregion AppendLessThanOrEqual

		#region AppendStartsWith
		public virtual SqlStringBuilder AppendStartsWith(string column, string value)
		{
			return AppendStartsWith(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendStartsWith(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, SqlUtil.StartsWith(column, value));
			}
			return this;
		}
		#endregion AppendStartsWith

		#region AppendEndsWith
		public virtual SqlStringBuilder AppendEndsWith(string column, string value)
		{
			return AppendEndsWith(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendEndsWith(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, SqlUtil.EndsWith(column, value));
			}
			return this;
		}
		#endregion AppendEndsWith

		#region AppendContains
		public virtual SqlStringBuilder AppendContains(string column, string value)
		{
			return AppendContains(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendContains(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, SqlUtil.Contains(column, value));
			}
			return this;
		}
		#endregion AppendContains

		#region AppendNotContains
		public virtual SqlStringBuilder AppendNotContains(string column, string value)
		{
			return AppendNotContains(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendNotContains(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, SqlUtil.Contains(column, value));
			}
			return this;
		}
		#endregion AppendNotContains

		#region AppendLike
		public virtual SqlStringBuilder AppendLike(string column, string value)
		{
			return AppendLike(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendLike(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, SqlUtil.Like(column, value));
			}
			return this;
		}
		#endregion AppendLike

		#region AppendNotLike
		public virtual SqlStringBuilder AppendNotLike(string column, string value)
		{
			return AppendNotLike(this.junction, column, value);
		}
		public virtual SqlStringBuilder AppendNotLike(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				AppendInternal(junction, SqlUtil.NotLike(column, value));
			}
			return this;
		}
		#endregion

		#region AppendInternal
		protected virtual void AppendInternal(string junction, string column, string compare, string value)
		{
			AppendInternal(junction, string.Format("{0} {1} {2}", column, compare, value));
		}
		protected virtual void AppendInternal(string junction, string query)
		{
			if (!string.IsNullOrEmpty(query)) {
#if DEBUG
				String end = System.Environment.NewLine;
#else
				String end = String.Empty;
#endif
				String format = (sql.Length > 0) ? " {0} ({1}){2}" : " ({1}){2}";
				sql.AppendFormat(format, junction, query, end);
			}
		}
		#endregion AppendInternal

	}
}