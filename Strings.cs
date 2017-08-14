﻿using System;
using System.Collections.Generic;
using System.Text;

namespace More.System
{
	public static class Strings
	{
		public static int IndexOf(this char[] arr, char c)
		{
			for (int i = 0; i < arr.Length; ++i)
			{
				if (arr[i] == c)
					return i;
			}
			return -1;
		}

		public static bool Contains(this char[] arr, char c)
		{
			return arr.IndexOf(c) >= 0;
		}

		public static bool Contains(this string s, char c)
		{
			return s.IndexOf(c) >= 0;
		}

		private static readonly char[] StraightQuotes = { '\'', '"' };
		private static readonly string LeftQuotes = "‘“";
		private static readonly string RightQuotes = "’”";
		private static readonly string LeftPunctuation = "([{<";

		public static string WithSmartQuotes(this string s)
		{
			StringBuilder b = new StringBuilder();
			int max = s.Length;
			int i = 0;
			while (max > 0 && i < max)
			{
				int j = s.IndexOfAny(StraightQuotes, i);
				if (j < 0)
				{
					b.Append(s, i, max - i);
					break;
				}
				b.Append(s, i, j - i);

				char straight = s[j];
				int k = StraightQuotes.IndexOf(straight);
				char left = LeftQuotes[k];
				char right = RightQuotes[k];
				if (j == 0)
				{
					b.Append(left);
				}
				else
				{
					char prev = s[j - 1];
					if (Char.IsWhiteSpace(prev) || LeftPunctuation.Contains(prev))
					{
						b.Append(left);
					}
					else if (StraightQuotes.Contains(prev) && StraightQuotes.IndexOf(prev) != k)
					{
						b.Append(left);
					}
					else
					{
						b.Append(right);
					}
				}
				i = j + 1;
			}
			return b.ToString();
		}

		private static readonly char[] Newlines = { '\n', '\r' };

		public static IEnumerable<string> Lines(this string s)
		{
			int max = s.Length;
			int i = 0;
			while (max > 0 && i < max)
			{
				int j = s.IndexOfAny(Newlines, i);
				if (j < 0)
				{
					yield return s.Substring(i);
					break;
				}
				yield return s.Substring(i, j - i);
				i = j + 1;
				while (i < max && Newlines.Contains(s[i]))
				{
					++i;
				}
			}
		}

		private static readonly char[] Spaces = { ' ', '\t' };

		public static IEnumerable<string> Words(this string s)
		{
			int max = s.Length;
			int i = 0;
			while (max > 0 && i < max)
			{
				int j = s.IndexOfAny(Spaces, i);
				if (j < 0)
				{
					yield return s.Substring(i);
					break;
				}
				yield return s.Substring(i, j - i);
				i = j + 1;
				while (i < max && Spaces.Contains(s[i]))
				{
					++i;
				}
			}
		}

		public static bool StartsWith(this string s, char c)
		{
			return s.Length > 0 && s[0] == c;
		}

		public static bool EndsWith(this string s, char c)
		{
			return s.Length > 0 && s[s.Length - 1] == c;
		}
	}
}
