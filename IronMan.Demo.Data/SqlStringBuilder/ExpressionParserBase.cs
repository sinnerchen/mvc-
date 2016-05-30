/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace  IronMan.Demo.Data
{
  public abstract class ExpressionParserBase
	{
		#region 构造函数
		protected ExpressionParserBase(String propertyName, SqlComparisonType comparisonType, bool ignoreCase)
		{
			PropertyName = propertyName;
			ComparisonType = comparisonType;
			IgnoreCase = ignoreCase;
		}
		#endregion 构造函数

		#region 属性区
		private bool ignoreCase;
		public bool IgnoreCase
		{
			get { return ignoreCase; }
			set { ignoreCase = value; }
		}
		private String propertyName;
		public String PropertyName
		{
			get { return propertyName; }
			set { propertyName = value; }
		}
		private SqlComparisonType comparisonType;
		public SqlComparisonType ComparisonType
		{
			get { return comparisonType; }
			set { comparisonType = value; }
		}
		#endregion 属性区

		#region 方法
		protected void ParseCore(String searchText)
		{
			IList<String> quotedValues = new List<String>();
			int leftNumber = 0;
			int rightNumber = 0;
			int i = -1;
			//最后一个参数是否为关键词
			bool isKeyWord = false;
			// 如果第一个参数是关键词，用来跟踪
			// i.e., and or "john smith"
			int numParams = 0;
			// 使用AND来搜索
			// 如 John Smith能够转换成John and Smith
			// 但 "John Smith" 是一个整体
			bool needToInsertAND = false;
			String outStr = ParseQuotes(searchText, quotedValues);
			StringTokenizer tokenizer = new StringTokenizer(outStr, "( ),\t\r\n", true);
			String nextToken;
			while (tokenizer.HasMoreTokens) {
				nextToken = tokenizer.NextToken.Trim();
				if (nextToken.Equals(SqlUtil.LEFT)) {
					leftNumber++;
					if (needToInsertAND) {
						AppendAnd();
					}
					OpenGrouping();
					needToInsertAND = false;
					isKeyWord = false;
				} else if (nextToken.Equals(SqlUtil.RIGHT)) {
					rightNumber++;
					CloseGrouping();
					needToInsertAND = true;
					isKeyWord = false;
				} else if (nextToken.Equals(SqlUtil.COMMA)) {
					AppendOr();
					needToInsertAND = false;
					isKeyWord = false;
				} else if (IsKeyWord(nextToken)) {
					numParams++;
					if (numParams == 1) {
						needToInsertAND = true;
						AppendSearchText(nextToken);
					} else if ((numParams == 2) && (tokenizer.CountTokens <= 1)) {
						AppendAnd();
						AppendSearchText(nextToken);
					} else if (isKeyWord) {
						needToInsertAND = true;
						isKeyWord = false;
						AppendSearchText(nextToken);
					} else {
						if (tokenizer.CountTokens <= 1) {
							AppendAnd();
							AppendSearchText(nextToken);
						} else {
							if (SqlUtil.AND.Equals(nextToken, StringComparison.OrdinalIgnoreCase)) {
								AppendAnd();
							} else if (SqlUtil.OR.Equals(nextToken, StringComparison.OrdinalIgnoreCase)) {
								AppendOr();
							}
							needToInsertAND = false;
							isKeyWord = true;
						}
					}
				} else if (nextToken.Equals(" ")) {
					AppendSpace();
				} else if (nextToken.Equals("")) {
				} else if (nextToken.Equals(SqlUtil.TOKEN)) {
					numParams++;
					if (needToInsertAND) {
						AppendAnd();
					}
					needToInsertAND = true;
					isKeyWord = false;
					i++;
					AppendSearchText(quotedValues[i]);
				} else {
					numParams++;
					if (needToInsertAND) {
						AppendAnd();
					}
					needToInsertAND = true;
					isKeyWord = false;
					AppendSearchText(nextToken);
				}
			}
			if (leftNumber != rightNumber) {
				throw new ArgumentException("Syntax Error: mismatched parenthesis.");
			}
		}

		private String ParseQuotes(String searchText, IList<String> quotedValues)
		{
			if (String.IsNullOrEmpty(searchText) || searchText.IndexOf('"') < 0) {
				return searchText;
			}
			String[] tokens = searchText.Split('"');
			StringBuilder sb = new StringBuilder();
			bool needEndQuotes = true;
			foreach (String token in tokens) {
				needEndQuotes = !needEndQuotes;
				if (needEndQuotes) {
					sb.Append(SqlUtil.TOKEN);
					quotedValues.Add(token);
				} else {
					sb.Append(token);
				}
			}
			if (needEndQuotes) {
				throw new ArgumentException("Syntax Error: mismatched quotes.");
			}
			return sb.ToString();
		}

		private bool IsKeyWord(String word)
		{
			return (word != null && (
				SqlUtil.AND.Equals(word, StringComparison.OrdinalIgnoreCase) ||
				SqlUtil.OR.Equals(word, StringComparison.OrdinalIgnoreCase)
			));
		}
		#endregion 方法

		#region 抽象方法
		protected abstract void AppendOr();
		protected abstract void AppendAnd();
		protected abstract void AppendSpace();
		protected abstract void OpenGrouping();
		protected abstract void CloseGrouping();
		protected abstract void AppendSearchText(String searchText);
		#endregion 抽象方法
	}
}