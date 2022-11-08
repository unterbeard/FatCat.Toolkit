using FatCat.Toolkit.Console;
using Microsoft.AspNetCore.SignalR;

namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitHub : Hub
{
	public const string ServerMessage = "ServerMessage";

	public async Task Message(int messageId, string sessionId, string data)
	{
		ConsoleLog.WriteCyan($"Got Message | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>");

		var receivedMessage = await WebApplication.Settings.OnOnClientHubMessage(messageId, data);
		
		ConsoleLog.WriteDarkMagenta($"Response for message | <{receivedMessage}>");
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