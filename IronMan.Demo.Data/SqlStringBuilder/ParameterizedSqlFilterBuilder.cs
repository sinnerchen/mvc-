/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Text;
using System.Configuration;

namespace IronMan.Demo.Data
{
  public class ParameterizedSqlFilterBuilder<EntityColumn> : SqlFilterBuilder<EntityColumn>, IFilterParameterCollection
	{
		private bool _isDirty = true;

		#region 构造函数
		public ParameterizedSqlFilterBuilder() : base() { }
		public ParameterizedSqlFilterBuilder(bool ignoreCase) : base(ignoreCase) { }
		public ParameterizedSqlFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }
		#endregion 构造函数

		#region 方法
		public override string Parse(string column, string searchText, bool ignoreCase)
		{
			ParameterizedSqlExpressionParser parser = new ParameterizedSqlExpressionParser(column, ignoreCase);
			parser.Parameters = this.Parameters;
			return parser.Parse(searchText);
		}
		public override String GetInQueryValues(string values, bool encode)
		{
			CommaDelimitedStringCollection csv = new CommaDelimitedStringCollection();
			String[] split = values.Split(',');
			String temp;
			foreach (string value in split) {
				temp = value.Trim();
				if (!string.IsNullOrEmpty(temp)) {
					csv.Add(Parameters.GetParameter(temp));
				}
			}
			return csv.ToString();
		}
		public virtual SqlFilterParameterCollection GetParameters()
		{
			EnsureGroups();
			_isDirty = false;
			Parameters.FilterExpression = this.ToString();
			return Parameters;
		}
		#endregion 方法

		#region 属性
		private SqlFilterParameterCollection parameters;
		public virtual SqlFilterParameterCollection Parameters
		{
			get
			{
				if (parameters == null) {
					parameters = new SqlFilterParameterCollection();
				}
				if (_isDirty)
					GetParameters();
				return parameters;
			}
			set { parameters = value; }
		}
		#endregion 属性

		#region Append
		public override SqlFilterBuilder<EntityColumn> Append(string junction, EntityColumn column, string searchText, bool ignoreCase)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.Append(junction, column, searchText, ignoreCase);
		}
		#endregion Append

		#region AppendEquals
		public override SqlFilterBuilder<EntityColumn> AppendEquals(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendEquals(junction, column, value);
		}
		public override SqlStringBuilder AppendEquals(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.Equals(value);
				AppendInternal(junction, column, "=", Parameters.GetParameter(value));
			} else {
				AppendIsNull(junction, column);
			}
			return this;
		}

		#endregion AppendEquals

		#region AppendNotEquals
		public override SqlFilterBuilder<EntityColumn> AppendNotEquals(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendNotEquals(junction, column, value);
		}
		public override SqlStringBuilder AppendNotEquals(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.Equals(value);
				AppendInternal(junction, column, "<>", Parameters.GetParameter(value));
			}else {
				AppendIsNotNull(junction, column);
			}
			return this;
		}
		#endregion AppendNotEquals

		#region AppendIn
		public override SqlFilterBuilder<EntityColumn> AppendIn(string junction, EntityColumn column, string[] values, bool encode)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendIn(junction, column, values, encode);
		}
		public override SqlStringBuilder AppendIn(string junction, string column, string[] values, bool encode)
		{
			if (values != null && values.Length > 0) {
				string sqlQuery = GetInQueryValues(values, encode);

				if (!string.IsNullOrEmpty(sqlQuery)) {
					_isDirty = true;
					AppendInQuery(junction, column, sqlQuery);
				}
			}
			return this;
		}
		#endregion AppendIn

		#region AppendNotIn
		public override SqlFilterBuilder<EntityColumn> AppendNotIn(string junction, EntityColumn column, string[] values, bool encode)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendNotIn(junction, column, values, encode);
		}
		public override SqlStringBuilder AppendNotIn(string junction, string column, string[] values, bool encode)
		{
			if (values != null && values.Length > 0) {
				string sqlQuery = GetInQueryValues(values, encode);
				if (!string.IsNullOrEmpty(sqlQuery)) {
					_isDirty = true;
					AppendNotInQuery(junction, column, sqlQuery);
				}
			}
			return this;
		}
		#endregion AppendNotIn

		#region AppendInQuery
		public override SqlFilterBuilder<EntityColumn> AppendInQuery(string junction, EntityColumn column, string query)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendInQuery(junction, column, query);
		}
		#endregion AppendInQuery

		#region AppendNotInQuery
		public override SqlFilterBuilder<EntityColumn> AppendNotInQuery(string junction, EntityColumn column, string query)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendNotInQuery(junction, column, query);
		}
		#endregion AppendNotInQuery

		#region AppendRange
		public override SqlFilterBuilder<EntityColumn> AppendRange(string junction, EntityColumn column, string from, string to)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendRange(junction, column, from, to);
		}
		public override SqlStringBuilder AppendRange(string junction, string column, string from, string to)
		{
			if (!string.IsNullOrEmpty(from) || !string.IsNullOrEmpty(to)) {
				StringBuilder sb = new StringBuilder();
				if (!string.IsNullOrEmpty(from)) {
					sb.AppendFormat("{0} >= {1}", column, Parameters.GetParameter(from));
				}
				if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to)) {
					sb.AppendFormat(" {0} ", SqlUtil.AND);
				}
				if (!string.IsNullOrEmpty(to)) {
					sb.AppendFormat("{0} <= {1}", column, Parameters.GetParameter(to));
				}
				_isDirty = true;
				AppendInternal(junction, sb.ToString());
			}
			return this;
		}
		#endregion AppendRange

		#region AppendNotRange
		public override SqlFilterBuilder<EntityColumn> AppendNotRange(string junction, EntityColumn column, string from, string to)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendNotRange(junction, column, from, to);
		}
		public override SqlStringBuilder AppendNotRange(string junction, string column, string from, string to)
		{
			if (!string.IsNullOrEmpty(from) || !string.IsNullOrEmpty(to)) {
				StringBuilder sb = new StringBuilder();
				sb.Append("NOT (");
				if (!string.IsNullOrEmpty(from)) {
					sb.AppendFormat("{0} >= {1}", column, Parameters.GetParameter(from));
				}
				if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to)) {
					sb.AppendFormat(" {0} ", SqlUtil.AND);
				}
				if (!string.IsNullOrEmpty(to)) {
					sb.AppendFormat("{0} <= {1}", column, Parameters.GetParameter(to));
				}
				sb.Append(")");
				_isDirty = true;
				AppendInternal(junction, sb.ToString());
			}
			return this;
		}
		#endregion AppendNotRange

		#region AppendGreaterThan
		public override SqlFilterBuilder<EntityColumn> AppendGreaterThan(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendGreaterThan(junction, column, value);
		}
		public override SqlStringBuilder AppendGreaterThan(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.Equals(value);
				AppendInternal(junction, column, ">", Parameters.GetParameter(value));
			}
			return this;
		}
		#endregion AppendGreaterThan

		#region AppendGreaterThanOrEqual
		public override SqlFilterBuilder<EntityColumn> AppendGreaterThanOrEqual(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendGreaterThanOrEqual(junction, column, value);
		}
		public override SqlStringBuilder AppendGreaterThanOrEqual(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.Equals(value);
				AppendInternal(junction, column, ">=", Parameters.GetParameter(value));
			}
			return this;
		}
		#endregion AppendGreaterThanOrEqual

		#region AppendLessThan
		public override SqlFilterBuilder<EntityColumn> AppendLessThan(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendLessThan(junction, column, value);
		}
		public override SqlStringBuilder AppendLessThan(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.Equals(value);
				AppendInternal(junction, column, "<", Parameters.GetParameter(value));
			}
			return this;
		}
		#endregion AppendLessThan

		#region AppendLessThanOrEqual
		public override SqlFilterBuilder<EntityColumn> AppendLessThanOrEqual(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendLessThanOrEqual(junction, column, value);
		}
		public override SqlStringBuilder AppendLessThanOrEqual(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.Equals(value);
				AppendInternal(junction, column, "<=", Parameters.GetParameter(value));
			}
			return this;
		}
		#endregion AppendLessThanOrEqual

		#region AppendStartsWith
		public override SqlFilterBuilder<EntityColumn> AppendStartsWith(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendStartsWith(junction, column, value);
		}
		public override SqlStringBuilder AppendStartsWith(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.StartsWith(value);
				AppendInternal(junction, column, "LIKE", Parameters.GetParameter(value));
			}
			return this;
		}
		#endregion AppendStartsWith

		#region AppendEndsWith
		public override SqlFilterBuilder<EntityColumn> AppendEndsWith(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendEndsWith(junction, column, value);
		}
		public override SqlStringBuilder AppendEndsWith(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.EndsWith(value);
				AppendInternal(junction, column, "LIKE", Parameters.GetParameter(value));
			}
			return this;
		}
		#endregion AppendEndsWith

		#region AppendContains
		public override SqlFilterBuilder<EntityColumn> AppendContains(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendContains(junction, column, value);
		}
		public override SqlStringBuilder AppendContains(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.Contains(value);
				AppendInternal(junction, column, "LIKE", Parameters.GetParameter(value));
			}
			return this;
		}
		#endregion AppendContains

		#region AppendNotContains
		public override SqlFilterBuilder<EntityColumn> AppendNotContains(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendNotContains(junction, column, value);
		}
		public override SqlStringBuilder AppendNotContains(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.NotContains(value);
				AppendInternal(junction, column, "NOT LIKE", Parameters.GetParameter(value));
			}
			return this;
		}
		#endregion AppendNotContains

		#region AppendLike
		public override SqlFilterBuilder<EntityColumn> AppendLike(string junction, EntityColumn column, string value)
		{
			_isDirty = true;
			Parameters.SetCurrentColumn(column);
			return base.AppendLike(junction, column, value);
		}
		public override SqlStringBuilder AppendLike(string junction, string column, string value)
		{
			if (!string.IsNullOrEmpty(value)) {
				_isDirty = true;
				value = SqlUtil.Like(value);
				AppendInternal(junction, column, "LIKE", Parameters.GetParameter(value));
			}
			return this;
		}
		#endregion AppendLike

		#region IFilterParameterCollection
		SqlFilterParameterCollection IFilterParameterCollection.GetParameters()
		{
			return GetParameters();
		}
		#endregion
	}
}