using System.Net;
using System.Net.Sockets;
using System.Text;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;

namespace OneOff;

public static class Program
{
	public static void Main(params string[] args)
	{
		ConsoleLog.WriteBlue("Going to implement a TCP Client/Server");

		var consoleUtilities = new ConsoleUtilities(new ManualWaitEvent());

		var server = new SpikeServer(IPAddress.Any, 62222);

		server.Start();

		consoleUtilities.WaitForExit();

		server.Dispose();
	}
}

public class SpikeServer : IDisposable
{
	private readonly TcpListener listener;
	private readonly CancellationTokenSource cancelSource;

	public SpikeServer(IPAddress ipAddress, ushort port)
	{
		listener = new TcpListener(ipAddress, port);

		cancelSource = new CancellationTokenSource();
	}

	public void Close() => listener.Stop();

	public void Dispose()
	{
		cancelSource.Dispose();
		Close();
	}

	public void Start()
	{
		listener.Start();
		
		ConsoleLog.WriteCyan($"Listening on <{listener.LocalEndpoint}>");

		listener.BeginAcceptTcpClient(TcpClientConnected, listener);
	}

	private void TcpClientConnected(IAsyncResult ar)
	{
		var syncListener = ar.AsyncState as TcpListener;

		var client = listener.EndAcceptTcpClient(ar);

		ConsoleLog.WriteBlue($"Client Connected from {client}");
		ConsoleLog.WriteBlue(". . . . . . . Connected!");

		syncListener?.BeginAcceptTcpClient(TcpClientConnected, syncListener);

		var bytes = new byte[1024];

		var stream = client.GetStream();

		if (stream.CanRead)
		{
			while (stream.DataAvailable)
			{
				var dataRead = stream.Read(bytes, 0, bytes.Length);

				ConsoleLog.WriteCyan($". . . . . Read {dataRead} bytes");

				var data = Encoding.ASCII.GetString(bytes, 0, dataRead);

				ConsoleLog.WriteCyan($"{data}");
				ConsoleLog.WriteCyan(". . . . . Done read data");
			}
		}

		client.Close();
	}
}