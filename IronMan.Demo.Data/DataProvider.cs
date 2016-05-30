/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
namespace IronMan.Demo.Data
{
  using System;
  public abstract class DataProvider
  {
    private string _connStr;
    private int _timeOut=30;
    
    public string ConnStr
    {
      get { return this._connStr; }
    }
    
    public int TimeOut
    {
      get { return this._timeOut; }
    }

    public DataProvider(string connStr)
    {
      this._connStr = connStr;
    }
    
    public DataProvider(string connStr,int timeOut)
    {
      this._connStr = connStr;
      this._timeOut = timeOut;
    }
    
    private DataProvider()
    {
    }
    
    public virtual TransactionManager CreateTransaction()
		{
			 throw new NotSupportedException();
		}

    public abstract IStudent StudentService();
    public abstract ITeacher TeacherService();
  }
}
