using System.Reflection;

namespace FatCat.Toolkit.Web.Api;

public class ApplicationSettings : EqualObject
{
	public string? CertificationLocation { get; set; }

	public string? CertificationPassword { get; set; }

	public List<Assembly> ContainerAssemblies { get; set; } = new();

	public List<Uri> CorsUri { get; set; } = new();

	public WebApplicationOptions Options { get; set; }

	public ushort Port { get; set; } = 443;
}