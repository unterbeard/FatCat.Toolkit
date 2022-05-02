using System.Net;
using System.Net.Sockets;
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

public class SpikeTcpClient : IDisposable
{
	private readonly IPEndPoint endPoint;

	public SpikeTcpClient(IPEndPoint endPoint) => this.endPoint = endPoint;

	public void Dispose() { }

	public async Task Send(string message)
	{
		using var client = new TcpClient();

		await client.ConnectAsync(endPoint);

		await using var networkStream = client.GetStream();

		ConsoleLog.WriteMagenta("Sending ---------------");
		ConsoleLog.WriteMagenta(message);

		var messageBytes = Encoding.UTF8.GetBytes(message);

		await networkStream.WriteAsync(messageBytes);
		await networkStream.FlushAsync();
		networkStream.Close();

		ConsoleLog.WriteMagenta("-----------   Done Sending");

		client.Close();
	}
}

public class SpikeServer : IDisposable
{
	private readonly int bufferSize;
	private readonly CancellationTokenSource cancelSource;
	private readonly TcpListener listener;

	public SpikeServer(IPAddress ipAddress, ushort port, int bufferSize = 1024)
	{
		this.bufferSize = bufferSize;
		listener = new TcpListener(ipAddress, port);

		cancelSource = new CancellationTokenSource();
	}

	public void Dispose()
	{
		cancelSource.Dispose();
		listener.Stop();
	}

	public void Start()
	{
		listener.Start();

		ConsoleLog.WriteCyan($"Listening on <{listener.LocalEndpoint}>");

		listener.BeginAcceptTcpClient(TcpClientConnected, listener);
	}

	private void TcpClientConnected(IAsyncResult ar)
	{
		try
		{
			var syncListener = ar.AsyncState as TcpListener;

			var client = listener.EndAcceptTcpClient(ar);

			ConsoleLog.WriteCyan("Going to Listen for Connections");
			syncListener?.BeginAcceptTcpClient(TcpClientConnected, syncListener);

			ConsoleLog.WriteBlue($"Client Connected from {client.Client.RemoteEndPoint}");
			ConsoleLog.WriteBlue(". . . . . . . Connected!");

			var buffer = new byte[bufferSize];

			var stream = client.GetStream();

			if (stream.CanRead)
			{
				if (!stream.DataAvailable) ConsoleLog.WriteRed($"There is no data available from {client.Client.RemoteEndPoint}");

				while (stream.DataAvailable)
				{
					var dataRead = stream.Read(buffer, 0, buffer.Length);

					ConsoleLog.WriteCyan($". . . . . Read {dataRead} bytes");

					var data = Encoding.ASCII.GetString(buffer, 0, dataRead);

					ConsoleLog.WriteCyan($"{data}");
					ConsoleLog.WriteCyan(". . . . . Done read data");
				}
			}
			else ConsoleLog.WriteRed($"Cannot read the stream from <{client.Client.RemoteEndPoint}>");

			client.Close();
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}
}