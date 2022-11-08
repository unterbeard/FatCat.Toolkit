using Autofac.AspNetCore.Extensions;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Enumerations;
using FatCat.Toolkit.Injection;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace FatCat.Toolkit.Web.Api;

public static class WebApplication
{
	public static ApplicationSettings Settings { get; private set; } = null!;
	
	public static void Run(ApplicationSettings settings)
	{
		Settings = settings;

		SystemScope.ContainerAssemblies = settings.ContainerAssemblies;
		
		ConsoleLog.WriteDarkYellow($"Running application");
		ConsoleLog.WriteDarkYellow($"    CertificationLocation := {Settings.CertificationLocation}");
		ConsoleLog.WriteDarkYellow($"    CertificationPassword := {Settings.CertificationPassword}");
		ConsoleLog.WriteDarkYellow($"    CorsUri               := {JsonConvert.SerializeObject(settings.CorsUri)}");
		ConsoleLog.WriteDarkYellow($"    Options               := {Settings.Options}");
		ConsoleLog.WriteDarkYellow($"    Port                  := {Settings.Port}");
		
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