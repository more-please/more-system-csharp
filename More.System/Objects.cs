using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace More.System
{
	public static class Objects
	{
		//
		// Generate a reasonable ToString for structs, via reflection.
		//
		public static string ToDebugString<T>(this T obj)
		{
			StringBuilder s = new StringBuilder();
			Type t = typeof(T);
			s.Append(t.Name);
			foreach (PropertyInfo p in t.GetRuntimeProperties())
			{
				s.Append(' ');
				s.Append(p.Name);
				s.Append('=');
				s.Append(p.GetValue(obj));
			}
			return s.ToString();
		}

		//
		// Print obj.ToDebugString to the debug console.
		//
		public static void Dump(this object obj)
		{
			Debug.WriteLine(obj.ToDebugString());
		}
	}
}