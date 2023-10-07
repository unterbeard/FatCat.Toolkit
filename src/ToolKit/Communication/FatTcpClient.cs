using System.Net;
using System.Net.Sockets;
using FatCat.Toolkit.Console;

namespace FatCat.Toolkit.Communication;

public interface IFatTcpClient
{
	event TcpMessageReceived OnMessageReceived;

	void Connect(string host, ushort port, int bufferSize = 1024, CancellationToken cancellationToken = default);

	void Disconnect();

	void Send(byte[] bytes);
}

public class FatTcpClient : IFatTcpClient
{
	private int bufferSize;
	private CancellationTokenSource cancelSource;
	private CancellationToken cancelToken;
	private string host;
	private IPEndPoint ipEndpoint;
	private ushort port;
	private IPAddress serverIp;
	private Socket socket;

	public event TcpMessageReceived OnMessageReceived;

	public void Connect(
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

		CreateSocket();
		VerifyCancelToken();
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

	private void CreateSocket()
	{
		socket = new Socket(serverIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

		socket.NoDelay = true;
		socket.ReceiveBufferSize = this.bufferSize;
		socket.ReceiveTimeout = 0;
		socket.SendBufferSize = this.bufferSize;
		socket.SendTimeout = 0;

		socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
		socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 900);
		socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 300);
		socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 5);
	}

	private void ReceiveThread()
	{
		try
		{
			
		}
		catch (Exception e)
		{
			ConsoleLog.WriteException(e);
		}
	}

	public void Disconnect()
	{
		throw new NotImplementedException();
	}

	public void Send(byte[] bytes)
	{
		throw new NotImplementedException();
	}

	protected virtual void OnOnMessageReceived(byte[] data)
	{
		OnMessageReceived?.Invoke(data);
	}
}
