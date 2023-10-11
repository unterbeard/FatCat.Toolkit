using System.Net.Sockets;

namespace FatCat.Toolkit.Communication;

internal abstract class ClientConnection
{
	protected readonly int bufferSize;
	protected readonly CancellationToken cancellationToken;
	protected readonly TcpClient client;
	protected readonly string clientId;
	protected readonly IFatTcpServer server;

	private bool IsNotCanceled => !cancellationToken.IsCancellationRequested;

	protected ClientConnection(
		IFatTcpServer server,
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

	protected abstract Stream GetStream();

	private async Task ReceivingThread()
	{
		var stream = GetStream();
		var buffer = new byte[bufferSize];

		try
		{
			while (IsNotCanceled)
			{
				var bytesCount = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

				if (bytesCount == 0)
				{
					continue;
				}

				var bytesReceived = new byte[bytesCount];

				Array.Copy(buffer, bytesReceived, bytesCount);

				server.OnMessageReceived(bytesReceived);
			}
		}
		catch (IOException) { }
		catch
		{ // ignored
		}
	}
}
