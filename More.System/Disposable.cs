using System;
using System.Threading;

namespace More.System
{
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
