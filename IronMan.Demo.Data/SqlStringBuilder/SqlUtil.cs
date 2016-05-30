/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Text;

namespace IronMan.Demo.Data
{
  public static class SqlUtil
	{
		#region 成员声明

		/// <summary>
		/// SQL AND 关键字.
		/// </summary>
		public static readonly String AND = "AND";
		/// <summary>
		/// SQL OR 关键字.
		/// </summary>
		public static readonly String OR = "OR";
		/// <summary>
		/// SQL ASC 关键字.
		/// </summary>
		public static readonly String ASC = "ASC";
		/// <summary>
		/// SQL DESC 关键字.
		/// </summary>
		public static readonly String DESC = "DESC";
		/// <summary>
		/// SQL NULL 关键字.
		/// </summary>
		public static readonly String NULL = "NULL";
		/// <summary>
		/// 用于表示引用的搜索条件.
		/// </summary>
		public static readonly String TOKEN = "@@@";
		/// <summary>
		/// 用于引用搜索词的分隔符.
		/// </summary>
		public static readonly String QUOTE = "\"";
		/// <summary>
		/// 搜索文本中的通配符.
		/// </summary>
		public static readonly String STAR = "*";
		/// <summary>
		/// SQL 通配符.
		/// </summary>
		public static readonly String WILD = "%";
		/// <summary>
		/// SQL 分组起始符号.
		/// </summary>
		public static readonly String LEFT = "(";
		/// <summary>
		/// SQL 分组结束符号.
		/// </summary>
		public static readonly String RIGHT = ")";
		/// <summary>
		/// 可选的搜索项分隔符.
		/// </summary>
		public static readonly String COMMA = ",";

		/// <summary>
		/// 分页临时表名
		/// </summary>
		public static readonly String PAGE_INDEX = "#PageIndex";

		#endregion 成员声明

		#region Equals表达式

		/// <summary>
		/// 创建 <see cref="SqlComparisonType.Equals"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String Equals(String column, String value)
		{
			return Equals(column, value, false);
		}

		/// <summary>
		/// 创建 <see cref="SqlComparisonType.Equals"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static String Equals(String column, String value, bool ignoreCase)
		{
			return Equals(column, value, ignoreCase, true);
		}

		/// <summary>
		/// 创建<see cref="SqlComparisonType.Equals"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String Equals(String column, String value, bool ignoreCase, bool surround)
		{
			if (String.IsNullOrEmpty(value)) return IsNull(column);
			return String.Format(GetEqualFormat(ignoreCase, surround), column, Equals(value));
		}

		/// <summary>
		/// 将 <see cref="SqlComparisonType.Equals"/> 表达式中的值进行Encode.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static String Equals(String value)
		{
			return String.Format("{0}", Encode(value));
		}

		#endregion Equals表达式

		#region Contains表达式

		/// <summary>
		/// 创建<see cref="SqlComparisonType.Contains"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String Contains(String column, String value)
		{
			return Contains(column, value, false);
		}

		/// <summary>
		/// 创建 <see cref="SqlComparisonType.Contains"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static String Contains(String column, String value, bool ignoreCase)
		{
			return Contains(column, value, ignoreCase, true);
		}

		/// <summary>
		/// 创建<see cref="SqlComparisonType.Contains"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String Contains(String column, String value, bool ignoreCase, bool surround)
		{
			if (String.IsNullOrEmpty(value)) return IsNull(column);
			return String.Format(GetLikeFormat(ignoreCase, surround), column, Contains(value));
		}

		/// <summary>
		/// 将 <see cref="SqlComparisonType.Contains"/> 表达式中的值Encode.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static String Contains(String value)
		{
			return String.Format("%{0}%", Encode(value));
		}

		#endregion Contains表达式

		#region NotContains表达式

		/// <summary>
		/// 创建<see cref="SqlComparisonType.NotContains"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String NotContains(String column, String value)
		{
			return NotContains(column, value, false);
		}

		/// <summary>
		/// 创建 <see cref="SqlComparisonType.NotContains"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static String NotContains(String column, String value, bool ignoreCase)
		{
			return NotContains(column, value, ignoreCase, true);
		}

		/// <summary>
		/// 创建<see cref="SqlComparisonType.NotContains"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String NotContains(String column, String value, bool ignoreCase, bool surround)
		{
			if (String.IsNullOrEmpty(value)) return IsNull(column);
			return String.Format(GetNotLikeFormat(ignoreCase, surround), column, NotContains(value));
		}

		/// <summary>
		/// 为<see cref="SqlComparisonType.NotContains"/> 表达式的值进行Encode.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static String NotContains(String value)
		{
			return String.Format("%{0}%", Encode(value));
		}

		#endregion Contains表达式

		#region StartsWith表达式

		/// <summary>
		/// 创建<see cref="SqlComparisonType.StartsWith"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String StartsWith(String column, String value)
		{
			return StartsWith(column, value, false);
		}

		/// <summary>
		/// 创建<see cref="SqlComparisonType.StartsWith"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static String StartsWith(String column, String value, bool ignoreCase)
		{
			return StartsWith(column, value, ignoreCase, true);
		}

		/// <summary>
		/// 创建 <see cref="SqlComparisonType.StartsWith"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String StartsWith(String column, String value, bool ignoreCase, bool surround)
		{
			if (String.IsNullOrEmpty(value)) return IsNull(column);
			return String.Format(GetLikeFormat(ignoreCase, surround), column, StartsWith(value));
		}

		/// <summary>
		/// 为<see cref="SqlComparisonType.StartsWith"/> 表达式的值进行Encode.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static String StartsWith(String value)
		{
			return String.Format("{0}%", Encode(value));
		}

		#endregion StartsWith表达式

		#region EndsWith表达式

		/// <summary>
		/// 创建 <see cref="SqlComparisonType.EndsWith"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String EndsWith(String column, String value)
		{
			return EndsWith(column, value, false);
		}

		/// <summary>
		/// 创建<see cref="SqlComparisonType.EndsWith"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static String EndsWith(String column, String value, bool ignoreCase)
		{
			return EndsWith(column, value, ignoreCase, true);
		}

		/// <summary>
		/// 创建<see cref="SqlComparisonType.EndsWith"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String EndsWith(String column, String value, bool ignoreCase, bool surround)
		{
			if (String.IsNullOrEmpty(value)) return IsNull(column);
			return String.Format(GetLikeFormat(ignoreCase, surround), column, EndsWith(value));
		}

		/// <summary>
		/// 为<see cref="SqlComparisonType.EndsWith"/> 表达式的值进行Encode.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static String EndsWith(String value)
		{
			return String.Format("%{0}", Encode(value));
		}

		#endregion EndsWith表达式

		#region Like表达式

		/// <summary>
		/// 创建 <see cref="SqlComparisonType.Like"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String Like(String column, String value)
		{
			return Like(column, value, false);
		}

		/// <summary>
		/// 创建<see cref="SqlComparisonType.Like"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static String Like(String column, String value, bool ignoreCase)
		{
			return Like(column, value, ignoreCase, true);
		}

		/// <summary>
		/// 创建<see cref="SqlComparisonType.Like"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String Like(String column, String value, bool ignoreCase, bool surround)
		{
			if (String.IsNullOrEmpty(value)) return IsNull(column);
			return String.Format(GetLikeFormat(ignoreCase, surround), column, Like(value));
		}

		/// <summary>
		/// 为<see cref="SqlComparisonType.Like"/> 表达式的值进行Encode.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static String Like(String value)
		{
			return String.Format("{0}", Encode(value));
		}

		#endregion Like表达式

		#region NotLike表达式

		/// <summary>
		/// 创建<see cref="SqlComparisonType.NotLike"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String NotLike(String column, String value)
		{
			return NotLike(column, value, false);
		}

		/// <summary>
		/// 创建<see cref="SqlComparisonType.NotLike"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static String NotLike(String column, String value, bool ignoreCase)
		{
			return NotLike(column, value, ignoreCase, true);
		}

		/// <summary>
		/// 创建 <see cref="SqlComparisonType.NotLike"/> 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String NotLike(String column, String value, bool ignoreCase, bool surround)
		{
			if (String.IsNullOrEmpty(value)) return IsNull(column);
			return String.Format(GetNotLikeFormat(ignoreCase, surround), column, Like(value));
		}

		/// <summary>
		/// 为<see cref="SqlComparisonType.NotLike"/> 表达式的值进行Encode.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static String NotLike(String value)
		{
			return String.Format("{0}", Encode(value));
		}

		#endregion Like表达式

		#region Null/Not Null表达式

		/// <summary>
		/// 创建 IS NULL 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public static String IsNull(String column)
		{
			return String.Format("{0} IS NULL", column);
		}

		/// <summary>
		/// 创建IS NOT NULL 表达式.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public static String IsNotNull(String column)
		{
			return String.Format("{0} IS NOT NULL", column);
		}

		#endregion Null/Not Null表达式

		#region Encode

		/// <summary>
		/// 用SQL 表达式对特殊值进行编码.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String Encode(String value)
		{
			return Encode(value, false);
		}

		/// <summary>
		/// 用SQL 表达式对特殊值进行编码，可选值用单引号括起
		/// </summary>
		/// <param name="value"></param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String Encode(String value, bool surround)
		{
			if (String.IsNullOrEmpty(value)) return SqlUtil.NULL;
			String format = surround ? "'{0}'" : "{0}";
			return String.Format(format, value.Replace("'", "''"));
		}

		/// <summary>
		/// 用SQL 表达式对特殊值进行编码.
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static String Encode(String[] values)
		{
			return Encode(values, false);
		}

		/// <summary>
		/// 用SQL 表达式对特殊值进行编码，可选值用单引号括起
		/// </summary>
		/// <param name="values"></param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String Encode(String[] values, bool surround)
		{
			if (values == null || values.Length < 1) {
				return SqlUtil.NULL;
			}

			StringBuilder csv = new StringBuilder();

			foreach (String value in values) {
				if (!String.IsNullOrEmpty(value)) {
					if (csv.Length > 0)
						csv.Append(",");

					csv.Append(SqlUtil.Encode(value.Trim(), surround));
				}
			}

			return csv.ToString();
		}

		#endregion Encode

		#region Format 方法

		/// <summary>
		/// 获取like格式串.
		/// </summary>
		/// <param name="ignoreCase">如果 <c>true</c> [忽略大小写].</param>
		/// <returns></returns>
		public static String GetLikeFormat(bool ignoreCase)
		{
			return GetLikeFormat(ignoreCase, true);
		}

		/// <summary>
		/// 获取like格式串.
		/// </summary>
		/// <param name="ignoreCase">如果为 <c>true</c> [忽略大小字].</param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String GetLikeFormat(bool ignoreCase, bool surround)
		{
			if (surround) {
				return ignoreCase ? "UPPER({0}) LIKE UPPER('{1}')" : "{0} LIKE '{1}'";
			}

			return ignoreCase ? "UPPER({0}) LIKE UPPER({1})" : "{0} LIKE {1}";
		}

		/// <summary>
		/// 获取not like格式串.
		/// </summary>
		/// <param name="ignoreCase">如果为<c>true</c> [忽略大小写].</param>
		/// <returns></returns>
		public static String GetNotLikeFormat(bool ignoreCase)
		{
			return GetNotLikeFormat(ignoreCase, true);
		}

		/// <summary>
		/// 获取not like格式串.
		/// </summary>
		/// <param name="ignoreCase">如果为 <c>true</c> [忽略大小写].</param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String GetNotLikeFormat(bool ignoreCase, bool surround)
		{
			if (surround) {
				return ignoreCase ? "UPPER({0}) NOT LIKE UPPER('{1}')" : "{0} NOT LIKE '{1}'";
			}

			return ignoreCase ? "UPPER({0}) NOT LIKE UPPER({1})" : "{0} NOT LIKE {1}";
		}



		/// <summary>
		/// 获取Equal格式串
		/// </summary>
		/// <param name="ignoreCase">如果 <c>true</c> [忽略大小写].</param>
		/// <returns></returns>
		public static String GetEqualFormat(bool ignoreCase)
		{
			return GetEqualFormat(ignoreCase, true);
		}

		/// <summary>
		/// 获取Equal格式串
		/// </summary>
		/// <param name="ignoreCase">如果 <c>true</c> [忽略大小写].</param>
		/// <param name="surround"></param>
		/// <returns></returns>
		public static String GetEqualFormat(bool ignoreCase, bool surround)
		{
			if (surround) {
				return ignoreCase ? "UPPER({0}) = UPPER('{1}')" : "{0} = '{1}'";
			}

			return ignoreCase ? "UPPER({0}) = UPPER({1})" : "{0} = {1}";
		}

		#endregion Format 方法
	}
}