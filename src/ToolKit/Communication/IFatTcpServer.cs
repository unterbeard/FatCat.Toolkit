using System.Text;

namespace FatCat.Toolkit.Communication;

public interface IFatTcpServer : IDisposable
{
	event TcpMessageReceived TcpMessageReceivedEvent;

	void OnMessageReceived(byte[] bytesReceived);

	void Start(ushort port, int receiveBufferSize = 1024);

	void Start(ushort port, Encoding encoding, int receiveBufferSize = 1024);

	void Stop();
}