using System;
using System.Threading;

namespace More.System
{
	//
	// Turns an Action into an IDisposable, so you can use it in a using() block.
	//
	public class Disposable : IDisposable
	{
		private readonly Action _action;
		private int _count = 0;

		public Disposable(Action action)
		{
			_action = action.NotNull();
		}

		public void Dispose()
		{
			if (Interlocked.Increment(ref _count) > 1)
				throw new ObjectDisposedException("More.System.Disposable");
			_action();
		}
	}
}
