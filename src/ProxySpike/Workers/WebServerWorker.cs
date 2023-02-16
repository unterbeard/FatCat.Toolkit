using FatCat.Toolkit.Console;

namespace ProxySpike.Workers;

public class WebServerWorker : ISpikeWorker
{
	public Task DoWork()
	{
		ConsoleLog.WriteCyan("Web Server Worker");
		
		return Task.CompletedTask;
	}
}