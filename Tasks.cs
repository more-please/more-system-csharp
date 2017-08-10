using System.Threading.Tasks;
using System;

namespace Utils
{
	public static class Tasks
	{
		public static Task ReturnToMain(this Task task, Action action)
		{
			return task.ContinueWith(action.ToMainThread());
		}

		public static Task ReturnToMain<T>(this Task<T> task, Action<T> action)
		{
			return task.ContinueWith(action.ToMainThread<T>());
		}
	}
}
