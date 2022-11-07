using Autofac.AspNetCore.Extensions;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Enumerations;
using Microsoft.AspNetCore.Hosting;

namespace FatCat.Toolkit.Web.Api;

public static class WebApplication
{
	public static ApplicationSettings Settings { get; private set; } = null!;
	
	public static void Run(ApplicationSettings settings)
	{
		Settings = settings;
		
		var host = new WebHostBuilder()
					.UseAutofac()
					.UseKestrel(options =>
								{
									options.AllowSynchronousIO = false;

									options.ListenAnyIP(Settings.Port, o =>
																	{
																		if (Settings.Options.IsFlagSet(WebApplicationOptions.UseHttps))
																		{
																			o.UseHttps(Settings.CertificationLocation ?? throw new InvalidOperationException(), Settings.CertificationPassword);
																		}
																	});
								})
					.UseStartup(typeof(ApplicationStartUp))
					.Build();

		host.Run();
	}
}