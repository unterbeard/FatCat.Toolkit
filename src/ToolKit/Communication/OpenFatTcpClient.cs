namespace FatCat.Toolkit.Communication;

public class OpenFatTcpClient(IFatTcpLogger logger) : FatTcpClient(logger), IFatTcpClient
{
	protected override Stream GetStream()
	{
		return tcpClient.GetStream();
	}
}
