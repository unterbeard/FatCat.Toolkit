#nullable enable
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Logging;
using Microsoft.AspNetCore.SignalR;

namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitHub : Hub
{
	public const string ServerOriginatedMessage = "ServerOriginatedMessage";
	public const string ServerResponseMessage = "ServerResponseMessage";

	private IToolkitHubServer HubServer => SystemScope.Container.Resolve<IToolkitHubServer>();

	private IToolkitLogger Logger => SystemScope.Container.Resolve<IToolkitLogger>();

	public async Task ClientMessage(int messageId, string sessionId, string data)
	{
		Logger.Debug($"Got Message | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>");

		var toolkitMessage = new ToolkitMessage
							{
								Data = data,
								ConnectionId = Context.ConnectionId,
								MessageId = messageId
							};

		var responseMessage = await ToolkitWebApplication.Settings.OnClientHubMessage(toolkitMessage);

		Logger.Debug($"Response for message | <{responseMessage}>");

		await Clients.Client(Context.ConnectionId).SendAsync(ServerResponseMessage, messageId, sessionId, responseMessage);
	}

	public async Task ClientDataBufferMessage(int messageId, string sessionId, string data, byte[] dataBuffer)
	{
		await Task.CompletedTask;
		
		Logger.Debug($"Got Message | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}> | Buffer <{dataBuffer.Length}>");
		
		var toolkitMessage = new ToolkitMessage
							{
								Data = data,
								ConnectionId = Context.ConnectionId,
								MessageId = messageId
							};

		var responseMessage = await ToolkitWebApplication.Settings.OnOnClientDataBufferMessage(toolkitMessage, dataBuffer);

		Logger.Debug($"Response for message | <{responseMessage}>");
		
		await Clients.Client(Context.ConnectionId).SendAsync(ServerResponseMessage, messageId, sessionId, responseMessage);
	}

	public async Task ClientResponseMessage(int messageId, string sessionId, string data)
	{
		await Task.CompletedTask;

		var toolkitMessage = new ToolkitMessage
							{
								Data = data,
								ConnectionId = Context.ConnectionId,
								MessageId = messageId
							};

		HubServer.ClientResponseMessage(sessionId, toolkitMessage);
	}

	public override Task OnConnectedAsync()
	{
		try { HubServer.OnClientConnected(Context.ConnectionId); }
		catch (Exception ex) { Logger.Exception(ex); }

		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		try { HubServer.OnClientDisconnected(Context.ConnectionId); }
		catch (Exception ex) { Logger.Exception(ex); }

		return base.OnDisconnectedAsync(exception);
	}
}