namespace More.System
{
	public static class Ints
	{
		//
		// Handy for picking the Nth item from a const array, for example:
		//
		//		i.Index("zero", "one", "two")
		//
		public static T Index<T>(this int i, params T[] arr)
		{
			return arr[i];
		}
	}
}
