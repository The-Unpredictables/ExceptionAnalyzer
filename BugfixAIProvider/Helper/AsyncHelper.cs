namespace BugfixAIProvider.Helper;

public static class AsyncHelper
{
	public static Task Loop(Action action, int interval, Func<bool> getBreakValue) =>
			Task.Run(() =>
					{
						while(!getBreakValue())
						{
							action();
							Thread.Sleep(interval);
						}
					});
}