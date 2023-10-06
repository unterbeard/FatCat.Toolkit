using System.Net;
using System.Text;
using FatCat.Toolkit.Communication;
using FatCat.Toolkit.Console;

namespace OneOff;

public class TcpWorker : SpikeWorker
{
	private const int TcpPort = 47899;
	private readonly ITcpServer tcpServer;
	private readonly ISimpleTcpSender tcpSender;

	public TcpWorker(ITcpServer tcpServer, ISimpleTcpSender tcpSender)
	{
		this.tcpServer = tcpServer;
		this.tcpSender = tcpSender;
	}

	public override async Task DoWork()
	{
		ConsoleLog.Write("Going to play with TCP Stuff");

		await Task.CompletedTask;

		if (Program.Args.Any())
		{
			tcpServer.OnMessageReceived += OnClientMessage;

			tcpServer.Start(TcpPort, Encoding.UTF8);
		}
		else
		{
			var endPoint = new IPEndPoint(IPAddress.Loopback, TcpPort);

			await tcpSender.Send(endPoint, "Hello World", Encoding.UTF8);
		}
	}

	private void OnClientMessage(byte[] data)
	{
		var message = Encoding.UTF8.GetString(data);

		ConsoleLog.WriteCyan($"Message from client := <{message}>");
	}
}
