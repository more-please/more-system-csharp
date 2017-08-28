using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace More.System
{
	//
	// Portable helpers for dealing with Tasks in tricky multithreaded scenarios.
	//
	// In an async method, you could suddenly find yourself on a different thread
	// after an "await". This is usually OK for the async method itself, but not
	// if you need to call some non-thread-safe code (e.g. do something on the
	// UI thread).
	//
	// I solve that problem as follows:
	//
	//		DoStuffAsync.ContinueHere(DoSomethingWithResult);
	//
	// Where "DoSomethingWithResult" is a method that handles the result of DoStuffAsync.
	// "ContinueHere" means that DoSomethingWithResult() will always run on the calling
	// thread, regardless of where the asynchronous call ends up.
	//
	// To make this work, you need to pump events in the calling thread. To do that,
	// just make this call in your update handler or similar:
	//
	//		Tasks.Continue();
	//
	// The standard way of accomplishing this is Task.ConfigureAwait():
	// https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.configureawait(v=vs.110).aspx
	// However, I find that approach more awkward and error-prone, and it doesn't
	// seem to work reliably on all platforms.
	//
	public static class Tasks
	{
		//
		// After the given task completes, run "action" on this thread.
		// The action will be invoked when Tasks.Continue() is called.
		//
		public static Task ContinueHere(this Task task, Action action)
		{
			action = action.NotNull();
			var actions = _actions;
			return task.ContinueWith((Task t) =>
			{
				actions.Enqueue(action);
			});
		}

		//
		// After the given task completes, call "action" with its result on this thread.
		// The action will be invoked when Tasks.Continue() is called.
		//
		public static Task ContinueHere<T>(this Task<T> task, Action<T> action)
		{
			action = action.NotNull();
			var actions = _actions;
			return task.ContinueWith((Task<T> t) =>
			{
				actions.Enqueue(() => action(t.Result));
			});
		}

		//
		// Perform the given action later on this thread, during Tasks.Continue()
		//
		public static void Post(Action action)
		{
			action = action.NotNull();
			_actions.Enqueue(action);
		}

		//
		// Invoke any pending ContinueHere() actions, in order.
		//
		public static void Continue()
		{
			Action action;
			while (_actions.TryDequeue(out action))
				action();
		}

		[ThreadStatic]
		private static readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
	}
}
