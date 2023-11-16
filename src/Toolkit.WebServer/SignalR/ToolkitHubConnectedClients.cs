#nullable enable
using FatCat.Toolkit.Web.Api.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace FatCat.Toolkit.WebServer.SignalR;

public interface IToolkitHubConnectedClients
{
	Task SendMessage(string clientId, int messageType, string data, string? sessionId = null);
}

public class ToolkitHubConnectedClients : IToolkitHubConnectedClients
{
	private readonly IGenerator generator;
	private readonly IHubContext<ToolkitHub> hubContext;

	public ToolkitHubConnectedClients(IHubContext<ToolkitHub> hubContext, IGenerator generator)
	{
		this.hubContext = hubContext;
		this.generator = generator;
	}

	public async Task SendMessage(string clientId, int messageType, string data, string? sessionId = null)
	{
		var client = hubContext.Clients.Client(clientId);

		await client.SendCoreAsync(
			ToolkitHubMethodNames.ServerResponseMessage,
			new object[] { messageType, data, sessionId ?? generator.NewId() }
		);
	}
}
