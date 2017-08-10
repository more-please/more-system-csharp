using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System;

namespace Utils
{
	public static class JsonWriter
	{
		public static string Write(object obj)
		{
			try
			{
				StringBuilder b = new StringBuilder();
				Write(obj, b);
				return b.ToString();
			}
			catch (ArgumentException)
			{
				// TODO: log error somewhere?
				return null;
			}
		}

		public static void Write(object obj, StringBuilder output)
		{
			if (obj == null)
			{
				output.Append("null");
			}
			else if (obj is string)
			{
				WriteString(obj as string, output);
			}
			else if (obj is Int16 || obj is Int32 || obj is Int64 || obj is Enum)
			{
				WriteInt(Convert.ToInt64(obj), output);
			}
			else if (obj is Single || obj is Double || obj is Decimal)
			{
				WriteFloat(Convert.ToDouble(obj), output);
			}
			else if (obj is bool)
			{
				output.Append((bool)obj ? "true" : "false");
			}
			else if (obj is IDictionary)
			{
				WriteDict(obj as IDictionary, output);
			}
			else if (obj is IEnumerable<KeyValuePair<string, object>>)
			{
				WriteDict(obj as IEnumerable<KeyValuePair<string, object>>, output);
			}
			else if (obj is IEnumerable)
			{
				WriteArray(obj as IEnumerable, output);
			}
			else
			{
				throw new ArgumentException($"Failed to write JSON from object: ${obj}");
			}
		}

		public static void WriteInt(long i, StringBuilder output)
		{
			output.Append(i.ToString("D", CultureInfo.InvariantCulture));
		}

		public static void WriteFloat(double f, StringBuilder output)
		{
			output.Append(f.ToString("R", CultureInfo.InvariantCulture));
		}

		private const string _hexDigits = "0123456789ABCDEF";

		public static void WriteString(string s, StringBuilder output)
		{
			output.Append('"');
			foreach (char c in s)
			{
				if (c == '\\')
				{
					output.Append("\\\\");
				}
				else if (c == '"')
				{
					output.Append("\\\"");
				}
				else if (c < 32)
				{
					output.Append("\\u00");
					output.Append(_hexDigits[c / 16]);
					output.Append(_hexDigits[c % 16]);
				}
				else
				{
					output.Append(c);
				}
			}
			output.Append('"');
		}

		public static void WriteDict(IDictionary dict, StringBuilder output)
		{
			output.Append('{');
			var e = dict.GetEnumerator();
			if (e.MoveNext())
			{
				WriteString(e.Key.ToString(), output);
				output.Append(':');
				Write(e.Value, output);
				while (e.MoveNext())
				{
					output.Append(',');
					WriteString(e.Key.ToString(), output);
					output.Append(':');
					Write(e.Value, output);
				}
			}
			output.Append('}');
		}

		public static void WriteDict(IEnumerable<KeyValuePair<string, object>> dict, StringBuilder output)
		{
			output.Append('{');
			var e = dict.GetEnumerator();
			if (e.MoveNext())
			{
				WriteString(e.Current.Key as string, output);
				output.Append(':');
				Write(e.Current.Value, output);
				while (e.MoveNext())
				{
					output.Append(',');
					WriteString(e.Current.Key as string, output);
					output.Append(':');
					Write(e.Current.Value, output);
				}
			}
			output.Append('}');
		}

		public static void WriteArray(IEnumerable array, StringBuilder output)
		{
			output.Append('[');
			var e = array.GetEnumerator();
			if (e.MoveNext())
			{
				Write(e.Current, output);
				while (e.MoveNext())
				{
					output.Append(',');
					Write(e.Current, output);
				}
			}
			output.Append(']');
		}
	}
}
