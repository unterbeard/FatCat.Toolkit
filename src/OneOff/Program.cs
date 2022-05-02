using System.Net;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;

namespace OneOff;

public static class Program
{
	private static SpikeServer server;

	public static async Task Main(params string[] args)
	{
		const int tcpPort = 62222;

		ConsoleLog.WriteBlue("Going to implement a TCP Client/Server");

		var consoleUtilities = new ConsoleUtilities(new ManualWaitEvent());

		if (args.Any(i => i.Contains("s")))
		{
			server = new SpikeServer(IPAddress.Any, tcpPort);

			server.Start();
		}
		else
		{
			var client = new SpikeTcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), tcpPort));

			// var longMessage = new StringBuilder();
			//
			// for (var i = 0; i < 25; i++) longMessage.Append($"This will be a long message {i} | -=-=-=-=-=-=-=-=-=-=- |");

			for (var i = 0; i < 3; i++) await client.Send($"{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}");

			// await client.Send(longMessage.ToString());

			consoleUtilities.Exit();
		}

		consoleUtilities.WaitForExit();

		server?.Dispose();
	}
}