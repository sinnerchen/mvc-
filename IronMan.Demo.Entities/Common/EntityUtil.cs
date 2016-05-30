/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
namespace IronMan.Demo.Entities
{
  using System;
  
  public class EntityUtil
	{
		/// <summary>
		/// 数据类型转换
		/// </summary>
		public static Object ChangeType(Object value, Type conversionType, bool convertBlankToNull)
		{
			Object newValue = null;
			//空值或纯空格串处理
			if (convertBlankToNull && value != null) {
				if (value is String) {
					String strValue = value.ToString().Trim();
					if (String.IsNullOrEmpty(strValue)) {
						value = null;
					}
				}
			}
			if (conversionType.IsInstanceOfType(value)) {
				newValue = value;
			} else if (conversionType.IsGenericType) {
				//泛类型转换
				newValue = ChangeGenericType(value, conversionType, convertBlankToNull);
			} else if (value != null) {
				// 无法直接转换的类型处理
				if (!(value is IConvertible)) {
					// 特殊的byte[]类型转换
					if (conversionType == typeof(Byte[])) {
						newValue = value;
					} else {
						value = value.ToString();
					}
				}
				// Guid类型转换
				if (conversionType == typeof(Guid)) {
					if (!String.IsNullOrEmpty(value.ToString())) {
						newValue = new Guid(value.ToString());
					}
				} else {
					newValue = Convert.ChangeType(value, conversionType);
				}
			}
			return newValue;
		}

		/// <summary>
		/// 转换为指定类型
		/// </summary>
		public static Object ChangeType(Object value, Type conversionType)
		{
			return ChangeType(value, conversionType, true);
		}

		/// <summary>
		/// 泛类型转换
		/// </summary>
		public static Object ChangeGenericType(Object value, Type conversionType)
		{
			return ChangeGenericType(value, conversionType, true);
		}
		/// <summary>
		/// 泛类型转换
		/// </summary>
		/// <param name="value"></param>
		/// <param name="conversionType"></param>
		/// <param name="convertBlankToNull"></param>
		/// <returns></returns>
		public static Object ChangeGenericType(Object value, Type conversionType, bool convertBlankToNull)
		{
			Object newValue = null;
			if (conversionType.IsGenericType) {
				Type typeDef = conversionType.GetGenericTypeDefinition();
				Type[] typeArgs = conversionType.GetGenericArguments();
				if (typeArgs.Length == 1) {
					Type newType = typeArgs[0];
					Object arg = ChangeType(value, newType, convertBlankToNull);
					newValue = GetNewGenericEntity(typeDef, typeArgs, arg);
				}
			}
			return newValue;
		}

		/// <summary>
		/// 创建一个泛类型的实例
		/// </summary>
		public static Object GetNewGenericEntity(Type typeDefinition, Type[] typeArguments, params Object[] args)
		{
			Type genericType = MakeGenericType(typeDefinition, typeArguments);
			return GetNewGenericEntity(genericType, args);
		}

		/// <summary>
		/// 使用与指定参数匹配程度最高的构造函数创建指定泛类型的实例
		/// 如：创建Dictionary&lt;string,string&gt;
		/// </summary>
		public static Object GetNewGenericEntity(Type genericType, params Object[] args)
		{
			Object entity = null;
			if (genericType != null) {
				// 确保不创建空参数的泛型
				if (args != null && args.Length == 1 && args[0] == null) {
					args = null;
				}
				entity = Activator.CreateInstance(genericType, args);
			}
			return entity;
		}

		/// <summary>
		/// 创建泛类型，如创建Dictionary&lt;int,string&gt;
		/// </summary>
		public static Type MakeGenericType(Type typeDefinition, Type[] typeArguments)
		{
			Type genericType = null;
			if (typeDefinition != null && typeArguments != null && typeArguments.Length > 0) {
				genericType = typeDefinition.MakeGenericType(typeArguments);
			}
			return genericType;
		}
	}

}