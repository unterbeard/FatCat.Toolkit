using System.Net;
using System.Net.Sockets;
using System.Text;
using FatCat.Toolkit.Console;

namespace OneOff;

public class SpikeTcpClient : IDisposable
{
	private readonly IPEndPoint endPoint;

	public SpikeTcpClient(IPEndPoint endPoint) => this.endPoint = endPoint;

	public void Dispose() { }

	public async Task Send(string message)
	{
		using var client = new TcpClient();

		await client.ConnectAsync(endPoint);

		await using var networkStream = client.GetStream();

		ConsoleLog.WriteMagenta("Sending ---------------");
		ConsoleLog.WriteMagenta(message);

		var messageBytes = Encoding.UTF8.GetBytes(message);

		await networkStream.WriteAsync(messageBytes);
		await networkStream.FlushAsync();
		networkStream.Close();

		ConsoleLog.WriteMagenta("-----------   Done Sending");

		client.Close();
	}
}