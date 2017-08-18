﻿using System;

namespace More.System
{
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

		public static float Uniform(this Random random, float lo, float hi)
		{
			return lo + (float)random.NextDouble() * (hi - lo);
		}

		public static int Uniform(this Random random, int lo, int hi)
		{
			return random.Next(lo, hi);
		}
	}
}
