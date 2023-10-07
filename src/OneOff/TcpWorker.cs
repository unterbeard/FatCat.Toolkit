using System.Net;
using System.Text;
using FatCat.Toolkit.Communication;
using FatCat.Toolkit.Console;
using Humanizer;

namespace OneOff;

public class TcpWorker : SpikeWorker
{
	private const int TcpPort = 47899;
	private readonly ISimpleTcpSender tcpSender;
	private readonly IFatTcpServer fatTcpServer;

	public TcpWorker(IFatTcpServer fatTcpServer, ISimpleTcpSender tcpSender)
	{
		this.fatTcpServer = fatTcpServer;
		this.tcpSender = tcpSender;
	}

	public override async Task DoWork()
	{
		ConsoleLog.Write("Going to play with TCP Stuff");

		await Task.CompletedTask;

		if (Program.Args.Any())
		{
			fatTcpServer.OnMessageReceived += OnClientMessage;

			fatTcpServer.Start(TcpPort, Encoding.UTF8);
		}
		else
		{
			var endPoint = new IPEndPoint(IPAddress.Loopback, TcpPort);

			for (var i = 0; i < 850000; i++)
			{
				var message = $"{i:X} This is a	message | {DateTime.Now:T}";

				await tcpSender.Send(endPoint, message, Encoding.UTF8);

				await Task.Delay(10.Milliseconds());
			}
		}
	}

	private void OnClientMessage(byte[] data)
	{
		var message = Encoding.UTF8.GetString(data);

		ConsoleLog.WriteCyan($"Message from client := <{message}>");
	}
}