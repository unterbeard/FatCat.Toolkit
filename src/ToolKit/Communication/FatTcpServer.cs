using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FatCat.Toolkit.Communication;

public abstract class FatTcpServer
{
	private readonly IGenerator generator;
	protected readonly IFatTcpLogger logger;
	protected int bufferSize;
	private CancellationTokenSource cancelSource;
	protected CancellationToken cancelToken;
	private Encoding encoding;
	private TcpListener listener;
	private ushort port;
	private Socket server;

	private ConcurrentDictionary<string, ClientConnection> Connections { get; } = new();

	protected FatTcpServer(IGenerator generator, IFatTcpLogger logger)
	{
		this.generator = generator;
		this.logger = logger;
	}

	public event TcpMessageReceived TcpMessageReceivedEvent;

	public void Dispose() { }

	public virtual void OnMessageReceived(byte[] data) { TcpMessageReceivedEvent?.Invoke(data); }

	public void Start(ushort port, int receiveBufferSize = 1024) { Start(port, Encoding.Unicode, receiveBufferSize); }

	public void Start(ushort port, Encoding encoding, int receiveBufferSize = 1024)
	{
		cancelSource = new CancellationTokenSource();
		cancelToken = cancelSource.Token;

		this.port = port;
		bufferSize = receiveBufferSize;
		this.encoding = encoding;

		Task.Factory.StartNew(ServerThread, TaskCreationOptions.LongRunning);
	}

	public void Stop() { Dispose(); }

	internal abstract ClientConnection GetClientConnection(TcpClient client, string clientId);

	private async Task ServerThread()
	{
		await Task.CompletedTask;

		listener = new TcpListener(IPAddress.Any, port) { Server = { NoDelay = true } };
		server = listener.Server;

		SetUpKeepAlive();

		listener.Start();

		logger.WriteDebug($"Server listening on {port}");

		while (!cancelToken.IsCancellationRequested)
		{
			var client = await listener.AcceptTcpClientAsync(cancelToken);

			var clientId = generator.NewId();

			var clientConnection = GetClientConnection(client, clientId);

			Connections.TryAdd(clientId, clientConnection);

			clientConnection.Start();
		}
	}

	private void SetUpKeepAlive()
	{
		server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
		server.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 900);
		server.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 300);
		server.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 15);
	}
}