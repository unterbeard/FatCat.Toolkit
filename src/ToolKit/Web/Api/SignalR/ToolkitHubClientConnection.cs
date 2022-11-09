using System.Collections.Concurrent;
using FatCat.Toolkit.Console;
using Humanizer;
using Microsoft.AspNetCore.SignalR.Client;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubClientConnection : IAsyncDisposable
{
	Task Connect(string hubUrl);

	Task<ToolkitMessage> Send(ToolkitMessage message, TimeSpan? timeout = null);

	Task SendNoResponse(ToolkitMessage message);
}

public class ToolkitHubClientConnection : IToolkitHubClientConnection
{
	private readonly IGenerator generator;

	private readonly ConcurrentDictionary<string, ToolkitMessage> responses = new();
	private readonly ConcurrentDictionary<string, int> timedOutResponses = new();
	private readonly ConcurrentDictionary<string, ToolkitMessage> waitingForResponses = new();
	private HubConnection connection = null!;

	public ToolkitHubClientConnection(IGenerator generator) => this.generator = generator;

	public async Task Connect(string hubUrl)
	{
		connection = new HubConnectionBuilder()
					.WithUrl(hubUrl)
					.Build();

		await connection.StartAsync();

		var responseMethod = OnServerResponseMessageReceived;
		var originalMethod = OnServerOriginatedMessage;

		connection.On(ToolkitHub.ServerResponseMessage, responseMethod);
		connection.On(ToolkitHub.ServerOriginatedMessage, originalMethod);
	}

	public ValueTask DisposeAsync() => connection.DisposeAsync();

	public async Task<ToolkitMessage> Send(ToolkitMessage message, TimeSpan? timeout = null)
	{
		timeout ??= 30.Seconds();

		var sessionId = generator.NewId();

		waitingForResponses.TryAdd(sessionId, message);

		await SendSessionMessage(message.MessageId, message.Data ?? string.Empty, sessionId);

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

	public Task SendNoResponse(ToolkitMessage message) => SendSessionMessage(message.MessageId, message.Data ?? string.Empty, generator.NewId());

	private void OnServerOriginatedMessage(int messageId, string sessionId, string data)
	{
		ConsoleLog.WriteMagenta(new string('-', 80));
		ConsoleLog.WriteMagenta($"OnServerOriginatedMessage | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>");
		ConsoleLog.WriteMagenta(new string('-', 80));
	}

	private void OnServerResponseMessageReceived(int messageId, string sessionId, string data)
	{
		if (timedOutResponses.TryRemove(sessionId, out _)) return;
		if (!waitingForResponses.TryRemove(sessionId, out _)) return;

		ConsoleLog.WriteCyan($"On ServerMessageReceived | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>");

		responses.TryAdd(sessionId, new ToolkitMessage
									{
										MessageId = messageId,
										Data = data
									});
	}

	private Task SendSessionMessage(int messageId, string data, string sessionId) => connection.SendAsync("Message", messageId, sessionId, data);
}