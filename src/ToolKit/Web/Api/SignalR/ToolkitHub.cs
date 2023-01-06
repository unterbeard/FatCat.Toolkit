#nullable enable
using System.Security.Claims;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Logging;
using Microsoft.AspNetCore.SignalR;

namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitHub : Hub
{
	public const string ServerDataBufferMessage = "ServerDataBufferMessage";
	public const string ServerOriginatedMessage = "ServerOriginatedMessage";
	public const string ServerResponseMessage = "ServerResponseMessage";

	private IToolkitHubServer HubServer => SystemScope.Container.Resolve<IToolkitHubServer>();

	private IToolkitLogger Logger => SystemScope.Container.Resolve<IToolkitLogger>();

	public async Task ClientDataBufferMessage(int messageType, string sessionId, string data, byte[] dataBuffer)
	{
		await Task.CompletedTask;

		Logger.Debug($"Got DataBuffer Message | MessageType <{messageType}> | SessionId <{sessionId}> | Data <{data}> | Buffer <{dataBuffer.Length}> | ConnectionId <{Context.ConnectionId}>");

		var toolkitMessage = new ToolkitMessage
							{
								Data = data,
								ConnectionId = Context.ConnectionId,
								User = GetUser(),
								MessageType = messageType
							};

		var responseMessage = await ToolkitWebApplication.Settings.OnOnClientDataBufferMessage(toolkitMessage, dataBuffer);

		Logger.Debug($"Response for message | <{responseMessage}> | Sending to ConnectionId {Context.ConnectionId}");

		await Clients.Client(Context.ConnectionId).SendAsync(ServerResponseMessage, messageType, sessionId, responseMessage);

		Logger.Debug($"Done sending response for message | <{responseMessage}> | SessionId <{sessionId}> | ConnectionId {Context.ConnectionId}");
	}

	public async Task ClientMessage(int messageType, string sessionId, string data)
	{
		Logger.Debug($"Got Message | MessageType <{messageType}> | SessionId <{sessionId}> | Data <{data}>");

		var toolkitMessage = new ToolkitMessage
							{
								Data = data,
								ConnectionId = Context.ConnectionId,
								User = GetUser(),
								MessageType = messageType
							};

		var responseMessage = await ToolkitWebApplication.Settings.OnClientHubMessage(toolkitMessage);

		Logger.Debug($"Response for message | <{responseMessage}>");

		await Clients.Client(Context.ConnectionId).SendAsync(ServerResponseMessage, messageType, sessionId, responseMessage);
	}

	public async Task ClientResponseMessage(int messageType, string sessionId, string data)
	{
		await Task.CompletedTask;

		var toolkitMessage = new ToolkitMessage
							{
								Data = data,
								ConnectionId = Context.ConnectionId,
								User = GetUser(),
								MessageType = messageType
							};

		HubServer.ClientResponseMessage(sessionId, toolkitMessage);
	}

	public override Task OnConnectedAsync()
	{
		try
		{
			var username = Context!.User?.Identity?.Name;

			Logger.Debug($"Connection made | ConnectionId <{Context!.ConnectionId}> | Username <{username}>");

			HubServer.OnClientConnected(Context!.ConnectionId);
		}
		catch (Exception ex) { Logger.Exception(ex); }

		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		try { HubServer.OnClientDisconnected(Context.ConnectionId); }
		catch (Exception ex) { Logger.Exception(ex); }

		return base.OnDisconnectedAsync(exception);
	}

	private static ToolkitClaim CreateClaim(Claim claim) => new()
															{
																Type = claim.Type,
																Value = claim.Value,
																Issuer = claim.Issuer,
																OriginalIssuer = claim.OriginalIssuer,
															};

	private ToolkitUser? GetUser()
	{
		if (Context.User == null) return null;

		var contextUser = Context.User;

		return new ToolkitUser
				{
					Name = Context.User.Identity?.Name,
					AuthenticationType = Context.User.Identity?.AuthenticationType,
					IsAuthenticated = Context.User.Identity?.IsAuthenticated ?? false,
					Claims = contextUser.Claims.Select(CreateClaim)
										.ToList()
				};
	}
}