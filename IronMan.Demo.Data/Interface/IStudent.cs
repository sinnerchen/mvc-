/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using IronMan.Demo.Entities;
using System.Collections.Generic;
namespace IronMan.Demo.Data
{
  public partial interface IStudent
  {
    Student GetByID(int id);
    Student GetByUID(string uID);
    List<Student> GetAll(int start,int pageLength,out int count);
    List<Student> GetAll(out int count);
    List<Student> GetPaged(string whereClause, string orderBy, int start, int pageLength, out int count);
    List<Student> Find(TransactionManager trans, IFilterParameterCollection parameters, 
			                        string orderBy, int start, int pageLength, out int count);
    bool Add(TransactionManager trans,Student entity);
    bool Update(TransactionManager trans,Student entity);
    bool Delete(TransactionManager trans,int id);
  }
}
