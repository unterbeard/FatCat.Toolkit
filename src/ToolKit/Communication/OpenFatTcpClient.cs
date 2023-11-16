namespace FatCat.Toolkit.Communication;

public class OpenFatTcpClient : FatTcpClient, IFatTcpClient
{
	public OpenFatTcpClient(IFatTcpLogger logger)
		: base(logger) { }

	protected override Stream GetStream() => tcpClient.GetStream();
}
