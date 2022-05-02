﻿using System.Net;
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

			await client.Send("This is my first test message");

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

		var messageBytes = Encoding.UTF8.GetBytes(message);

		await networkStream.WriteAsync(messageBytes);
	}
}

public class SpikeServer : IDisposable
{
	private readonly CancellationTokenSource cancelSource;
	private readonly TcpListener listener;

	public SpikeServer(IPAddress ipAddress, ushort port)
	{
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
		var syncListener = ar.AsyncState as TcpListener;

		var client = listener.EndAcceptTcpClient(ar);

		ConsoleLog.WriteBlue($"Client Connected from {client.Client.RemoteEndPoint}");
		ConsoleLog.WriteBlue(". . . . . . . Connected!");

		syncListener?.BeginAcceptTcpClient(TcpClientConnected, syncListener);

		var bytes = new byte[1024];

		var stream = client.GetStream();

		if (stream.CanRead)
		{
			while (stream.DataAvailable)
			{
				var dataRead = stream.Read(bytes, 0, bytes.Length);

				ConsoleLog.WriteCyan($". . . . . Read {dataRead} bytes");

				var data = Encoding.ASCII.GetString(bytes, 0, dataRead);

				ConsoleLog.WriteCyan($"{data}");
				ConsoleLog.WriteCyan(". . . . . Done read data");
			}
		}

		client.Close();
	}
}