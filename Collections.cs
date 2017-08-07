using System.Collections;

namespace Utils
{
	public static class Collections
	{
		public static bool IsEmpty(this ICollection collection)
		{
			return collection.Count == 0;
		}
	}
}
