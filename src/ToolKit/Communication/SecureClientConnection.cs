using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace FatCat.Toolkit.Communication;

internal static class CertificateHelper
{
	public static bool CertValidation(
		X509Certificate remoteCertificate,
		X509Certificate localCertificate,
		SslPolicyErrors sslPolicyErrors
	)
	{
		switch (sslPolicyErrors)
		{
			case SslPolicyErrors.None:
			case SslPolicyErrors.RemoteCertificateChainErrors:
				return true;
			default:

				// Valid if keys are the same
				return remoteCertificate.GetPublicKeyString() == localCertificate.GetPublicKeyString();
		}
	}
}

internal class SecureClientConnection(
	X509Certificate certificate,
	IFatTcpServer server,
	TcpClient client,
	string clientId,
	int bufferSize,
	IFatTcpLogger logger,
	CancellationToken cancellationToken
) : ClientConnection(server, client, clientId, bufferSize, logger, cancellationToken)
{
	protected override async Task<Stream> GetStream()
	{
		var sslStream = new SslStream(client.GetStream(), false, CertValidation);

		await sslStream.AuthenticateAsServerAsync(certificate, true, SslProtocols.Tls12, false);

		return sslStream;
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
}
