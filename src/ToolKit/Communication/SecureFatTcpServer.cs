using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace FatCat.Toolkit.Communication;

public class SecureFatTcpServer : FatTcpServer, IFatTcpServer
{
	private readonly X509Certificate certificate;

	public SecureFatTcpServer(X509Certificate certificate, IGenerator generator)
		: base(generator)
	{
		this.certificate = certificate;
	}

	internal override ClientConnection GetClientConnection(TcpClient client, string clientId)
	{
		return new SecureClientConnection(certificate, this, client, clientId, bufferSize, cancelToken);
	}
}
