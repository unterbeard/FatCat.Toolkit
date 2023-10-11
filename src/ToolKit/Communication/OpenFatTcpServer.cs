using System.Net.Sockets;

namespace FatCat.Toolkit.Communication;

public class OpenFatTcpServer : FatTcpServer, IFatTcpServer
{
	public OpenFatTcpServer(IGenerator generator)
		: base(generator) { }

	internal override ClientConnection GetClientConnection(TcpClient client, string clientId)
	{
		return new OpenClientConnection(this, client, clientId, bufferSize, cancelToken);
	}
}