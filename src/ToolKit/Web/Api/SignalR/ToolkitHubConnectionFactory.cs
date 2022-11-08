using System.Collections.Concurrent;
using FatCat.Toolkit.Injection;

namespace FatCat.Toolkit.Web.Api.SignalR;

public interface IToolkitHubConnectionFactory : IAsyncDisposable
{
	Task<IToolkitHubConnection> Connect(string hubUrl);
}

public class ToolkitHubConnectionFactory : IToolkitHubConnectionFactory
{
	private readonly ConcurrentDictionary<string, IToolkitHubConnection> connections = new();
	private readonly ISystemScope scope;

	public ToolkitHubConnectionFactory(ISystemScope scope) => this.scope = scope;

	public async Task<IToolkitHubConnection> Connect(string hubUrl)
	{
		if (connections.TryGetValue(hubUrl, out var connection)) return connection;

		connection = scope.Resolve<IToolkitHubConnection>();

		await connection.Connect(hubUrl);

		connections.TryAdd(hubUrl, connection);

		return connection;
	}

	public async ValueTask DisposeAsync()
	{
		foreach (var connection in connections.Values) await connection.DisposeAsync();
	}
}