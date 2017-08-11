using System.Collections;
using System.Collections.Generic;

namespace Utils
{
	using Entry = KeyValuePair<string, object>;

	public class JsonDict : IDictionary<string, object>
	{
		public bool IsReadOnly => false;
		public ICollection<string> Keys => _entries.Map(Key);
		public ICollection<object> Values => _entries.Map(Value);
		public int Count => _entries.Count;

		public void Add<T>(string key, IEnumerable<T> val) where T: IJsonValue
		{
			_entries.Add(new Entry(key, val.ToJsonValue()));
		}

		public void Add<T>(string key, IDictionary<string, T> val) where T : IJsonValue
		{
			_entries.Add(new Entry(key, val.ToJsonValue()));
		}

		public void Add(string key, IDictionary<string, object> val)
		{
			_entries.Add(new Entry(key, val));
		}

		public void Add(string key, IJsonValue val)
		{
			_entries.Add(new Entry(key, val.ToJsonValue()));
		}

		public void Add(string key, object val)
		{
			if (val is IJsonValue)
				Add(key, val as IJsonValue);
			else
				Add(new Entry(key, val));
		}

		public void Add(Entry e)
		{
			if (e.Value is IJsonValue)
				Add(e.Key, e.Value as IJsonValue);
			else
				_entries.Add(e);
		}

		public bool Remove(Entry e) { return _entries.Remove(e); }
		public bool Contains(Entry e) { return _entries.Contains(e); }
		public void Clear() { _entries.Clear(); }
		public void CopyTo(Entry[] array, int i) { _entries.CopyTo(array, i); }

		IEnumerator<Entry> IEnumerable<Entry>.GetEnumerator() { return _entries.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return _entries.GetEnumerator(); }

		public object this[string key]
		{
			get
			{
				foreach (var e in _entries)
					if (key.Equals(e.Key))
						return e.Value;
				return null;
			}
			set => Add(key, value);
		}

		public bool TryGetValue(string key, out object val)
		{
			foreach (var e in _entries)
				if (key.Equals(e.Key))
				{
					val = e.Value;
					return true;
				}
			val = null;
			return false;
		}

		public bool ContainsKey(string key)
		{
			foreach (var e in _entries)
				if (key.Equals(e.Key))
					return true;
			return false;
		}

		public bool Remove(string key)
		{
			for (int i = 0; i < _entries.Count; ++i)
				if (key.Equals(_entries[i].Key))
				{
					_entries.RemoveAt(i);
					return true;
				}
			return false;
		}

		// ---------------------------------------------------------------------

		private readonly List<Entry> _entries = new List<Entry>();

		private static string Key(Entry e) { return e.Key; }
		private static object Value(Entry e) { return e.Value; }
	}
}
