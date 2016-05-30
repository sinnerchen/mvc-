/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
namespace IronMan.Demo.Entities
{
  using System;
  using System.Data;
  
  /// <summary>
	/// 列属性元数据类型一
	/// </summary>
	public sealed class ColumnEnumAttribute : Attribute
	{
		#region 构造函数
		public ColumnEnumAttribute(String name, Type systemType, DbType dbType, bool isPrimaryKey, bool isIdentity, bool allowDbNull, int length)
		{
			this.Name = name;
			this.SystemType = systemType;
			this.DbType = dbType;
			this.IsPrimaryKey = isPrimaryKey;
			this.IsIdentity = isIdentity;
			this.AllowDbNull = allowDbNull;
			this.Length = length;
		}

		public ColumnEnumAttribute(String name, Type systemType, DbType dbType, bool isPrimaryKey, bool isIdentity, bool allowDbNull)
			: this(name, systemType, dbType, isPrimaryKey, isIdentity, allowDbNull, -1)
		{
		}
		#endregion

		#region 属性区

		private String name;
		/// <summary>
		/// 数据表中的列名
		/// </summary>
		public String Name
		{
			get { return name; }
			set { name = value; }
		}

		private Type systemType;
		public Type SystemType
		{
			get { return systemType; }
			set { systemType = value; }
		}

		private DbType dbType;
		public DbType DbType
		{
			get { return dbType; }
			set { dbType = value; }
		}

		private bool isPrimaryKey;
		public bool IsPrimaryKey
		{
			get { return isPrimaryKey; }
			set { isPrimaryKey = value; }
		}

		private bool isIdentity;
		public bool IsIdentity
		{
			get { return isIdentity; }
			set { isIdentity = value; }
		}

		private bool allowDbNull;
		public bool AllowDbNull
		{
			get { return allowDbNull; }
			set { allowDbNull = value; }
		}

		private int length;
		public int Length
		{
			get { return length; }
			set { length = value; }
		}

		#endregion 属性
	}

}