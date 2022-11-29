using Autofac.AspNetCore.Extensions;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Enumerations;
using FatCat.Toolkit.Injection;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

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
																			if (Settings.Options.IsFlagSet(WebApplicationOptions.UseHttps)) o.UseHttps(Settings.CertificationLocation ?? throw new InvalidOperationException(), Settings.CertificationPassword);
																		});
								})
					.UseStartup(typeof(ApplicationStartUp))
					.Build();

		host.Run();
	}
}