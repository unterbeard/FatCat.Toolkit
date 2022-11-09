using Microsoft.AspNetCore.SignalR;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubServer
{
	Task SendToClientNoResponse(string connectionId, ToolkitMessage message);
	
	Task SendToClient(string connectionId, ToolkitMessage message);
	
	Task SendToAllClients(ToolkitMessage message);
}

public class ToolkitHubServer : IToolkitHubServer
{
	private readonly IHubContext<ToolkitHub> hubContext;
	private readonly IGenerator generator;

	public ToolkitHubServer(IHubContext<ToolkitHub> hubContext, 
							IGenerator generator)
	{
		this.hubContext = hubContext;
		this.generator = generator;
	}

	public async Task SendToClientNoResponse(string connectionId, ToolkitMessage message)
	{
		await hubContext.Clients.Client(connectionId).SendAsync(ToolkitHub.ServerOriginatedMessage, message.MessageId, generator.NewId(), message.Data);
	}

	public Task SendToClient(string connectionId, ToolkitMessage message) => throw new NotImplementedException();

	public Task SendToAllClients(ToolkitMessage message) => throw new NotImplementedException();
}