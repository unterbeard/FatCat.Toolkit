using FatCat.Toolkit.Console;
using Microsoft.AspNetCore.SignalR;

namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitHub : Hub
{
	public const string ServerMessage = "ServerMessage";
	
	public Task Message(int messageId, string sessionId, string data)
	{
		ConsoleLog.WriteCyan($"Got Message | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>");
		
		return Task.CompletedTask;
	}
	
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