using System.Net;
using System.Text;
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
			server = new SpikeServer(IPAddress.Any, tcpPort, 256);

			server.Start();

			server.OnMessageReceived += m => ConsoleLog.WriteMagenta($"{Environment.NewLine}{new string('-', 100)}{Environment.NewLine}{m}{Environment.NewLine}{new string('-', 100)}");
		}
		else
		{
			var client = new SpikeTcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), tcpPort));

			var longMessage = new StringBuilder();

			// for (var i = 0; i < 110; i++) longMessage.Append($"This will be a long message {i} | -=-=-=-=-=-=-=-=-=-=- |");

			for (var i = 0; i < 75000; i++)
			{
				await client.Send($"{i} || {i}{i}{i}{i}{i}{i}{i}{i}{i}{i}");

				// var delayTime = i % 3;
				//
				// if (i % 300 == 0) delayTime = 100;
				//
				// await Task.Delay(TimeSpan.FromMilliseconds(delayTime * 4));
			}

			// await client.Send(longMessage.ToString());

			consoleUtilities.Exit();
		}

		consoleUtilities.WaitForExit();

		server?.Dispose();
	}
}