using FatCat.Toolkit.Console;
using Microsoft.AspNetCore.SignalR;

namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitHub : Hub
{
	public const string ServerOriginatedMessage = "ServerOriginatedMessage";
	public const string ServerResponseMessage = "ServerResponseMessage";

	public async Task Message(int messageId, string sessionId, string data)
	{
		ConsoleLog.WriteCyan($"Got Message | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>");

		var toolkitMessage = new ToolkitMessage
							{
								Data = data,
								ConnectionId = Context.ConnectionId,
								MessageId = messageId
							};

		var responseMessage = await WebApplication.Settings.OnOnClientHubMessage(toolkitMessage);

		ConsoleLog.WriteDarkMagenta($"Response for message | <{responseMessage}>");

		await Task.Delay(messageId);

		await Clients.Client(Context.ConnectionId).SendAsync(ServerResponseMessage, messageId, sessionId, responseMessage);
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