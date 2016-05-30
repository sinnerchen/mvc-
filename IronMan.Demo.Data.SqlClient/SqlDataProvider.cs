/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using IronMan.Demo.Data;
namespace IronMan.Demo.Data.SqlClient
{
  public class SqlDataProvider:DataProvider
  {
    public SqlDataProvider(string connStr) : base(connStr) { }
    
    public SqlDataProvider(string connStr,int timeOut) : base(connStr,timeOut) { }

    #region Singleton Patten
    private static object _locker = new object();
    private static SqlStudentService _sqlStudentService;
    private static SqlTeacherService _sqlTeacherService;
    #endregion
    
    public override TransactionManager CreateTransaction()
		{
			return new TransactionManager(this.ConnStr);
		}

    public override IStudent StudentService()
    {
      if (_sqlStudentService == null) {
        lock (_locker) {
          if (_sqlStudentService == null) {
            _sqlStudentService = new SqlStudentService(this.ConnStr,this.TimeOut);
          }
        }
      }
      return _sqlStudentService;
    }

    public override ITeacher TeacherService()
    {
      if (_sqlTeacherService == null) {
        lock (_locker) {
          if (_sqlTeacherService == null) {
            _sqlTeacherService = new SqlTeacherService(this.ConnStr,this.TimeOut);
          }
        }
      }
      return _sqlTeacherService;
    }

  }
}

