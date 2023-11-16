#nullable enable
using System.Collections.Concurrent;
using FatCat.Toolkit.Injection;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubClientFactory : IAsyncDisposable
{
	Task<IToolkitHubClientConnection> ConnectToClient(string hubUrl);

	void RemoveHubFromConnections(string hubUrl);

	Task<ConnectionResult> TryToConnectToClient(string hubUrl, Action? onConnectionLost = null);
}

public class ToolkitHubClientFactory : IToolkitHubClientFactory
{
	private readonly ConcurrentDictionary<string, IToolkitHubClientConnection> connections = new();
	private readonly ISystemScope scope;

	public ToolkitHubClientFactory(ISystemScope scope) => this.scope = scope;

	public async Task<IToolkitHubClientConnection> ConnectToClient(string hubUrl)
	{
		if (connections.TryGetValue(hubUrl, out var connection))
		{
			return connection;
		}

		connection = scope.Resolve<IToolkitHubClientConnection>();

		await connection.Connect(hubUrl);

		connections.TryAdd(hubUrl, connection);

		return connection;
	}

	public async ValueTask DisposeAsync()
	{
		foreach (var connection in connections.Values)
		{
			await connection.DisposeAsync();
		}
	}

	public void RemoveHubFromConnections(string hubUrl)
	{
		connections.TryRemove(hubUrl, out _);
	}

	public async Task<ConnectionResult> TryToConnectToClient(string hubUrl, Action? onConnectionLost = null)
	{
		if (connections.TryGetValue(hubUrl, out var connection))
		{
			return new ConnectionResult(true, connection);
		}

		connection = scope.Resolve<IToolkitHubClientConnection>();

		var result = await connection.TryToConnect(
			hubUrl,
			() =>
			{
				RemoveHubFromConnections(hubUrl);

				onConnectionLost?.Invoke();
			}
		);

		if (!result)
		{
			return new ConnectionResult(false);
		}

		connections.TryAdd(hubUrl, connection);

		return new ConnectionResult(true, connection);
	}
}
