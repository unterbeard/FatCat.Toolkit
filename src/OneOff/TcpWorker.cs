using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FatCat.Toolkit;
using FatCat.Toolkit.Communication;
using FatCat.Toolkit.Console;
using Humanizer;

namespace OneOff;

public class TcpWorker : SpikeWorker
{
	private const int TcpPort = 24329;
	private readonly IGenerator generator;
	private readonly ISimpleTcpSender tcpSender;
	private IFatTcpClient fatTcpClient;
	private IFatTcpServer fatTcpServer;

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
			fatTcpClient = new SecureFatTcpClient(cert, new ConsoleFatTcpLogger());

			fatTcpClient.Reconnect = true;

			var host = Program.Args.FirstOrDefault();

			var hostEntry = await Dns.GetHostEntryAsync(host);

			var ip = hostEntry.AddressList.FirstOrDefault();

			var ipString = ip.ToString();

			ConsoleLog.WriteBlue($"{ipString}");

			if (ip.ToString() == "::1")
			{
				ipString = "127.0.0.1";
			}

			await fatTcpClient.Connect(ipString, TcpPort);

			for (var i = 0; i < 8500000; i++)
			{
				var message = $"{i:X} This is a	message | {DateTime.Now:T}";

				var bytes = Encoding.UTF8.GetBytes(message);

				await fatTcpClient.Send(bytes);

				await Task.Delay(1.Milliseconds());
			}
		}
		else
		{
			StartServer(cert);
		}
	}

	private void StartServer(X509Certificate2 cert)
	{
		fatTcpServer = new SecureFatTcpServer(cert, generator, new ConsoleFatTcpLogger());

		fatTcpServer.TcpMessageReceivedEvent += TcpClientMessage;

		fatTcpServer.Start(TcpPort, Encoding.UTF8);
	}

	private void TcpClientMessage(byte[] data)
	{
		var message = Encoding.UTF8.GetString(data);

		ConsoleLog.WriteCyan($"Message from client := <{message}>");
	}
}
