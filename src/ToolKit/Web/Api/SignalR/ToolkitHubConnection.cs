using Microsoft.AspNetCore.SignalR.Client;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubConnection : IAsyncDisposable
{
	Task Connect(string hubUrl);

	Task Send(int messageId, string sessionId, string data);
}

public class ToolkitHubConnection : IToolkitHubConnection
{
	private HubConnection connection = null!;

	public async Task Connect(string hubUrl)
	{
		connection = new HubConnectionBuilder()
					.WithUrl(hubUrl)
					.Build();

		await connection.StartAsync();
	}

	public ValueTask DisposeAsync() => connection.DisposeAsync();

	public Task Send(int messageId, string sessionId, string data) => connection.SendAsync("Message", messageId, sessionId, data);
}