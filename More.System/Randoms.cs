using System;

namespace More.System
{
	//
	// Helper functions for random numbers.
	// Holds a private Random instance so you don't need to.
	//
	public static class Randoms
	{
		private static Random _random = new Random();

		public static void SetSeed(int seed)
		{
			_random = new Random(seed);
		}

		public static float Uniform(float lo, float hi)
		{
			return _random.Uniform(lo, hi);
		}

		public static int Uniform(int lo, int hi)
		{
			return _random.Uniform(lo, hi);
		}

		public static int Weight(params float[] weights)
		{
			return _random.Weight(weights);
		}

		public static T Choose<T>(params T[] options)
		{
			return _random.Choose(options);
		}

		public static float Uniform(this Random random, float lo, float hi)
		{
			return lo + (float)random.NextDouble() * (hi - lo);
		}

		public static int Uniform(this Random random, int lo, int hi)
		{
			return random.Next(lo, hi);
		}

		public static T Choose<T>(this Random random, params T[] options)
		{
			return options[random.Next(0, options.Length)];
		}

		public static int Weight(this Random random, params float[] weights)
		{
			float total = 0;
			foreach (var w in weights)
				total += w;

			float t = random.Uniform(0, total);
			for (int i = 0; i < weights.Length; ++i)
			{
				t -= weights[i];
				if (t < 0) return i;
			}
			return weights.Length - 1;
		}
	}
}
