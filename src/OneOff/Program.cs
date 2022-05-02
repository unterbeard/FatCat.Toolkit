using System.Net;
using System.Net.Sockets;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;

namespace OneOff;

public static class Program
{
	public static void Main(params string[] args)
	{
		ConsoleLog.WriteBlue("Going to implement a TCP Client/Server");

		var consoleUtilities = new ConsoleUtilities(new ManualWaitEvent());

		consoleUtilities.WaitForExit();
	}
}

public class SpikeServer
{
	private readonly TcpListener listener;

	public SpikeServer(IPAddress ipAddress, ushort port) => listener = new TcpListener(ipAddress, port);

	public async Task Start()
	{
		listener.Start();
		await StartListener();
	}

	private async Task HandleConnection(TcpClient client)
	{
		var stream = client.GetStream();

		var buffer = new Memory<byte>();

		var bytesRead = -1;

		do
		{
			bytesRead = await stream.ReadAsync(buffer);
			
			
			
		} while (bytesRead != 0);
	}

	private async Task StartListener()
	{
		try
		{
			ConsoleLog.WriteCyan("Waiting for a Connection . . . .");

			var client = await listener.AcceptTcpClientAsync();

			ConsoleLog.WriteCyan("Somebody Connected");

			HandleConnection(client);
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}
}