using System.Collections.Concurrent;
using FatCat.Toolkit.Console;
using Humanizer;
using Microsoft.AspNetCore.SignalR.Client;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubConnection : IAsyncDisposable
{
	Task Connect(string hubUrl);

	Task<ToolkitMessage> Send(int messageId, string data, TimeSpan? timeout = null);

	Task SendNoResponse(int messageId, string data);
}

public class ToolkitHubConnection : IToolkitHubConnection
{
	private readonly IGenerator generator;

	private readonly ConcurrentDictionary<string, ToolkitMessage> responses = new();
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

	public async Task<ToolkitMessage> Send(int messageId, string data, TimeSpan? timeout = null)
	{
		timeout ??= 30.Seconds();

		var sessionId = generator.NewId();

		await SendSessionMessage(messageId, data, sessionId);

		var startTime = DateTime.UtcNow;
		
		while (true)
		{
			if (responses.TryRemove(sessionId, out var response)) return response;

			if (DateTime.UtcNow - startTime > timeout) throw new TimeoutException();

			await Task.Delay(100);
		}
	}

	public Task SendNoResponse(int messageId, string data) => SendSessionMessage(messageId, data, generator.NewId());

	private void OnServerMessageReceived(int messageId, string sessionId, string data)
	{
		ConsoleLog.WriteCyan($"On ServerMessageReceived | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>");

		responses.TryAdd(sessionId, new ToolkitMessage
									{
										MessageId = messageId,
										Data = data
									});
	}

	private Task SendSessionMessage(int messageId, string data, string sessionId) => connection.SendAsync("Message", messageId, sessionId, data);
}

public class ToolkitMessage
{
	public string? Data { get; set; }

	public int MessageId { get; set; }
}