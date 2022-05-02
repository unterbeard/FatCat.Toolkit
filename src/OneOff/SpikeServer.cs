using System.Net;
using System.Net.Sockets;
using System.Text;
using FatCat.Toolkit.Console;

namespace OneOff;

public delegate void MessageReceived(string message);

public class SpikeServer : IDisposable
{
	private readonly int bufferSize;
	private readonly CancellationTokenSource cancelSource;
	private readonly TcpListener listener;

	public SpikeServer(IPAddress ipAddress, ushort port, int bufferSize = 1024)
	{
		this.bufferSize = bufferSize;
		listener = new TcpListener(ipAddress, port);

		cancelSource = new CancellationTokenSource();
	}

	public event MessageReceived? OnMessageReceived;

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

	protected virtual void InvokeMessageReceived(string message) => OnMessageReceived?.Invoke(message);

	private void TcpClientConnected(IAsyncResult ar)
	{
		try
		{
			var syncListener = ar.AsyncState as TcpListener;

			var client = listener.EndAcceptTcpClient(ar);

			ConsoleLog.WriteCyan("Going to Listen for Connections");
			syncListener?.BeginAcceptTcpClient(TcpClientConnected, syncListener);

			ConsoleLog.WriteBlue($"Client Connected from {client.Client.RemoteEndPoint}");
			ConsoleLog.WriteBlue(". . . . . . . Connected!");

			var buffer = new byte[bufferSize];

			var stream = client.GetStream();

			var fullMessage = new StringBuilder();

			if (stream.CanRead)
			{
				var dataRead = -1;

				do
				{
					dataRead = stream.Read(buffer, 0, buffer.Length);

					if (dataRead != 0)
					{
						var data = Encoding.ASCII.GetString(buffer, 0, dataRead);

						ConsoleLog.WriteCyan("Reading . . . . . ");
						ConsoleLog.WriteCyan($"{data}");
						ConsoleLog.WriteCyan($". . . . . Done read data | {client.Client.RemoteEndPoint}");

						fullMessage.Append(data);
					}
				} while (dataRead != 0);
			}
			else ConsoleLog.WriteRed($"Cannot read the stream from <{client.Client.RemoteEndPoint}>");

			stream.Close();
			client.Close();

			InvokeMessageReceived(fullMessage.ToString());
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}
}