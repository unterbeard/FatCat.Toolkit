using System.Collections.Concurrent;
using FatCat.Toolkit.Console;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubServer
{
	void ClientResponseMessage(string sessionId, ToolkitMessage toolkitMessage);

	Task SendToAllClients(ToolkitMessage message);

	Task<ToolkitMessage> SendToClient(string connectionId, ToolkitMessage message, TimeSpan? timeout = null);

	Task SendToClientNoResponse(string connectionId, ToolkitMessage message);
}

public class ToolkitHubServer : IToolkitHubServer
{
	private readonly IGenerator generator;
	private readonly IHubContext<ToolkitHub> hubContext;

	private readonly ConcurrentDictionary<string, ToolkitMessage> responses = new();
	private readonly ConcurrentDictionary<string, int> timedOutResponses = new();
	private readonly ConcurrentDictionary<string, ToolkitMessage> waitingForResponses = new();

	public ToolkitHubServer(IHubContext<ToolkitHub> hubContext,
							IGenerator generator)
	{
		this.hubContext = hubContext;
		this.generator = generator;
	}

	public void ClientResponseMessage(string sessionId, ToolkitMessage toolkitMessage)
	{
		if (timedOutResponses.TryRemove(sessionId, out _)) return;
		if (!waitingForResponses.TryRemove(sessionId, out _)) return;
		
		ConsoleLog.WriteMagenta($"Got a client response!!!!!!! | SessionId {sessionId} | {JsonConvert.SerializeObject(toolkitMessage)}");
		
		responses.TryAdd(sessionId, toolkitMessage);
	}

	public Task SendToAllClients(ToolkitMessage message) => throw new NotImplementedException();

	public async Task<ToolkitMessage> SendToClient(string connectionId, ToolkitMessage message, TimeSpan? timeout = null)
	{
		timeout ??= TimeSpan.FromSeconds(30);

		var sessionId = generator.NewId();

		waitingForResponses.TryAdd(sessionId, message);

		await SendMessageToClient(connectionId, message, sessionId);

		var startTime = DateTime.UtcNow;

		while (true)
		{
			if (responses.TryRemove(sessionId, out var response)) return response;

			if (DateTime.UtcNow - startTime > timeout)
			{
				timedOutResponses.TryAdd(sessionId, message.MessageId);

				throw new TimeoutException();
			}

			await Task.Delay(100);
		}
	}

	public async Task SendToClientNoResponse(string connectionId, ToolkitMessage message) => await SendMessageToClient(connectionId, message, generator.NewId());

	private async Task SendMessageToClient(string connectionId, ToolkitMessage message, string sessionId) => await hubContext.Clients.Client(connectionId).SendAsync(ToolkitHub.ServerOriginatedMessage, message.MessageId, sessionId, message.Data);
}