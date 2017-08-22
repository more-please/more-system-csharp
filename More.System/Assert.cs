using System.Diagnostics;
using System;

namespace More.System
{
	public static class Assert
	{
		//
		// The standard System.Diagnostics.Debug.Assert doesn't seem to reliably break
		// into the debugger! It just logs a warning and continues. I want assertions
		// to break into the debugger pretty much every time, so I use this wrapper.
		//
		public static void That(bool condition, string message = "Assertion failed!")
		{
			Debug.Assert(condition, message);
			if (!condition)
			{
				Debugger.Break();
			}
		}

		//
		// Assert that obj is not null. Returns obj so you can chain the call.
		//
		public static T NotNull<T>(this T obj) where T : class
		{
			if (obj == null) throw new NullReferenceException();
			return obj;
		}

		//
		// Assert that obj is not null. Returns obj so you can chain the call.
		//
		public static T NotNull<T>(this T obj, string message) where T : class
		{
			if (obj == null) throw new NullReferenceException(message);
			return obj;
		}
	}
}
