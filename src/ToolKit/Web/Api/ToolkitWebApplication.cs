using Autofac.AspNetCore.Extensions;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.Injection;
using Microsoft.AspNetCore.Hosting;

namespace FatCat.Toolkit.Web.Api;

public static class ToolkitWebApplication
{
	public static ToolkitWebApplicationSettings Settings { get; private set; } = null!;

	public static void Run(ToolkitWebApplicationSettings settings)
	{
		Settings = settings;

		SystemScope.ContainerAssemblies = settings.ContainerAssemblies;

		var host = new WebHostBuilder()
					.UseAutofac()
					.UseKestrel(options =>
								{
									options.AllowSynchronousIO = false;

									options.ListenAnyIP(Settings.Port, o =>
																		{
																			// if (Settings.Options.IsFlagSet(WebApplicationOptions.UseHttps)) o.UseHttps(Settings.TlsCertificate?.Location ?? throw new InvalidOperationException(), Settings.TlsCertificate.Password);
																		});
								})
					.UseStartup(typeof(ApplicationStartUp))
					.Build();

		host.Run();
	}
}