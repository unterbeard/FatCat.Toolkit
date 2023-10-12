using System.Net;
using System.Net.Sockets;
using System.Text;
using FatCat.Toolkit.Console;
using Humanizer;

namespace FatCat.Toolkit.Communication;

public abstract class FatTcpClient
{
	private byte[] buffer;
	private int bufferSize;
	private CancellationTokenSource cancelSource;
	private CancellationToken cancelToken;
	private string host;
	private IPEndPoint ipEndpoint;
	private ushort port;
	private IPAddress serverIp;
	private Stream stream;
	protected TcpClient tcpClient;

	public bool Connected { get; private set; }

	public bool Reconnect { get; set; } = false;

	public TimeSpan ReconnectDelay { get; set; } = 2.Seconds();

	public event TcpMessageReceived TcpMessageReceivedEvent;

	public async Task Connect(
		string host,
		ushort port,
		int bufferSize = 1024,
		CancellationToken cancellationToken = default
	)
	{
		this.host = host;
		this.port = port;
		this.bufferSize = bufferSize;
		cancelToken = cancellationToken;

		serverIp = IPAddress.Parse(host);
		ipEndpoint = new IPEndPoint(serverIp, this.port);

		await MakeConnection();
	}

	public void Disconnect()
	{
		try
		{
			cancelSource.Cancel();
		}
		catch
		{ // ignored
		}

		ShutdownSocket();
	}

	public async Task Send(byte[] bytes)
	{
		if (!Connected || tcpClient is null || stream is null)
		{
			return;
		}

		try
		{
			await stream.WriteAsync(bytes, cancelToken);
		}
		catch (SocketException)
		{
			ShutdownSocket();

			await TryReconnect();
		}
		catch (IOException)
		{
			ShutdownSocket();

			await TryReconnect();
		}
	}

	public Task Send(string message, Encoding encoding = null)
	{
		encoding ??= Encoding.UTF8;

		var bytes = encoding.GetBytes(message);

		return Send(bytes);
	}

	protected abstract Stream GetStream();

	protected virtual void OnOnMessageReceived(byte[] data)
	{
		TcpMessageReceivedEvent?.Invoke(data);
	}

	private async Task CreateSocket()
	{
		ConsoleLog.Write("Create TCP Client");

		tcpClient = new TcpClient();

		await tcpClient.ConnectAsync(host, port, cancelToken);

		stream = GetStream();

		ConsoleLog.Write("After ConnectAsync");

		tcpClient.Client.NoDelay = true;
		tcpClient.Client.ReceiveBufferSize = bufferSize;
		tcpClient.Client.ReceiveTimeout = 0;
		tcpClient.Client.SendBufferSize = bufferSize;
		tcpClient.Client.SendTimeout = 0;

		tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
		tcpClient.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 900);
		tcpClient.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 300);
		tcpClient.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 5);

		ConsoleLog.Write("After setting all socket options");
	}

	private async Task MakeConnection()
	{
		try
		{
			ConsoleLog.WriteYellow($"Attempting to connect to <{host}:{port}>");

			await CreateSocket();
			VerifyCancelToken();

			Connected = true;

			ConsoleLog.WriteGreen($"Connection succeed for <{host}:{port}>");
		}
		catch (IOException)
		{
			ConsoleLog.WriteRed($"Connection failed to <{host}:{port}>");

			await TryReconnect();
		}
		catch (SocketException)
		{
			ConsoleLog.WriteRed($"Connection failed to <{host}:{port}>");

			await TryReconnect();
		}
	}

	private async Task ReceiveThread()
	{
		buffer = new byte[bufferSize];

		try
		{
			while (!cancelToken.IsCancellationRequested)
			{
				var bytesCount = await stream.ReadAsync(buffer, cancelToken);

				if (bytesCount > 0)
				{
					var bytes = new byte[bytesCount];

					Array.Copy(buffer, bytes, bytesCount);

					OnOnMessageReceived(bytes);
				}
			}
		}
		catch (Exception e)
		{
			ConsoleLog.WriteException(e);
		}
	}

	private void ShutdownSocket()
	{
		try
		{
			Connected = false;

			stream?.Close();
		}
		catch
		{ // ignored
		}

		try
		{
			tcpClient.Dispose();
		}
		catch
		{ // ignored
		}
	}

	private async Task TryReconnect()
	{
		ConsoleLog.WriteGray($"Reconnect <{Reconnect}>");

		if (Reconnect)
		{
			ConsoleLog.WriteYellow($"Going to try to reconnect to <{host}:{port}>");

			await Task.Delay(ReconnectDelay, cancelToken);

			await MakeConnection();
		}
	}

	private void VerifyCancelToken()
	{
		if (cancelToken != default)
		{
			return;
		}

		cancelSource = new CancellationTokenSource();
		cancelToken = cancelSource.Token;
	}
}
