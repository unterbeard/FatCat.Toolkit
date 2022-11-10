#nullable enable
namespace FatCat.Toolkit.Web.Api.SignalR;

public class ConnectionResult
{
	public bool Connected { get; set; }

	public IToolkitHubClientConnection? Connection { get; set; }

	public ConnectionResult(bool connected, IToolkitHubClientConnection? connection = null)
	{
		Connected = connected;
		Connection = connection;
	}
}