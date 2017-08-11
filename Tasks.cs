using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;

namespace Utils
{
	public static class Tasks
	{
		private static readonly ConcurrentQueue<Action> _workQueue = new ConcurrentQueue<Action>();

		public static Task ContinueHere(this Task task, Action action)
		{
			return task.ContinueWith((Task t) =>
			{
				_workQueue.Enqueue(() => action());
			});
		}

		public static Task ContinueHere<T>(this Task<T> task, Action<T> action)
		{
			return task.ContinueWith((Task<T> t) =>
			{
				_workQueue.Enqueue(() => action(t.Result));
			});
		}

		public static void Continue()
		{
			Action action;
			while (_workQueue.TryDequeue(out action))
			{
				action();
			}
		}
	}
}
