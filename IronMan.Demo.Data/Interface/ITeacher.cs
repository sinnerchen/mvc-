/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using IronMan.Demo.Entities;
using System.Collections.Generic;
namespace IronMan.Demo.Data
{
  public partial interface ITeacher
  {
    Teacher GetByID(int id);
    Teacher GetByUID(string uID);
    List<Teacher> GetAll(int start,int pageLength,out int count);
    List<Teacher> GetAll(out int count);
    List<Teacher> GetPaged(string whereClause, string orderBy, int start, int pageLength, out int count);
    List<Teacher> Find(TransactionManager trans, IFilterParameterCollection parameters, 
			                        string orderBy, int start, int pageLength, out int count);
    bool Add(TransactionManager trans,Teacher entity);
    bool Update(TransactionManager trans,Teacher entity);
    bool Delete(TransactionManager trans,int id);
  }
}
