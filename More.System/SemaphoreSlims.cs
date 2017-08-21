using System;
using System.Threading;
using System.Threading.Tasks;

namespace More.System
{
	public static class SemaphoreSlims
	{
		public static async Task<IDisposable> DisposableWaitAsync(this SemaphoreSlim sem)
		{
			await sem.WaitAsync();
			return new Disposable(() => sem.Release());
		}
	}
}
