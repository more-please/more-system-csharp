using System.Collections.Concurrent;
using System;
using System.Threading.Tasks;

namespace Utils
{
	public static class Actions
	{
		private static readonly ConcurrentQueue<Action> _workQueue = new ConcurrentQueue<Action>();

		public static Action<Task> ToMainThread(this Action action)
		{
			return delegate (Task task)
			{
				_workQueue.Enqueue(() => action());
			};
		}

		public static Action<Task<T>> ToMainThread<T>(this Action<T> action)
		{
			return delegate (Task<T> task)
			{
				_workQueue.Enqueue(() => action(task.Result));
			};
		}

		public static void RunMainThreadActions()
		{
			Action action;
			while (_workQueue.TryDequeue(out action))
			{
				action();
			}
		}
	}
}
