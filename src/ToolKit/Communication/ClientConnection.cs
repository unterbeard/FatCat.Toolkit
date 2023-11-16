using System.Net.Sockets;

namespace FatCat.Toolkit.Communication;

internal abstract class ClientConnection
{
	protected readonly int bufferSize;
	protected readonly CancellationToken cancellationToken;
	protected readonly TcpClient client;
	protected readonly string clientId;
	private readonly IFatTcpLogger logger;
	protected readonly IFatTcpServer server;

	private bool IsNotCanceled
	{
		get => !cancellationToken.IsCancellationRequested;
	}

	protected ClientConnection(
		IFatTcpServer server,
		TcpClient client,
		string clientId,
		int bufferSize,
		IFatTcpLogger logger,
		CancellationToken cancellationToken
	)
	{
		this.server = server;
		this.client = client;
		this.clientId = clientId;
		this.bufferSize = bufferSize;
		this.logger = logger;
		this.cancellationToken = cancellationToken;
	}

	public void Start()
	{
		Task.Factory.StartNew(ReceivingThread, TaskCreationOptions.LongRunning);
	}

	protected abstract Task<Stream> GetStream();

	private async Task ReceivingThread()
	{
		var stream = await GetStream();
		var buffer = new byte[bufferSize];

		try
		{
			logger.WriteInformation($"Client Connected from {client.Client.RemoteEndPoint}");

			while (IsNotCanceled)
			{
				var bytesCount = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

				if (bytesCount == 0)
				{
					continue;
				}

				var bytesReceived = new byte[bytesCount];

				Array.Copy(buffer, bytesReceived, bytesCount);

				logger.WriteDebug($"Received {bytesCount} bytes from {clientId}");

				server.OnMessageReceived(bytesReceived);
			}
		}
		catch (IOException) { }
		catch (Exception ex)
		{
			logger.WriteException(ex);
		}
	}
}
