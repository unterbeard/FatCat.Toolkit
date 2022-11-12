#nullable enable
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Logging;
using Humanizer;
using Microsoft.AspNetCore.SignalR.Client;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubClientConnection : IAsyncDisposable
{
	event ToolkitHubMessage? ServerMessage;

	Task Connect(string hubUrl);

	Task<ToolkitMessage> Send(ToolkitMessage message, TimeSpan? timeout = null);

	Task<ToolkitMessage> SendDataBuffer(ToolkitMessage message, byte[] dataBuffer, TimeSpan? timeout = null);

	Task SendDataBufferNoResponse(ToolkitMessage message, byte[] dataBuffer);

	Task SendNoResponse(ToolkitMessage message);

	Task<bool> TryToConnect(string hubUrl);
}

public class ToolkitHubClientConnection : IToolkitHubClientConnection
{
	private readonly IGenerator generator;
	private readonly IToolkitLogger logger;

	private readonly ConcurrentDictionary<string, ToolkitMessage> responses = new();
	private readonly ConcurrentDictionary<string, int> timedOutResponses = new();
	private readonly ConcurrentDictionary<string, ToolkitMessage> waitingForResponses = new();
	private HubConnection connection = null!;

	public ToolkitHubClientConnection(IGenerator generator,
									IToolkitLogger logger)
	{
		this.generator = generator;
		this.logger = logger;
	}

	public event ToolkitHubMessage? ServerMessage;

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

		return await WaitForResponse(message, timeout, sessionId);
	}

	public async Task<ToolkitMessage> SendDataBuffer(ToolkitMessage message, byte[] dataBuffer, TimeSpan? timeout = null)
	{
		timeout ??= 30.Seconds();

		var sessionId = generator.NewId();

		waitingForResponses.TryAdd(sessionId, message);

		await connection.SendAsync(nameof(ToolkitHub.ClientDataBufferMessage), message.MessageId, sessionId, message.Data, dataBuffer);

		return await WaitForResponse(message, timeout, sessionId);
	}

	public async Task SendDataBufferNoResponse(ToolkitMessage message, byte[] dataBuffer)
	{
		var sessionId = generator.NewId();

		await connection.SendAsync(nameof(ToolkitHub.ClientDataBufferMessage), message.MessageId, sessionId, message.Data, dataBuffer);
	}

	public Task SendNoResponse(ToolkitMessage message) => SendSessionMessage(message.MessageId, message.Data ?? string.Empty, generator.NewId());

	public async Task<bool> TryToConnect(string hubUrl)
	{
		try
		{
			await Connect(hubUrl);

			return true;
		}
		catch (Exception e)
		{
			ConsoleLog.WriteBlue($"Failed Exception is {e.GetType().FullName}");

			return false;
		}
	}

	private Task<string?> OnServerMessage(ToolkitMessage message) => ServerMessage?.Invoke(message)!;

	private async Task OnServerOriginatedMessage(int messageId, string sessionId, string data)
	{
		logger.Debug(new string('-', 80));
		logger.Debug($"OnServerOriginatedMessage | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>");
		logger.Debug(new string('-', 80));

		var message = new ToolkitMessage
					{
						Data = data,
						MessageId = messageId
					};

		var response = await OnServerMessage(message);

		if (response is not null) await connection.SendAsync(nameof(ToolkitHub.ClientResponseMessage), messageId, sessionId, response);
	}

	private void OnServerResponseMessageReceived(int messageId, string sessionId, string data)
	{
		if (timedOutResponses.TryRemove(sessionId, out _)) return;

		if (!waitingForResponses.TryRemove(sessionId, out _))
		{
			ConsoleLog.WriteDarkYellow($"Did not have a waiting for response for {sessionId}");

			return;
		}

		logger.Debug($"On ServerMessageReceived | MessageId <{messageId}> | SessionId <{sessionId}> | Data <{data}>");

		responses.TryAdd(sessionId, new ToolkitMessage
									{
										MessageId = messageId,
										Data = data
									});
	}

	private Task SendSessionMessage(int messageId, string data, string sessionId) => connection.SendAsync(nameof(ToolkitHub.ClientMessage), messageId, sessionId, data);

	private async Task<ToolkitMessage> WaitForResponse(ToolkitMessage message, [DisallowNull] TimeSpan? timeout, string sessionId)
	{
		var startTime = DateTime.UtcNow;

		ConsoleLog.WriteDarkYellow($"Going to wait for a response for {timeout} | SessionId <{sessionId}>");

		while (true)
		{
			if (responses.TryRemove(sessionId, out var response)) return response;

			if (DateTime.UtcNow - startTime > timeout)
			{
				timedOutResponses.TryAdd(sessionId, message.MessageId);

				throw new TimeoutException();
			}

			await Task.Delay(35);
		}
	}
}