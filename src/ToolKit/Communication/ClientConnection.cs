using System.Net.Sockets;
using FatCat.Toolkit.Console;

namespace FatCat.Toolkit.Communication;

internal class ClientConnection
{
	private readonly FatFatTcpServer server;
	private readonly TcpClient client;
	private readonly string clientId;
	private readonly int bufferSize;
	private readonly CancellationToken cancellationToken;

	public ClientConnection(
		FatFatTcpServer server,
		TcpClient client,
		string clientId,
		int bufferSize,
		CancellationToken cancellationToken
	)
	{
		this.server = server;
		this.client = client;
		this.clientId = clientId;
		this.bufferSize = bufferSize;
		this.cancellationToken = cancellationToken;
	}

	public void Start()
	{
		Task.Factory.StartNew(ReceivingThread, TaskCreationOptions.LongRunning);
	}

	private async Task ReceivingThread()
	{
		// ConsoleLog.WriteMagenta($"Client Connected RemoveEndpoint <{client.Client.RemoteEndPoint}>");

		var stream = client.GetStream();
		var buffer = new byte[bufferSize];

		try
		{
			var bytesCount = 0;

			while (
				!cancellationToken.IsCancellationRequested
				&& (bytesCount = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) != 0
			)
			{
				var bytesReceived = new byte[bytesCount];

				Array.Copy(buffer, bytesReceived, bytesCount);

				// ConsoleLog.WriteMagenta(
				// 	$"Message Received From <{client.Client.RemoteEndPoint}> | Total Bytes <{bytesCount}>"
				// );

				server.OnMessageReceived(bytesReceived);
			}
		}
		catch (IOException) { }
		catch
		{ // ignored
		}
	}
}
