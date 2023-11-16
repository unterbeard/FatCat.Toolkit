using FatCat.Toolkit.Logging;

namespace FatCat.Toolkit.Threading;

public interface IThread
{
	void Run(Action action);

	void Run(Func<Task> action);

	Task Sleep(TimeSpan sleepTime);

	Task Sleep(TimeSpan sleepTime, CancellationToken token);
}

public class Thread : IThread
{
	private readonly IToolkitLogger logger;

	public Thread(IToolkitLogger logger) => this.logger = logger;

	public void Run(Func<Task> action)
	{
		Task.Run(async () =>
		{
			try
			{
				await action();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		});
	}

	public void Run(Action action)
	{
		Task.Run(() =>
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		});
	}

	public Task Sleep(TimeSpan sleepTime) => Task.Delay(sleepTime);

	public Task Sleep(TimeSpan sleepTime, CancellationToken token) => Task.Delay(sleepTime, token);

	private void HandleException(Exception ex)
	{
		logger.Error("Exception running background task.");

		logger.Exception(ex);
	}
}
