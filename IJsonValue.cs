using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Utils
{
	//
	// Interface for classes that can serialize themselves to JSON
	//
	public interface IJsonValue
	{
		// Serialize to a JSON object (dictionary, array, string or number)
		object ToJsonValue();
	}

	//
	// Implementation for collections of IJsonValues
	//
	public static class IJsonValues
	{
		public static IEnumerable<object> ToJsonValue<T>(this IEnumerable<T> list) where T : IJsonValue
		{
			var builder = ImmutableList.CreateBuilder<object>();
			foreach (var t in list)
			{
				builder.Add(t.ToJsonValue());
			}
			return builder.ToImmutable();
		}

		public static IDictionary<string, object> ToJsonValue<T>(this IDictionary<string, T> dict) where T : IJsonValue
		{
			var builder = ImmutableSortedDictionary.CreateBuilder<string, object>();
			foreach (var t in dict)
			{
				builder.Add(t.Key, t.Value.ToJsonValue());
			}
			return builder.ToImmutable();
		}

		public static IDictionary<string, object> AsJsonDict(this object obj)
		{
			return (IDictionary<string, object>)obj;
		}

		public static ICollection<object> AsJsonArray(this object obj)
		{
			return (ICollection<object>)obj;
		}

		public static IDictionary<string, T> AsJsonDict<T>(this object obj, Func<object, T> func)
		{
			var dict = obj.AsJsonDict();
			var result = new Dictionary<string, T>(dict.Count);
			foreach (var e in obj as IDictionary<string, object>)
				result.Add(e.Key, func(e.Value));
			return result;
		}

		public static IList<T> AsJsonArray<T>(this object obj, Func<object, T> func)
		{
			var array = obj.AsJsonArray();
			var result = new List<T>(array.Count);
			foreach (var t in array)
				result.Add(func(t));
			return result;
		}
	}
}
