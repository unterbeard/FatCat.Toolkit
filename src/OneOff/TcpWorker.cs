using System.Security.Cryptography.X509Certificates;
using System.Text;
using FatCat.Toolkit;
using FatCat.Toolkit.Communication;
using FatCat.Toolkit.Console;
using Humanizer;

namespace OneOff;

public class TcpWorker : SpikeWorker
{
	private const int TcpPort = 47899;
	private IFatTcpClient fatTcpClient;
	private readonly IGenerator generator;
	private IFatTcpServer fatTcpServer;
	private readonly ISimpleTcpSender tcpSender;

	public TcpWorker(ISimpleTcpSender tcpSender, IGenerator generator)
	{
		this.tcpSender = tcpSender;
		this.generator = generator;
	}

	public override async Task DoWork()
	{
		ConsoleLog.Write("Going to play with TCP Stuff");

		var cert = new X509Certificate2(@"C:\DevelopmentCert\DevelopmentCert.pfx", "basarab_cert");

		if (Program.Args.Any())
		{
			fatTcpServer = new SecureFatTcpServer(cert, generator);

			fatTcpServer.TcpMessageReceivedEvent += TcpClientMessage;

			fatTcpServer.Start(TcpPort, Encoding.UTF8);
		}
		else
		{
			fatTcpClient = new SecureFatTcpClient(cert);

			fatTcpClient.Reconnect = true;

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
