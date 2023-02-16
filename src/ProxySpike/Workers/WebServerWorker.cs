using FatCat.Toolkit.Console;
using ProxySpike.Options;

namespace ProxySpike.Workers;

public class WebServerWorker : ISpikeWorker<ServerOptions>
{
	public Task DoWork(ServerOptions options)
	{
		ConsoleLog.WriteCyan($"Web Server Worker on port <{options.WebPort}>");
		
		return Task.CompletedTask;
	}
}