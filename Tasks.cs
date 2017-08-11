using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Utils
{
	public static class Tasks
	{
		public static Task ContinueHere(this Task task, Action action)
		{
			var actions = _actions;
			return task.ContinueWith((Task t) =>
			{
				actions.Enqueue(() => action());
			});
		}

		public static Task ContinueHere<T>(this Task<T> task, Action<T> action)
		{
			var actions = _actions;
			return task.ContinueWith((Task<T> t) =>
			{
				actions.Enqueue(() => action(t.Result));
			});
		}

		public static void Continue()
		{
			Action action;
			while (_actions.TryDequeue(out action))
			{
				action();
			}
		}

		[ThreadStatic]
		private static readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
	}
}
