/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using IronMan.Demo.Data;
using IronMan.Demo.Entities;

namespace IronMan.Demo.Data.SqlClient
{
  public partial class SqlStudentService:IStudent
  {
    #region Fields
    private string _connStr;
    private int _timeOut=30;
    private string TableName
    {
      get { return "[Student]"; }
    }
    private string[] _columnNames = { "[ID]","[UID]","[Name]","[Sex]","[Born]","[Address]" };
    private string ColumnStr
    {
      get{
        return string.Join(",", _columnNames);
      }
    }
		//{0}为where clause,{1}为Order By,{2}为start,{3}为start+pageLength
		private string _pagedSqlFormat = "begin \r\n "
																							+ "create table #PageIndex([IndexId] int identity(1, 1) not null,[ID] Int) \r\n "
																							+ "insert into #PageIndex ([ID]) select top {3} [ID] from [Student] {0} "
																							+ "order by {1} \r\n "
																							+ "select O.[ID],O.[UID],O.[Name],O.[Sex],O.[Born],O.[Address] "
																							+ "from  [Student] O,#PageIndex PageIndex "
																							+ "where PageIndex.IndexId > {2} and O.[ID] = PageIndex.[ID] "
																							+ "order by PageIndex.IndexId \r\n "
																							+ "select count(1) as TotalRowCount from [Student] {0} \r\n "
																							+ "drop table #PageIndex \r\n "
																							+ "end";
    #endregion
    
    #region 构造函数：只允许本项目内构造
    internal SqlStudentService(string connStr,int timeOut)
    {
      this._connStr = connStr;
			this._timeOut = timeOut;
    }
    
    internal SqlStudentService(string connStr):this(connStr,30)
    {
    }
    #endregion 构造函数：只允许本项目内构造

    #region IStudent 成员

    public Student GetByID(int id)
    {
      SqlDatabase db = new SqlDatabase(this._connStr);
      DbCommand cmd=db.GetSqlStringCommand(string.Format("select {0} from {1} where ID=@id",this.ColumnStr,this.TableName));
      cmd.CommandTimeout=this._timeOut;
      db.AddInParameter(cmd,"@id",SqlDbType.Int,id);
      IDataReader reader = null;
      List<Student> result = null;
      try{
        reader = Utility.ExecuteReader(db,cmd);
        result = Fill(reader, 0, int.MaxValue);
        if ( result != null ) {
          if (result.Count == 1) {
            return result[0];
          } else if(result.Count>1) {
            throw new DataException("Cannot find the unique instance of the class.");
          }
        }
        return null;
      }
      finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }
    
    public Student GetByUID(string uID)
    {
      SqlDatabase db = new SqlDatabase(this._connStr);
      DbCommand cmd=db.GetSqlStringCommand(string.Format("select {0} from {1} where UID=@uID",this.ColumnStr,this.TableName));
      cmd.CommandTimeout=this._timeOut;
      db.AddInParameter(cmd,"@uID",SqlDbType.VarChar,uID);
      IDataReader reader = null;
      List<Student> result = null;
      try{
        reader = Utility.ExecuteReader(db,cmd);
        result = Fill(reader, 0, int.MaxValue);
        if ( result != null ) {
          if (result.Count == 1) {
            return result[0];
          } else if(result.Count>1) {
            throw new DataException("Cannot find the unique instance of the class.");
          }
        }
        return null;
      }
      finally {
        if (reader != null) {
          reader.Close();
        }
      }
    }
    
    public List<Student> GetAll(int start,int pageLength,out int count)
    {
      SqlDatabase db = new SqlDatabase(this._connStr);
      DbCommand cmd = SqlCommandHelper.GetCommandWrapper(db, this._pagedSqlFormat, typeof(StudentColumn), null, this._timeOut, null, start, pageLength);
      IDataReader reader = null;
      List<Student> result = null;
      try{
        reader = Utility.ExecuteReader(db,cmd);
        result = Fill(reader, 0, int.MaxValue);
        if(result!=null){
          count = result.Count;
        } else {
          count = -1;
        }
        if (reader.NextResult()) {
          if (reader.Read()) {
            count = reader.GetInt32(0);
          }
        }
       }
       finally {
        if (reader != null) {
          reader.Close();
        }
        cmd = null;
      }
      return result;
    }
    
    public List<Student> GetAll(out int count)
    {
			SqlDatabase db = new SqlDatabase(this._connStr);
			DbCommand cmd = db.GetSqlStringCommand(string.Format("select {0} from {1} select count(1) from {1} ", this.ColumnStr, this.TableName));
			cmd.CommandTimeout = this._timeOut;
			IDataReader reader = null;
			List<Student> result = null;
			try {
				reader = Utility.ExecuteReader(db, cmd);
				result = Fill(reader, 0, int.MaxValue);
				if (result != null) {
					count = result.Count;
				} else {
					count = -1;
				}
				if (reader.NextResult()) {
					if (reader.Read()) {
						count = reader.GetInt32(0);
					}
				}
			}
			finally {
				if (reader != null) {
					reader.Close();
				}
				cmd = null;
			}
			return result;
    }
    
     /// <summary>
    /// 以拼接字符串方式进行的分页查询
    /// </summary>
    /// <param name="whereClause">示例：ID=3 and LoginName='111'，可空</param>
    /// <param name="orderBy">示例：ID DESC，可空</param>
    /// <param name="start">每页起启的记录索引号</param>
    /// <param name="pageLength">每页记录条数</param>
    /// <param name="count">返回符合条件的记录个数</param>
    /// <returns></returns>
    public List<Student> GetPaged(string whereClause, string orderBy, int start, int pageLength, out int count)
    {
      string whereStr = string.Empty;
      if (!string.IsNullOrEmpty(whereClause)) {
        if (Utility.DetectSqlInjection(whereClause)) {
          throw new Exception("Detect Sql Injection In WhereClause.");
        } else {
          whereStr = string.Format(" Where {0} ", whereClause);
        }
      }
      if (string.IsNullOrEmpty(orderBy)) {
        orderBy = string.Format(" ID ASC ");
      } else {
        if (Utility.DetectSqlInjection(orderBy)) {
          throw new Exception("Detect Sql Injection In OrderBy.");
        }
      }
      //{0}为where clause,{1}为Order By,{2}为start,{3}为start+pageLength
      string sql = String.Format(this._pagedSqlFormat, whereStr, orderBy, start, start + pageLength);
      SqlDatabase db = new SqlDatabase(this._connStr);
      DbCommand cmd = db.GetSqlStringCommand(sql);
      cmd.CommandTimeout = this._timeOut;
      IDataReader reader = null;
      List<Student> result = null;
      try {
        reader = Utility.ExecuteReader(db, cmd);
        result = Fill(reader, 0, int.MaxValue);
        if (result != null) {
          count = result.Count;
        } else {
          count = -1;
        }
        if (reader.NextResult()) {
          if (reader.Read()) {
            count = reader.GetInt32(0);
          }
        }
      }
      finally {
        if (reader != null) {
          reader.Close();
        }
        cmd = null;
      }
      return result;
    }
    
    public List<Student> Find(TransactionManager trans, IFilterParameterCollection parameters,
																		  string orderBy, int start, int pageLength, out int count)
		{
			SqlFilterParameterCollection filter = null;
			if (parameters == null)
				filter = new SqlFilterParameterCollection();
			else
				filter = parameters.GetParameters();
			SqlDatabase db = new SqlDatabase(this._connStr);
			DbCommand cmd = SqlCommandHelper.GetCommandWrapper(db, this._pagedSqlFormat,
																												 typeof(StudentColumn),
																												 filter, this._timeOut,
																												 orderBy, start, pageLength);
			List<Student> rows = null;
			IDataReader reader = null;
			try {
				if (trans != null) {
					reader = Utility.ExecuteReader(trans, cmd);
				} else {
					reader = Utility.ExecuteReader(db, cmd);
				}
				rows = Fill(reader, 0, int.MaxValue);
				if (rows != null) {
					count = rows.Count;
				} else {
					count = -1;
				}
				if (reader.NextResult()) {
					if (reader.Read()) {
						count = reader.GetInt32(0);
					}
				}
			}
			finally {
				if (reader != null)
					reader.Close();
				cmd = null;
			}
			return rows;
		}

    public bool Add(TransactionManager trans,Student entity)
    {
      SqlDatabase db = new SqlDatabase(this._connStr);
      DbCommand cmd = AddCommand(db,entity);
      int result = 0;
      if (trans != null) {
        result = Convert.ToInt32(Utility.ExecuteScalar(trans,cmd));
      } else {
        result = Convert.ToInt32(Utility.ExecuteScalar(db,cmd));
      }
      if ( result != 0 ){
        entity.ID = result; 
        return true;
      }else{
        return false;
      }
    }
    
    public bool Update(TransactionManager trans,Student entity)
    {
      SqlDatabase db = new SqlDatabase(this._connStr);
      DbCommand cmd = UpdateCommand(db,entity);
      int result = 0;
      if (trans != null) {
        result = Utility.ExecuteNonQuery(trans,cmd);
      } else {
        result = Utility.ExecuteNonQuery(db,cmd);
      }
      return Convert.ToBoolean(result);
    }

    public bool Delete(TransactionManager trans,int id)
    {
      SqlDatabase db = new SqlDatabase(this._connStr);
      DbCommand cmd = db.GetSqlStringCommand(string.Format("delete from {0} where ID=@id",this.TableName));
      cmd.CommandTimeout = this._timeOut;
      db.AddInParameter(cmd,"@id",SqlDbType.Int,id);
      int result = 0;
      if (trans != null) {
        result = Utility.ExecuteNonQuery(trans,cmd);
      } else {
        result = Utility.ExecuteNonQuery(db,cmd);
      }
      return Convert.ToBoolean(result);
    }

    #endregion
    
    #region 内部方法
    private DbCommand AddCommand(SqlDatabase db,Student entity)
    {
      StringBuilder colNames = new StringBuilder("[UID],[Name]");
      StringBuilder argNames = new StringBuilder("@uID,@name");
      if (!string.IsNullOrEmpty(entity.Sex)) {
        colNames.Append(",[Sex]");
        argNames.Append(",@sex");
      }
      if (entity.Born.HasValue) {
        colNames.Append(",[Born]");
        argNames.Append(",@born");
      }
      if (!string.IsNullOrEmpty(entity.Address)) {
        colNames.Append(",[Address]");
        argNames.Append(",@address");
      }
      DbCommand cmd = db.GetSqlStringCommand(string.Format("insert into {0}({1}) values({2})  select SCOPE_IDENTITY()",this.TableName,colNames.ToString(), argNames.ToString()));
      db.AddInParameter(cmd,"@uID",SqlDbType.VarChar,entity.UID);
      db.AddInParameter(cmd,"@name",SqlDbType.VarChar,entity.Name);
      if (!string.IsNullOrEmpty(entity.Sex)) {
        db.AddInParameter(cmd,"@sex",SqlDbType.VarChar,entity.Sex);
      }
      if (entity.Born.HasValue) {
        db.AddInParameter(cmd,"@born",SqlDbType.DateTime,entity.Born.Value);
      }
      if (!string.IsNullOrEmpty(entity.Address)) {
        db.AddInParameter(cmd,"@address",SqlDbType.VarChar,entity.Address);
      }
      cmd.CommandTimeout = this._timeOut;
      return cmd;
    }
    
    private DbCommand UpdateCommand(SqlDatabase db,Student entity)
    {
      DbCommand cmd = db.GetSqlStringCommand("update [Student] set [UID]=@uID,[Name]=@name,[Sex]=@sex,[Born]=@born,[Address]=@address where [ID]=@id");
      db.AddInParameter(cmd,"@id",SqlDbType.Int,entity.ID);
      db.AddInParameter(cmd,"@uID",SqlDbType.VarChar,entity.UID);
      db.AddInParameter(cmd,"@name",SqlDbType.VarChar,entity.Name);
      if (!string.IsNullOrEmpty(entity.Sex)) {
        db.AddInParameter(cmd,"@sex",SqlDbType.VarChar,entity.Sex);
      } else {
        db.AddInParameter(cmd,"@sex",SqlDbType.VarChar,DBNull.Value);
      }
      if (entity.Born.HasValue) {
        db.AddInParameter(cmd,"@born",SqlDbType.DateTime,entity.Born.Value);
      } else {
        db.AddInParameter(cmd,"@born",SqlDbType.DateTime,DBNull.Value);
      }
      if (!string.IsNullOrEmpty(entity.Address)) {
        db.AddInParameter(cmd,"@address",SqlDbType.VarChar,entity.Address);
      } else {
        db.AddInParameter(cmd,"@address",SqlDbType.VarChar,DBNull.Value);
      }
      cmd.CommandTimeout = this._timeOut;
      return cmd;
    }
    
    private List<Student> Fill(IDataReader reader,int start,int PageLength)
    {
      List<Student> result = null;
      for (int i = 0;i < start; i++) {
        if (!reader.Read())
          break;
      }
      for (int i = 0;i < PageLength; i++) {
        if (!reader.Read())
          break;
        if (result == null) {
          result = new List<Student>();
        }
        Student entity = new Student();
        entity.ID = (int)reader["ID"];
        entity.UID = (string)reader["UID"];
        entity.Name = (string)reader["Name"];
        entity.Sex = reader["Sex"] as string;
        if (reader["Born"] != DBNull.Value) {
          entity.Born = (DateTime)reader["Born"];
        }
        entity.Address = reader["Address"] as string;
        result.Add(entity);
       }
     return result;
   }
    #endregion
  }
}
