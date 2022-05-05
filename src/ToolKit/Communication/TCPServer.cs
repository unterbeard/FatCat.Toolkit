using System.Net;
using System.Net.Sockets;
using System.Text;
using FatCat.Toolkit.Console;

namespace FatCat.Toolkit.Communication;

public delegate void TcpMessageReceived(string message);

public interface ITcpServer : IDisposable
{
	event TcpMessageReceived? OnMessageReceived;

	void Start(IPAddress ipAddress, ushort port, int receiveBufferSize = 1024);

	void Start(IPAddress ipAddress, ushort port, Encoding encoding, int receiveBufferSize = 1024);

	void Stop();
}

public class TcpServer : ITcpServer
{
	private int bufferSize;
	private Encoding? encoding;
	private TcpListener? listener;

	public event TcpMessageReceived? OnMessageReceived;

	public void Dispose() => listener?.Stop();

	public void Start(IPAddress ipAddress, ushort port, int receiveBufferSize = 1024) => Start(ipAddress, port, Encoding.UTF8, receiveBufferSize);

	public void Start(IPAddress ipAddress, ushort port, Encoding encoding, int receiveBufferSize = 1024)
	{
		bufferSize = receiveBufferSize;
		this.encoding = encoding;

		listener = new TcpListener(ipAddress, port);

		listener.Start();

		listener.BeginAcceptTcpClient(OnTcpClientConnected, listener);
	}

	public void Stop() => Dispose();

	protected virtual void InvokeMessageReceived(string message) => OnMessageReceived?.Invoke(message);

	private void OnTcpClientConnected(IAsyncResult ar)
	{
		try
		{
			var syncListener = ar.AsyncState as TcpListener;

			var client = listener?.EndAcceptTcpClient(ar);

			syncListener?.BeginAcceptTcpClient(OnTcpClientConnected, syncListener);

			var buffer = new byte[bufferSize];

			var stream = client?.GetStream();

			var fullMessage = new StringBuilder();

			if (stream is { CanRead: true })
			{
				int dataRead;

				do
				{
					dataRead = stream.Read(buffer, 0, buffer.Length);

					if (dataRead != 0)
					{
						var encodingToUse = encoding ?? Encoding.UTF8;

						var data = encodingToUse.GetString(buffer, 0, dataRead);

						fullMessage.Append(data);
					}
				} while (dataRead != 0);
			}

			stream?.Close();
			client?.Close();

			InvokeMessageReceived(fullMessage.ToString());
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}
}