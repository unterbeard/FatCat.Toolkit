using FatCat.Toolkit.Console;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubServer
{
	void ClientResponseMessage(string sessionId, ToolkitMessage toolkitMessage);

	Task SendToAllClients(ToolkitMessage message);

	Task SendToClient(string connectionId, ToolkitMessage message);

	Task SendToClientNoResponse(string connectionId, ToolkitMessage message);
}

public class ToolkitHubServer : IToolkitHubServer
{
	private readonly IGenerator generator;
	private readonly IHubContext<ToolkitHub> hubContext;

	public ToolkitHubServer(IHubContext<ToolkitHub> hubContext,
							IGenerator generator)
	{
		this.hubContext = hubContext;
		this.generator = generator;
	}

	public void ClientResponseMessage(string sessionId, ToolkitMessage toolkitMessage) { ConsoleLog.WriteMagenta($"Got a client response!!!!!!! | SessionId {sessionId} | {JsonConvert.SerializeObject(toolkitMessage)}"); }

	public Task SendToAllClients(ToolkitMessage message) => throw new NotImplementedException();

	public Task SendToClient(string connectionId, ToolkitMessage message) => throw new NotImplementedException();

	public async Task SendToClientNoResponse(string connectionId, ToolkitMessage message) => await SendMessageToClient(connectionId, message, generator.NewId());

	private async Task SendMessageToClient(string connectionId, ToolkitMessage message, string sessionId) => await hubContext.Clients.Client(connectionId).SendAsync(ToolkitHub.ServerOriginatedMessage, message.MessageId, sessionId, message.Data);
}