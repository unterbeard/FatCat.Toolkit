using FatCat.Toolkit.Console;

namespace OneOff;

public class TcpWorker : SpikeWorker
{
	public override async Task DoWork()
	{
		ConsoleLog.Write("Going to play with TCP Stuff");

		await Task.CompletedTask;
	}
}
