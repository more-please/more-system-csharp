using System;
using System.Threading;
using System.Threading.Tasks;

namespace More.System
{
	public static class SemaphoreSlims
	{
		//
		// Wait on the given semaphore, and return an IDisposable that releases it.
		// This allows you to use a SemaphoreSlim as an async-friendly lock, like this:
		//
		//		mySemaphore = new SemaphoreSlim(1, 1);
		//
		//		async Task MyAsyncMethod()
		//		{
		//			using (var _ = await mySemaphore.DisposableWaitAsync())
		//				...
		//		}
		//
		// Note that this isn't a recursive lock. Trying to lock it twice will deadlock.
		//
		public static async Task<IDisposable> DisposableWaitAsync(this SemaphoreSlim sem)
		{
			await sem.WaitAsync();
			return new Disposable(() => sem.Release());
		}
	}
}
