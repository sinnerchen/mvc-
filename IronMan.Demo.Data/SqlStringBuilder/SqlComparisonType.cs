/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/

namespace IronMan.Demo.Data
{
  public enum SqlComparisonType
	{
		/// <summary>
		/// 相等比较
		/// </summary>
		Equals,
		/// <summary>
		/// 相当于 LIKE value%.
		/// </summary>
		StartsWith,
		/// <summary>
		/// 相当于 LIKE %value.
		/// </summary>
		EndsWith,
		/// <summary>
		/// 相当于 LIKE %value%.
		/// </summary>
		Contains,
		/// <summary>
		/// 相当于 NOT LIKE %value%.
		/// </summary>
		NotContains,
		/// <summary>
		/// 相当于 LIKE value.
		/// </summary>
		Like,
		/// <summary>
		/// 相当于 NOT LIKE value.
		/// </summary>
		NotLike

	}
}