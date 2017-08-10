using System.Collections;
using System.Collections.Generic;
using System;

namespace Utils
{
	public static class Collections
	{
		public static bool IsEmpty(this ICollection collection)
		{
			return collection.Count == 0;
		}

		public static ICollection<B> Map<A,B>(this ICollection<A> collection, Func<A, B> func)
		{
			return new CollectionMap<A, B>(collection, func);
		}
	}
}
