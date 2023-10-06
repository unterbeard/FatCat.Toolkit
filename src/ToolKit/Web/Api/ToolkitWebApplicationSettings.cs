#nullable enable
using System.Reflection;
using FatCat.Toolkit.Web.Api.SignalR;

namespace FatCat.Toolkit.Web.Api;

public class ToolkitWebApplicationSettings : EqualObject
{
	public string[] Args { get; set; } = Array.Empty<string>();

	public List<Assembly> ContainerAssemblies { get; set; } = new();

	public Action? OnWebApplicationStarted { get; set; }

	public WebApplicationOptions Options { get; set; } =
		WebApplicationOptions.Cors | WebApplicationOptions.HttpsRedirection;

	public string SignalRPath { get; set; } = "/api/events";

	public string? StaticFileLocation { get; set; }

	public CertificationSettings? TlsCertificate { get; set; }

	public IToolkitTokenParameters? ToolkitTokenParameters { get; set; }

	public event ToolkitHubClientConnected ClientConnected;

	public event ToolkitHubDataBufferMessage ClientDataBufferMessage;

	public event ToolkitHubClientDisconnected ClientDisconnected;

	public event ToolkitHubMessage ClientMessage;

	public Task OnClientConnected(ToolkitUser user, string connectionId) =>
		ClientConnected?.Invoke(user, connectionId);

	public Task OnClientDisconnected(ToolkitUser user, string connectionId) =>
		ClientDisconnected?.Invoke(user, connectionId);

	public Task<string?> OnClientHubMessage(ToolkitMessage message) => ClientMessage?.Invoke(message)!;

	public Task<string?> OnOnClientDataBufferMessage(ToolkitMessage message, byte[] dataBuffer) =>
		ClientDataBufferMessage?.Invoke(message, dataBuffer)!;
}
