/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
namespace IronMan.Demo.Entities
{
  using System;
  using System.Data;
  using System.Runtime.Serialization;

  [DataContract]
  public class Teacher
  {
    /// <summary>
    /// Teacher.ID，主键，唯一，非空
    /// </summary>
    [DataMember]
    public int ID{ get; set; }

    /// <summary>
    /// Teacher.UID，唯一，非空，长度32
    /// </summary>
    [DataMember]
    public string UID{ get; set; }

    /// <summary>
    /// Teacher.Name，非空，长度30
    /// </summary>
    [DataMember]
    public string Name{ get; set; }

    /// <summary>
    /// Teacher.Sex，长度1
    /// </summary>
    [DataMember]
    public string Sex{ get; set; }

    /// <summary>
    /// Teacher.Subject，长度100
    /// </summary>
    [DataMember]
    public string Subject{ get; set; }

  }
  
  public enum TeacherColumn
  {
    [EnumTextValue("ID")]
    [ColumnEnum("ID",typeof(System.Int32),DbType.Int32,true,true,false)]
    ID,

    [EnumTextValue("UID")]
    [ColumnEnum("UID",typeof(System.String),DbType.AnsiString,false,false,false)]
    UID,

    [EnumTextValue("Name")]
    [ColumnEnum("Name",typeof(System.String),DbType.AnsiString,false,false,false)]
    Name,

    [EnumTextValue("Sex")]
    [ColumnEnum("Sex",typeof(System.String),DbType.AnsiString,false,false,true)]
    Sex,

    [EnumTextValue("Subject")]
    [ColumnEnum("Subject",typeof(System.String),DbType.AnsiString,false,false,true)]
    Subject
  }
}
