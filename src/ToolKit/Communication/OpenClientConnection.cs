using System.Net.Sockets;

namespace FatCat.Toolkit.Communication;

internal class OpenClientConnection : ClientConnection
{
	public OpenClientConnection(
		IFatTcpServer server,
		TcpClient client,
		string clientId,
		int bufferSize,
		CancellationToken cancellationToken
	)
		: base(server, client, clientId, bufferSize, cancellationToken) { }

	protected override Stream GetStream()
	{
		return client.GetStream();
	}
}
