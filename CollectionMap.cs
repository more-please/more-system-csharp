using System;
using System.Collections;
using System.Collections.Generic;

namespace More.System
{
	public class CollectionMap<A, B> : ICollection<B>, IReadOnlyCollection<B>
	{
		private readonly ICollection<A> _collection;
		private readonly Func<A, B> _func;

		public CollectionMap(ICollection<A> collection, Func<A, B> func)
		{
			_collection = collection;
			_func = func;
		}

		public int Count => _collection.Count;
		public bool IsReadOnly => true;

		IEnumerator<B> IEnumerable<B>.GetEnumerator()
		{
			foreach (var a in _collection)
				yield return _func(a);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (var a in _collection)
				yield return _func(a);
		}

		public bool Contains(B item)
		{
			foreach (var a in _collection)
				if (_func(a).Equals(item))
					return true;
			return false;
		}

		public void CopyTo(B[] array, int i)
		{
			foreach (var a in _collection)
				array[i++] = _func(a);
		}

		public void Add(B item)
		{
			throw new NotSupportedException();
		}

		public bool Remove(B item)
		{
			throw new NotSupportedException();
		}

		public void Clear()
		{
			throw new NotSupportedException();
		}
	}
}
