using Microsoft.AspNetCore.SignalR.Client;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubConnection : IAsyncDisposable
{
	Task Connect(string hubUrl);

	Task Send(int messageId, string data, string? sessionId = null);
}

public class ToolkitHubConnection : IToolkitHubConnection
{
	private readonly IGenerator generator;
	private HubConnection connection = null!;

	public ToolkitHubConnection(IGenerator generator) => this.generator = generator;

	public async Task Connect(string hubUrl)
	{
		connection = new HubConnectionBuilder()
					.WithUrl(hubUrl)
					.Build();

		await connection.StartAsync();
	}

	public ValueTask DisposeAsync() => connection.DisposeAsync();

	public Task Send(int messageId, string data, string? sessionId = null) => connection.SendAsync("Message", messageId, sessionId ?? generator.NewId(), data);
}