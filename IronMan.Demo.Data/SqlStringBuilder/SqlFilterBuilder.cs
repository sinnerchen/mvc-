/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using IronMan.Demo.Entities;

namespace IronMan.Demo.Data
{
  public class SqlFilterBuilder<EntityColumn> : SqlStringBuilder
	{
		#region 构造函数
		public SqlFilterBuilder() : base() { }
		public SqlFilterBuilder(bool ignoreCase) : base(ignoreCase) { }
		public SqlFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }
		#endregion 构造函数

		#region 方法
		protected virtual String GetColumnName(EntityColumn column)
		{
			String name = EntityHelper.GetEnumTextValue(column as Enum);
			if (string.IsNullOrEmpty(name)) {
				name = column.ToString();
			}
			return string.Format("[{0}]",name);
		}
		#endregion 方法

		#region Append
		public virtual SqlFilterBuilder<EntityColumn> Append(EntityColumn column, string searchText)
		{
			return Append(this.Junction, column, searchText, this.IgnoreCase);
		}
		public virtual SqlFilterBuilder<EntityColumn> Append(EntityColumn column, string searchText, bool ignoreCase)
		{
			return Append(this.Junction, column, searchText, ignoreCase);
		}
		public virtual SqlFilterBuilder<EntityColumn> Append(string junction, EntityColumn column, string searchText, bool ignoreCase)
		{
			Append(junction, GetColumnName(column), searchText, ignoreCase);
			return this;
		}
		#endregion Append

		#region AppendEquals
		public virtual SqlFilterBuilder<EntityColumn> AppendEquals(EntityColumn column, string value)
		{
			return AppendEquals(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendEquals(string junction, EntityColumn column, string value)
		{
			AppendEquals(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendEquals

		#region AppendNotEquals
		public virtual SqlFilterBuilder<EntityColumn> AppendNotEquals(EntityColumn column, string value)
		{
			return AppendNotEquals(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendNotEquals(string junction, EntityColumn column, string value)
		{
			AppendNotEquals(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendNotEquals

		#region AppendIn
		public virtual SqlFilterBuilder<EntityColumn> AppendIn(EntityColumn column, string[] values)
		{
			return AppendIn(this.Junction, column, values, true);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendIn(EntityColumn column, string[] values, bool encode)
		{
			return AppendIn(this.Junction, column, values, encode);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendIn(string junction, EntityColumn column, string[] values)
		{
			return AppendIn(junction, column, values, true);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendIn(string junction, EntityColumn column, string[] values, bool encode)
		{
			AppendIn(junction, GetColumnName(column), values, encode);
			return this;
		}
		#endregion AppendIn

		#region AppendNotIn
		public virtual SqlFilterBuilder<EntityColumn> AppendNotIn(EntityColumn column, string[] values)
		{
			return AppendNotIn(this.Junction, column, values, true);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendNotIn(EntityColumn column, string[] values, bool encode)
		{
			return AppendNotIn(this.Junction, column, values, encode);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendNotIn(string junction, EntityColumn column, string[] values)
		{
			return AppendNotIn(junction, column, values, true);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendNotIn(string junction, EntityColumn column, string[] values, bool encode)
		{
			AppendNotIn(junction, GetColumnName(column), values, encode);
			return this;
		}
		#endregion AppendIn

		#region AppendInQuery
		public virtual SqlFilterBuilder<EntityColumn> AppendInQuery(EntityColumn column, string query)
		{
			return AppendInQuery(this.Junction, column, query);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendInQuery(string junction, EntityColumn column, string query)
		{
			AppendInQuery(junction, GetColumnName(column), query);
			return this;
		}
		#endregion AppendInQuery

		#region AppendNotInQuery
		public virtual SqlFilterBuilder<EntityColumn> AppendNotInQuery(EntityColumn column, string query)
		{
			return AppendNotInQuery(this.Junction, column, query);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendNotInQuery(string junction, EntityColumn column, string query)
		{
			AppendNotInQuery(junction, GetColumnName(column), query);
			return this;
		}
		#endregion AppendNotInQuery

		#region AppendIsNull
		public virtual SqlFilterBuilder<EntityColumn> AppendIsNull(EntityColumn column)
		{
			return AppendIsNull(this.Junction, column);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendIsNull(string junction, EntityColumn column)
		{
			AppendIsNull(junction, GetColumnName(column));
			return this;
		}
		#endregion AppendIsNull

		#region AppendIsNotNull
		public virtual SqlFilterBuilder<EntityColumn> AppendIsNotNull(EntityColumn column)
		{
			return AppendIsNotNull(this.Junction, column);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendIsNotNull(string junction, EntityColumn column)
		{
			AppendIsNotNull(junction, GetColumnName(column));
			return this;
		}
		#endregion AppendIsNotNull

		#region AppendRange
		public virtual SqlFilterBuilder<EntityColumn> AppendRange(EntityColumn column, string from, string to)
		{
			return AppendRange(this.Junction, column, from, to);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendRange(string junction, EntityColumn column, string from, string to)
		{
			AppendRange(junction, GetColumnName(column), from, to);
			return this;
		}
		#endregion AppendRange

		#region AppendNotRange
		public virtual SqlFilterBuilder<EntityColumn> AppendNotRange(EntityColumn column, string from, string to)
		{
			return AppendNotRange(this.Junction, column, from, to);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendNotRange(string junction, EntityColumn column, string from, string to)
		{
			AppendNotRange(junction, GetColumnName(column), from, to);
			return this;
		}
		#endregion AppendNotRange

		#region AppendGreaterThan
		public virtual SqlFilterBuilder<EntityColumn> AppendGreaterThan(EntityColumn column, string value)
		{
			return AppendGreaterThan(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendGreaterThan(string junction, EntityColumn column, string value)
		{
			AppendGreaterThan(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendGreaterThan

		#region AppendGreaterThanOrEqual
		public virtual SqlFilterBuilder<EntityColumn> AppendGreaterThanOrEqual(EntityColumn column, string value)
		{
			return AppendGreaterThanOrEqual(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendGreaterThanOrEqual(string junction, EntityColumn column, string value)
		{
			AppendGreaterThanOrEqual(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendGreaterThanOrEqual

		#region AppendLessThan
		public virtual SqlFilterBuilder<EntityColumn> AppendLessThan(EntityColumn column, string value)
		{
			return AppendLessThan(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendLessThan(string junction, EntityColumn column, string value)
		{
			AppendLessThan(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendLessThan

		#region AppendLessThanOrEqual
		public virtual SqlFilterBuilder<EntityColumn> AppendLessThanOrEqual(EntityColumn column, string value)
		{
			return AppendLessThanOrEqual(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendLessThanOrEqual(string junction, EntityColumn column, string value)
		{
			AppendLessThanOrEqual(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendLessThanOrEqual

		#region AppendStartsWith
		public virtual SqlFilterBuilder<EntityColumn> AppendStartsWith(EntityColumn column, string value)
		{
			return AppendStartsWith(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendStartsWith(string junction, EntityColumn column, string value)
		{
			AppendStartsWith(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendStartsWith

		#region AppendEndsWith
		public virtual SqlFilterBuilder<EntityColumn> AppendEndsWith(EntityColumn column, string value)
		{
			return AppendEndsWith(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendEndsWith(string junction, EntityColumn column, string value)
		{
			AppendEndsWith(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendEndsWith

		#region AppendContains
		public virtual SqlFilterBuilder<EntityColumn> AppendContains(EntityColumn column, string value)
		{
			return AppendContains(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendContains(string junction, EntityColumn column, string value)
		{
			AppendContains(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendContains

		#region AppendNotContains
		public virtual SqlFilterBuilder<EntityColumn> AppendNotContains(EntityColumn column, string value)
		{
			return AppendNotContains(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendNotContains(string junction, EntityColumn column, string value)
		{
			AppendNotContains(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendNotContains

		#region AppendLike
		public virtual SqlFilterBuilder<EntityColumn> AppendLike(EntityColumn column, string value)
		{
			return AppendLike(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendLike(string junction, EntityColumn column, string value)
		{
			AppendLike(junction, GetColumnName(column), value);
			return this;
		}
		#endregion AppendLike

		#region AppendNotLike
		public virtual SqlFilterBuilder<EntityColumn> AppendNotLike(EntityColumn column, string value)
		{
			return AppendNotLike(this.Junction, column, value);
		}
		public virtual SqlFilterBuilder<EntityColumn> AppendNotLike(string junction, EntityColumn column, string value)
		{
			AppendNotLike(junction, GetColumnName(column), value);
			return this;
		}
		#endregion

	}
}