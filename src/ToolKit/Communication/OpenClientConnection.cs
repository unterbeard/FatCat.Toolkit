using System.Net.Sockets;

namespace FatCat.Toolkit.Communication;

internal class OpenClientConnection : ClientConnection
{
	public OpenClientConnection(
		IFatTcpServer server,
		TcpClient client,
		string clientId,
		int bufferSize,
		IFatTcpLogger logger,
		CancellationToken cancellationToken
	)
		: base(server, client, clientId, bufferSize, logger, cancellationToken) { }

	protected override async Task<Stream> GetStream()
	{
		await Task.CompletedTask;

		return client.GetStream();
	}
}