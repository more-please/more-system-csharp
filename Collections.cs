using System.Collections;
using System.Collections.Generic;
using System;

namespace Utils
{
	public static class Collections
	{
		public static bool IsEmpty<T>(this ICollection<T> collection)
		{
			return collection.Count == 0;
		}

		// Bizarrely, Queue<T> implements ICollection but not ICollection<T>.
		// We can't overload on ICollection because many collections also
		// implement ICollection<T>, IList<T> etc, so the overload would be
		// ambiguous. So just treat Queue as a special case.
		public static bool IsEmpty<T>(this Queue<T> collection)
		{
			return collection.Count == 0;
		}

		public static ICollection<B> Map<A,B>(this ICollection<A> collection, Func<A, B> func)
		{
			return new CollectionMap<A, B>(collection, func);
		}
	}
}
