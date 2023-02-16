using FatCat.Toolkit.Console;

namespace ProxySpike.Client;

public class ClientWorker : ISpikeWorker
{
	public Task DoWork()
	{
		ConsoleLog.WriteCyan("Client Worker");
		
		return Task.CompletedTask;
	}
}