using FatCat.Toolkit.Console;
using ProxySpike.Options;

namespace ProxySpike.Workers;

public class ProxyWorker : ISpikeWorker<ProxyOptions>
{
	public Task DoWork(ProxyOptions options)
	{
		ConsoleLog.WriteDarkCyan("Starting the proxy worker");

		return Task.CompletedTask;
	}
}