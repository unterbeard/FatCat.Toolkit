using System.Text;
using FatCat.Toolkit.Communication;
using FatCat.Toolkit.Console;
using Humanizer;

namespace OneOff;

public class TcpWorker : SpikeWorker
{
	private const int TcpPort = 47899;
	private readonly IFatTcpClient fatTcpClient;
	private readonly IFatTcpServer fatTcpServer;
	private readonly ISimpleTcpSender tcpSender;

	public TcpWorker(IFatTcpServer fatTcpServer, ISimpleTcpSender tcpSender, IFatTcpClient fatTcpClient)
	{
		this.fatTcpServer = fatTcpServer;
		this.tcpSender = tcpSender;
		this.fatTcpClient = fatTcpClient;
	}

	public override async Task DoWork()
	{
		ConsoleLog.Write("Going to play with TCP Stuff");

		if (Program.Args.Any())
		{
			fatTcpServer.TcpMessageReceivedEvent += TcpClientMessage;

			fatTcpServer.Start(TcpPort, Encoding.UTF8);
		}
		else
		{
			await fatTcpClient.Connect("127.0.0.1", TcpPort);

			for (var i = 0; i < 8500000; i++)
			{
				var message = $"{i:X} This is a	message | {DateTime.Now:T}";

				var bytes = Encoding.UTF8.GetBytes(message);

				await fatTcpClient.Send(bytes);

				await Task.Delay(1.Milliseconds());
			}
		}
	}

	private void TcpClientMessage(byte[] data)
	{
		var message = Encoding.UTF8.GetString(data);

		ConsoleLog.WriteCyan($"Message from client := <{message}>");
	}
}
