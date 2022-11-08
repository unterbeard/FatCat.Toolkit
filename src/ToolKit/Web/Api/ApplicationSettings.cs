using System.Reflection;

namespace FatCat.Toolkit.Web.Api;

public delegate Task<string?> ClientHubMessage(int messageId, string data);

public class ApplicationSettings : EqualObject
{
	public string? CertificationLocation { get; set; }

	public string? CertificationPassword { get; set; }

	public List<Assembly> ContainerAssemblies { get; set; } = new();

	public List<Uri> CorsUri { get; set; } = new();

	public Action? OnWebApplicationStarted { get; set; }

	public WebApplicationOptions Options { get; set; }

	public ushort Port { get; set; } = 443;

	public string SignalRPath { get; set; } = "/api/events";

	public event ClientHubMessage? ClientHubMessage;

	public virtual Task<string?> OnOnClientHubMessage(int messageId, string data) => ClientHubMessage?.Invoke(messageId, data)!;
}