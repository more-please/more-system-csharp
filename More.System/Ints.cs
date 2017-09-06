using System;
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

		public static float Float(this int n)
		{
			return (float)n;
		}

		public static int Clamp(this int i, int min, int max)
		{
			return Math.Min(max, Math.Max(min, i));
		}
	}
}
