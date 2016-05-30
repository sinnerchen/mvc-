/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
namespace IronMan.Demo.Entities
{
  using System;
  
  /// <summary>
	/// 列元数据类型二：列名
	/// </summary>
	public sealed class EnumTextValueAttribute : Attribute
	{
		private readonly string enumTextValue;
		public string Text
		{
			get
			{
				return enumTextValue;
			}
		}

		public EnumTextValueAttribute(string text)
		{
			enumTextValue = text;
		}
	}

}