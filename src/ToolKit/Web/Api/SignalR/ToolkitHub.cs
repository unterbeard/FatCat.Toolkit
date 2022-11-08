using FatCat.Toolkit.Console;
using Microsoft.AspNetCore.SignalR;

namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitHub : Hub
{
	public override Task OnConnectedAsync()
	{
		try { ConsoleLog.WriteCyan($"Connected | <{Context.ConnectionId}>"); }
		catch (Exception ex) { ConsoleLog.WriteException(ex); }

		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		try { ConsoleLog.WriteCyan($"Client has disconnected | <{Context.ConnectionId}>"); }
		catch (Exception ex) { ConsoleLog.WriteException(ex); }

		return base.OnDisconnectedAsync(exception);
	}
}