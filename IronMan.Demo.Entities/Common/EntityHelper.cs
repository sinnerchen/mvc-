/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
namespace IronMan.Demo.Entities
{
  using System;
  using System.Reflection;
  
  public class EntityHelper
	{
		#region 读取自定义的元数据(属性)
		/// <summary>
		/// 读取用户自定义的某类元数据中的第一个值
		/// </summary>
		/// <typeparam name="T">元数据的自定义类型</typeparam>
		/// <param name="e">相关的列枚举值</param>
		/// <returns></returns>
		public static T GetAttribute<T>(System.Enum e) where T : System.Attribute
		{
			T attribute = default(T);
			Type enumType = e.GetType();
			//获取此枚举类型的所有成员
			System.Reflection.MemberInfo[] members = enumType.GetMember(e.ToString());
			if (members != null && members.Length == 1) {
				//获取此枚举的所有此类型元数据（自定义属性）
				object[] attrs = members[0].GetCustomAttributes(typeof(T), false);
				if (attrs.Length > 0) {
					attribute = (T)attrs[0];
				}
			}
			return attribute;
		}

		/// <summary>
		/// 获取列枚举元数据的列名
		/// </summary>
		public static string GetEnumTextValue(Enum e)
		{
			string ret = "";
			Type t = e.GetType();
			MemberInfo[] members = t.GetMember(e.ToString());
			if (members != null && members.Length == 1) {
				object[] attrs = members[0].GetCustomAttributes(typeof(EnumTextValueAttribute), false);
				if (attrs.Length == 1) {
					ret = ((EnumTextValueAttribute)attrs[0]).Text;
				}
			}
			return ret;
		}
		#endregion
	}

}