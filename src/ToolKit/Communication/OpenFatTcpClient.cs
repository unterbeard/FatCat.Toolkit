namespace FatCat.Toolkit.Communication;

public class OpenFatTcpClient : FatTcpClient, IFatTcpClient
{
	protected override Stream GetStream()
	{
		return tcpClient.GetStream();
	}
}
