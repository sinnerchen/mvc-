/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using IronMan.Demo.Entities;

namespace IronMan.Demo.Data
{
  public class SqlFilterParameter
	{
		#region 构造函数
		public SqlFilterParameter(Enum column, string value, int index)
		{
			this.column = column;
			this.parameterValue = value;
			this.parameterIndex = index;
		}
		#endregion 构造函数

		#region 属性
		private Enum column;
		public Enum Column
		{
			get { return column; }
			set { column = value; }
		}

		private String parameterValue;
		/// <summary>
		/// 参数值：所有的参数都事先被转为String值了
		/// 在正式加入DbCommand前，又将被转换为相对应的系统类型的值
		/// </summary>
		public String Value
		{
			get { return parameterValue; }
			set { parameterValue = value; }
		}

		private int parameterIndex;
		/// <summary>
		/// 参数在此过程中的加入顺序
		/// </summary>
		public int Index
		{
			get { return parameterIndex; }
		}

		/// <summary>
		/// 参数名，会加上@Param前缀
		/// </summary>
		public String Name
		{
			get { return String.Format("@Param{0}", Index); }
		}

		/// <summary>
		/// 根据列枚举的元数据来获取此列的数据库类型
		/// </summary>
		public System.Data.DbType DbType
		{
			get
			{
				System.Data.DbType dbType = System.Data.DbType.String;
				if (column != null) {
					ColumnEnumAttribute attribute = EntityHelper.GetAttribute<ColumnEnumAttribute>(column);
					if (attribute != null) {
						dbType = attribute.DbType;
					}
				}
				return dbType;
			}
		}

		/// <summary>
		///根据列枚举的元数据来获取此列的系统数据类型
		/// </summary>
		public System.Type SystemType
		{
			get
			{
				System.Type type = typeof(string);
				if (column != null) {
					ColumnEnumAttribute attribute = EntityHelper.GetAttribute<ColumnEnumAttribute>(column);
					if (attribute != null) {
						type = attribute.SystemType;
					}
				}
				return type;
			}
		}
		#endregion 属性区

		#region 方法区
		/// <summary>
		/// 将参数的值转换为对应的系统数据类型的值
		/// </summary>
		public object GetValue()
		{
			return EntityUtil.ChangeType(Value, SystemType);
		}
		#endregion 方法区

	}
}