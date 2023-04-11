using Autofac;
using Autofac.Extensions.DependencyInjection;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Threading;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FatCat.Toolkit.Web.Api;

public static class ToolkitWebApplication
{
	public static ToolkitWebApplicationSettings Settings { get; private set; } = null!;

	public static void Run(ToolkitWebApplicationSettings settings)
	{
		Settings = settings;

		SystemScope.ContainerAssemblies = settings.ContainerAssemblies;

		var builder = WebApplication.CreateBuilder(settings.Args);

		// Call UseServiceProviderFactory on the Host sub property 
		builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

		builder.Configuration.AddEnvironmentVariables("ToolkitKey_");

		var applicationStartUp = new ApplicationStartUp();

		applicationStartUp.ConfigureServices(builder.Services);

		// Call ConfigureContainer on the Host sub property 
		// Register services directly with Autofac here. Don't
		// call builder.Populate(), that happens in AutofacServiceProviderFactory.
		builder.Host.ConfigureContainer<ContainerBuilder>((a, b) => SystemScope.Initialize(b, Settings.ContainerAssemblies));

		var app = builder.Build();

		applicationStartUp.Configure(app, app.Environment, app.Services.GetRequiredService<ILoggerFactory>());

		// app.UseHttpsRedirection();
		app.UseCors();

		app.UseAuthorization();

		app.MapControllers();

		SystemScope.Container.LifetimeScope = app.Services.GetAutofacRoot();

		var thread = SystemScope.Container.Resolve<IThread>();

		thread.Run(async () =>
					{
						await thread.Sleep(300.Milliseconds());

						Settings.OnWebApplicationStarted?.Invoke();
					});

		app.Run();
	}
}