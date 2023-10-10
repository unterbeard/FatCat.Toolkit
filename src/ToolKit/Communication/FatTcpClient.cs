using System.Net;
using System.Net.Sockets;
using FatCat.Toolkit.Console;
using Humanizer;

namespace FatCat.Toolkit.Communication;

public interface IFatTcpClient
{
	bool Connected { get; }

	bool Reconnect { get; set; }

	TimeSpan ReconnectDelay { get; set; }

	event TcpMessageReceived TcpMessageReceivedEvent;

	Task Connect(string host, ushort port, int bufferSize = 1024, CancellationToken cancellationToken = default);

	void Disconnect();

	Task Send(byte[] bytes);
}

public class FatTcpClient : IFatTcpClient
{
	private byte[] buffer;
	private int bufferSize;
	private CancellationTokenSource cancelSource;
	private CancellationToken cancelToken;

	private string host;
	private IPEndPoint ipEndpoint;
	private ushort port;
	private IPAddress serverIp;
	private Socket socket;

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

	private void ShutdownSocket()
	{
		try
		{
			Connected = false;

			socket?.Shutdown(SocketShutdown.Both);
			socket?.Close();
		}
		catch
		{ // ignored
		}

		try
		{
			socket?.Dispose();
		}
		catch
		{ // ignored
		}
	}

	public async Task Send(byte[] bytes)
	{
		if (!Connected || socket is null)
		{
			return;
		}

		try
		{
			await socket.SendAsync(bytes, cancelToken);
		}
		catch (SocketException)
		{
			ShutdownSocket();

			await TryReconnect();
		}
		catch (Exception e)
		{
			ConsoleLog.WriteException(e);
		}
	}

	protected virtual void OnOnMessageReceived(byte[] data)
	{
		TcpMessageReceivedEvent?.Invoke(data);
	}

	private void CreateSocket()
	{
		socket = new Socket(serverIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

		socket.NoDelay = true;
		socket.ReceiveBufferSize = bufferSize;
		socket.ReceiveTimeout = 0;
		socket.SendBufferSize = bufferSize;
		socket.SendTimeout = 0;

		socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
		socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 900);
		socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 300);
		socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 5);
	}

	private async Task MakeConnection()
	{
		try
		{
			ConsoleLog.WriteYellow($"Attempting to connect to <{host}:{port}>");

			CreateSocket();
			VerifyCancelToken();

			await socket.ConnectAsync(ipEndpoint, cancelToken);

			Connected = true;

			ConsoleLog.WriteGreen($"Connection succeed for <{host}:{port}>");
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
				var bytesCount = await socket.ReceiveAsync(buffer, cancelToken);

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
