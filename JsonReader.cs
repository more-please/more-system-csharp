using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
	public class JsonReader
	{
		// ---------------------------------------------------------------------
		// High-level API

		public static object Read(string str)
		{
			try
			{
				return new JsonReader(str).Read();
			}
			catch (FormatException)
			{
				// TODO: log error
				return null;
			}
		}

		public JsonReader(string str, int startIndex = 0)
		{
			_str = str;
			_max = str.Length;
			_i = startIndex;
		}

		public object Read()
		{
			Trim();
			if (AtEnd)
			{
				Fail("unexpected end of input");
			}
			char c = Peek;
			if (c == '"')
			{
				return ReadString();
			}
			else if (c == '[')
			{
				return ReadArray();
			}
			else if (c == '{')
			{
				return ReadDict();
			}
			else if (c == '-' || char.IsDigit(c))
			{
				return ReadNumber();
			}
			else if (c == 't')
			{
				Expect("true");
				return true;
			}
			else if (c == 'f')
			{
				Expect("false");
				return false;
			}
			else if (c == 'n')
			{
				Expect("null");
				return null;
			}
			else
			{
				Fail($"unexpected char: '{c}'");
				return null;
			}
		}

		// ---------------------------------------------------------------------
		// Parse functions for JSON types

		public object ReadNumber()
		{
			double result = Sign() * Decimal();

			// Return an int if we can
			if (result >= Int32.MinValue && result <= Int32.MaxValue)
				if (Peek != '.' && Peek != 'e' && Peek != 'E')
					return (int)result;

			if (Maybe('.'))
			{
				int fracPos = _i;
				long frac = Decimal();
				result += frac / Math.Pow(10, _i - fracPos);
			}
			if (Maybe('e') || Maybe('E'))
			{
				long exp = Sign() * Decimal();
				result *= Math.Pow(10, exp);
			}
			return result;
		}

		public string ReadString()
		{
			StringBuilder result = new StringBuilder();
			Expect('"');
			while (!AtEnd)
			{
				char c = Pop();
				if (c == '"')
				{
					return result.ToString();
				}
				if (c == '\\')
				{
					c = Pop();
					switch (c)
					{
						case 'b': c = '\b'; break;
						case 'f': c = '\f'; break;
						case 'n': c = '\n'; break;
						case 'r': c = '\r'; break;
						case 't': c = '\t'; break;
						case 'u': c = Unicode(); break;
						default: break;
					}
				}
				result.Append(c);
			}
			Fail("unterminated string");
			return null;
		}

		public IList<object> ReadArray()
		{
			List<object> result = new List<object>();
			Expect('[');
			Trim();
			if (Maybe(']'))
			{
				return result;
			}
			do
			{
				object item = Read();
				result.Add(item);
				Trim();
			}
			while (Maybe(','));
			Expect(']');
			return result;
		}

		public IDictionary<string, object> ReadDict()
		{
			var result = new Dictionary<string, object>();
			Expect('{');
			Trim();
			if (Maybe('}'))
			{
				return result;
			}
			do
			{
				Trim();
				string key = ReadString();
				Trim();
				Expect(':');
				object val = Read();
				result.Add(key, val);
				Trim();
			}
			while (Maybe(','));
			Expect('}');
			return result;
		}

		// ---------------------------------------------------------------------
		// Intermediate parsing constructs

		private char Unicode()
		{
			int result = 0;
			for (int i = 0; i < 4; ++i)
			{
				if (AtEnd)
				{
					Fail("Unterminated Unicode escape");
				}
				char c = Pop();
				if (c >= '0' && c <= '9')
				{
					result = (16 * result) + (c - '0');
				}
				else if (c >= 'a' && c <= 'f')
				{
					result = (16 * result) + (c - 'a' + 10);
				}
				else if (c >= 'A' && c <= 'F')
				{
					result = (16 * result) + (c - 'A' + 10);
				}
				else
				{
					Fail($"Expected hex digit");
				}
			}
			return (char)result;
		}

		private int Sign()
		{
			if (Maybe('-')) return -1;
			if (Maybe('+')) return 1;
			return 1;
		}

		private long Decimal()
		{
			long result = 0;
			while (!AtEnd && Peek >= '0' && Peek <= '9')
				result = result * 10 + Pop() - '0';
			return result;
		}

		private void Expect(string expected)
		{
			foreach (char c in expected)
				Expect(c);
		}

		private void Expect(char expected)
		{
			if (!Maybe(expected))
				Fail($"Expected '{expected}'");
		}

		// ---------------------------------------------------------------------
		// Low-level parse functions

		private bool AtEnd => _i >= _max;
		private char Peek => _str[_i];

		private char Pop()
		{
			return _str[_i++];
		}

		private bool Maybe(char maybe)
		{
			if (_i >= _max) return false;
			if (_str[_i] != maybe) return false;
			++_i;
			return true;
		}

		private void Trim()
		{
			while (_i < _max && char.IsWhiteSpace(_str, _i))
				++_i;
		}

		private void Fail(string message)
		{
			throw new FormatException($"JSON parse failed at index {_i}: {message}");
		}

		// ---------------------------------------------------------------------
		// State

		private readonly string _str;
		private readonly int _max;
		private int _i;

	}
}
