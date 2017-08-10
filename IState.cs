using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Utils
{
	//
	// Interface implemented by classes that can serialize themselves to JSON
	//
	public interface IState
	{
		// Serialize to a JSON object (dictionary, array, string or number)
		object ToObject();
	}

	//
	// Implementation of IState that doesn't do any serialization, just returns the object
	//
	public class SimpleState : IState
	{
		public object ToObject()
		{
			throw new NotImplementedException();
		}
	}

	//
	// Pseudo-implementation of IState for collections
	//
	public static class IStates
	{
		public static IEnumerable<object> ToObject<T>(this IEnumerable<T> list) where T : IState
		{
			var builder = ImmutableList.CreateBuilder<object>();
			foreach (var t in list)
			{
				builder.Add(t.ToObject());
			}
			return builder.ToImmutable();
		}

		public static IDictionary<string, object> ToObject<T>(this IDictionary<string, T> dict) where T : IState
		{
			var builder = ImmutableSortedDictionary.CreateBuilder<string, object>();
			foreach (var t in dict)
			{
				builder.Add(t.Key, t.Value.ToObject());
			}
			return builder.ToImmutable();
		}
	}
}
