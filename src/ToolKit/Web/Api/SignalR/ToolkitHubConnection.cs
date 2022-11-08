using FatCat.Toolkit.Console;
using Microsoft.AspNetCore.SignalR.Client;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubConnection : IAsyncDisposable
{
	Task Connect(string hubUrl);

	Task Send(int messageId, string data);
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

		var responseMethod = OnServerMessageReceived;

		connection.On(ToolkitHub.ServerMessage, responseMethod);
	}

	public ValueTask DisposeAsync() => connection.DisposeAsync();

	public Task Send(int messageId, string data) => SendSessionMessage(messageId, data, generator.NewId());

	private void OnServerMessageReceived(int messageId, string sessionId, string data) { ConsoleLog.WriteCyan($"On ServerMessageReceived | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>"); }

	private Task SendSessionMessage(int messageId, string data, string sessionId) => connection.SendAsync("Message", messageId, sessionId, data);
}