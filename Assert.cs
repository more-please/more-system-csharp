﻿using System.Diagnostics;

namespace Utils
{
	public static class Assert
	{
		public static void That(bool condition, string message = "Assertion failed!")
		{
			Debug.Assert(condition, message);
			if (!condition)
			{
				Debugger.Break();
			}
		}
	}
}
