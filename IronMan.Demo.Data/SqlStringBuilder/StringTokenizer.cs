/******************************
 * Author: rosiu 
 * Email:  rosiu#foxmail.com
 * Date:   2016.05.04
 * ****************************/
using System;
using System.Collections.Generic;

namespace IronMan.Demo.Data
{
  /// <summary>
	/// 字符串分隔类，类似split(java.util.StringTokenizer的C#版本)
	/// </summary>
	public class StringTokenizer : IEnumerable<string>
	{
		/// <summary>
		/// 默认分隔符，空格、tab、换行、回车、回退
		/// </summary>
		public const string DefaultDelimiters = " \t\n\r\f";

		private readonly string delims = DefaultDelimiters;
		private string[] tokens = null;
		private int index = 0;
		private string empty = String.Empty;

		#region 构造函数
		public StringTokenizer(string str)
		{
			Tokenize(str, false, false);
		}

		public StringTokenizer(string str, string delims)
		{
			if (delims != null) this.delims = delims;
			Tokenize(str, false, false);
		}

		public StringTokenizer(string str, params char[] delims)
		{
			if (delims != null) this.delims = new string(delims);
			Tokenize(str, false, false);
		}

		public StringTokenizer(string str, string delims, bool returnDelims)
		{
			if (delims != null) this.delims = delims;
			Tokenize(str, returnDelims, false);
		}

		public StringTokenizer(string str, string delims, bool returnDelims, bool returnEmpty)
		{
			if (delims != null) this.delims = delims;
			Tokenize(str, returnDelims, returnEmpty);
		}

		public StringTokenizer(string str, string delims, bool returnDelims, bool returnEmpty, string empty)
		{
			if (delims != null) this.delims = delims;
			this.empty = empty;
			Tokenize(str, returnDelims, returnEmpty);
		}
		#endregion

		#region
		private void Tokenize(string str, bool returnDelims, bool returnEmpty)
		{
			if (returnDelims) {
				this.tokens = str.Split(this.delims.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				List<string> tmp = new List<string>(tokens.Length << 1);

				int delimIndex = str.IndexOfAny(this.delims.ToCharArray());
				int tokensIndex = 0;
				int prevDelimIdx = delimIndex - 1;

				if (delimIndex == 0)
					do {
						tmp.Add(new string(str[delimIndex], 1));
						prevDelimIdx = delimIndex++;
						delimIndex = str.IndexOfAny(this.delims.ToCharArray(), delimIndex);
						if (returnEmpty && delimIndex == prevDelimIdx + 1)
							tmp.Add(this.empty);
					} while (delimIndex == prevDelimIdx + 1);

				while (delimIndex > -1) {
					tmp.Add(this.tokens[tokensIndex++]);

					do {
						tmp.Add(new string(str[delimIndex], 1));
						prevDelimIdx = delimIndex++;
						delimIndex = str.IndexOfAny(this.delims.ToCharArray(), delimIndex);
						if (returnEmpty && delimIndex == prevDelimIdx + 1)
							tmp.Add(this.empty);
					} while (delimIndex == prevDelimIdx + 1);

				}
				if (tokensIndex < tokens.Length)
					tmp.Add(this.tokens[tokensIndex++]);

				this.tokens = tmp.ToArray();
				tmp = null;
			} else if (returnEmpty) {
				this.tokens = str.Split(this.delims.ToCharArray(), StringSplitOptions.None);
				if (this.empty != String.Empty)
					for (int i = 0; i < this.tokens.Length; i++)
						if (this.tokens[i] == String.Empty) this.tokens[i] = this.empty;
			} else
				this.tokens = str.Split(this.delims.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		}
		#endregion

		#region

		public bool HasMoreTokens
		{
			get { return this.index < this.tokens.Length; }
		}

		public string NextToken
		{
			get { return this.tokens[index++]; }
		}

		public int CountTokens
		{
			get { return this.tokens.Length - this.index; }
		}
		#endregion

		#region 

		public int Count
		{
			get { return this.tokens.Length; }
		}

		public string this[int index]
		{
			get { return this.tokens[index]; }
		}

		public void Reset()
		{
			this.index = 0;
		}

		public string EmptyString
		{
			get { return this.empty; }
		}
		#endregion

		#region
		public IEnumerator<string> GetEnumerator()
		{
			while (this.HasMoreTokens)
				yield return this.NextToken;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion

	}
}