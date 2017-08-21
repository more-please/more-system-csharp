﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace More.System
{
	public static class Objects
	{
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

		public static void Dump(this object obj)
		{
			Debug.WriteLine(obj.ToDebugString());
		}

		public static T NotNull<T>(this T obj) where T : class
		{
			if (obj == null) throw new NullReferenceException();
			return obj;
		}

		public static T NotNull<T>(this T obj, string message) where T : class
		{
			if (obj == null) throw new NullReferenceException(message);
			return obj;
		}
	}
}