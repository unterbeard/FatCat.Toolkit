using FatCat.Toolkit.Threading;

namespace FatCat.Toolkit.Testing;

public class FakeThread : IThread
{
	public void Run(Action action) => action();

	public void Run(Func<Task> action) => action().Wait();

	public Task Sleep(TimeSpan sleepTime) => Task.CompletedTask;

	public Task Sleep(TimeSpan sleepTime, CancellationToken token) => Task.CompletedTask;
}