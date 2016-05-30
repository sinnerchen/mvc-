/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Collections.Generic;

namespace IronMan.Demo.Data
{
  public class SqlFilterParameterCollection : List<SqlFilterParameter>, IFilterParameterCollection
	{
		#region 方法区

		/// <summary>
		/// 当前列设置
		/// </summary>
		public void SetCurrentColumn(Object column)
		{
			this.currentColumn = (Enum)column;
		}

		/// <summary>
		/// 获取下一个参数的名称，并指定相关值，加入到参数列表中
		/// </summary>
		public String GetParameter(string value)
		{
			SqlFilterParameter parameter = new SqlFilterParameter(CurrentColumn, value, Count);
			Add(parameter);
			return parameter.Name;
		}

		#endregion 方法区

		#region 属性区

		/// <summary>
		/// 当前列
		/// </summary>
		private Enum currentColumn;

		/// <summary>
		/// 获取当前列
		/// </summary>
		public Enum CurrentColumn
		{
			get { return currentColumn; }
		}

		/// <summary>
		/// 选择表达式串成员变量
		/// </summary>
		private String filterExpression;

		/// <summary>
		/// 获取或设置选择表达式
		/// </summary>
		public String FilterExpression
		{
			get { return filterExpression; }
			set { filterExpression = value; }
		}

		#endregion 属性区

		#region IFilterParameterCollection
		/// <summary>
		/// 获取选择表达式列表
		/// </summary>
		SqlFilterParameterCollection IFilterParameterCollection.GetParameters()
		{
			return this;
		}
		#endregion
	}
}