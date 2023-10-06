using System.Net.Sockets;
using FatCat.Toolkit.Console;

namespace FatCat.Toolkit.Communication;

internal class ClientConnection
{
	private readonly TcpServer server;
	private readonly TcpClient client;
	private readonly string clientId;
	private readonly int bufferSize;
	private readonly CancellationToken cancellationToken;

	public ClientConnection(
		TcpServer server,
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
		await Task.CompletedTask;

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

				server.OnOnMessageReceived(bytesReceived);
			}
		}
		catch (IOException)
		{
			ConsoleLog.WriteCyan("IOException");
		}
		catch (Exception e)
		{
			ConsoleLog.WriteException(e);
		}
	}
}
