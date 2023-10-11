using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace FatCat.Toolkit.Communication;

public class OpenFatTcpClient : FatTcpClient, IFatTcpClient
{
	protected override Stream GetStream()
	{
		return tcpClient.GetStream();
	}
}

public class SecureFatTcpClient : FatTcpClient, IFatTcpClient
{
	private readonly X509Certificate certificate;

	public SecureFatTcpClient(X509Certificate certificate)
	{
		this.certificate = certificate;
	}

	private bool CertValidation(
		object sender,
		X509Certificate remoteCertificate,
		X509Chain chain,
		SslPolicyErrors sslPolicyErrors
	)
	{
		return CertificateHelper.CertValidation(remoteCertificate, certificate, sslPolicyErrors);
	}

	protected override Stream GetStream()
	{
		var sslStream = new SslStream(tcpClient.GetStream(), false, null, null);

		var clientCertificateCollection = new X509CertificateCollection(new X509Certificate[] { certificate });

		sslStream.AuthenticateAsClient(
			certificate.Subject,
			clientCertificateCollection,
			SslProtocols.Tls12,
			false
		);

		return sslStream;
	}
}
