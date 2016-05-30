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
  public class Student
  {
    /// <summary>
    /// Student.ID，主键，唯一，非空
    /// </summary>
    [DataMember]
    public int ID{ get; set; }

    /// <summary>
    /// Student.UID，唯一，非空，长度32
    /// </summary>
    [DataMember]
    public string UID{ get; set; }

    /// <summary>
    /// Student.Name，非空，长度30
    /// </summary>
    [DataMember]
    public string Name{ get; set; }

    /// <summary>
    /// Student.Sex，长度1
    /// </summary>
    [DataMember]
    public string Sex{ get; set; }

    /// <summary>
    /// Student.Born
    /// </summary>
    [DataMember]
    public DateTime? Born{ get; set; }

    /// <summary>
    /// Student.Address，长度200
    /// </summary>
    [DataMember]
    public string Address{ get; set; }

  }
  
  public enum StudentColumn
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

    [EnumTextValue("Born")]
    [ColumnEnum("Born",typeof(System.DateTime),DbType.DateTime,false,false,true)]
    Born,

    [EnumTextValue("Address")]
    [ColumnEnum("Address",typeof(System.String),DbType.AnsiString,false,false,true)]
    Address
  }
}
