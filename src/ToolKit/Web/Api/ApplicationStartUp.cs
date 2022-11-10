using Autofac;
using Autofac.Extensions.DependencyInjection;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Enumerations;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.Web.Api.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;

namespace FatCat.Toolkit.Web.Api;

public class ApplicationStartUp
{
	public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
	{
		ConsoleLog.WriteDarkMagenta("Application Start up Configure");

		app.Use(CaptureMiddlewareExceptions);

		app.UseFileServer();

		SetUpStaticFiles(app);

		app.UseHttpsRedirection();

		app.UseRouting();

		app.UseCors("CorsPolicy");

		app.UseEndpoints(endpoints => endpoints.MapControllers());

		ConsoleLog.WriteDarkMagenta("Before GetAutofacRoot");

		SystemScope.Container.LifetimeScope = app.ApplicationServices.GetAutofacRoot();

		ConsoleLog.WriteDarkMagenta("After GetAutofacRoot");

		SetUpSignalR(app);

		var thread = SystemScope.Container.Resolve<IThread>();

		thread.Run(() => ToolkitWebApplication.Settings.OnWebApplicationStarted?.Invoke());
	}

	public void ConfigureContainer(ContainerBuilder builder) => SystemScope.Initialize(builder, ToolkitWebApplication.Settings.ContainerAssemblies);

	public virtual void ConfigureServices(IServiceCollection services)
	{
		try
		{
			services.AddHttpContextAccessor();
			ConfigureControllers(services);
			AddCors(services);
			services.AddHttpClient();

			services.AddSignalR(options =>
								{
									options.MaximumReceiveMessageSize = int.MaxValue;
									options.EnableDetailedErrors = true;
								});
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}

	private void AddCors(IServiceCollection services)
	{
		ConsoleLog.Write("Start Adding Cors");

		services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

		services.AddCors(o =>
						{
							o.AddPolicy("CorsPolicy", configurePolicy =>
													{
														foreach (var corsUri in ToolkitWebApplication.Settings.CorsUri) AddCorsForUri(corsUri, configurePolicy);
													});
						});
	}

	private static void AddCorsForUri(Uri uri, CorsPolicyBuilder builder)
	{
		var applicationTools = new ApplicationTools();

		var originTemplate = $"{uri.Scheme}://+:{uri.Port}";

		ConsoleLog.WriteMagenta($"Adding Cores for <{originTemplate}>");

		var localHostCors = originTemplate.Replace("+", "localhost");
		var machineCors = originTemplate.Replace("+", Environment.MachineName.ToLower());
		var domainCors = originTemplate.Replace("+", applicationTools.GetHost().ToLower());

		var originsToAdd = new List<string>
							{
								localHostCors,
								machineCors,
								domainCors
							};

		foreach (var corsOrigin in originsToAdd) ConsoleLog.WriteDarkCyan($"   Using Origin <{corsOrigin}>");

		builder
			.WithOrigins(originsToAdd.ToArray())
			.SetIsOriginAllowed(_ => true)
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials();
	}

	private async Task CaptureMiddlewareExceptions(HttpContext context, Func<Task> next)
	{
		try { await next().ConfigureAwait(false); }
		catch (TaskCanceledException)
		{
			var displayUrl = context.Request.GetDisplayUrl();

			ConsoleLog.WriteCyan($"Could not complete call to {displayUrl}");
		}
		catch (Exception e)
		{
			var displayUrl = context.Request.GetDisplayUrl();

			ConsoleLog.WriteCyan($"Error calling {displayUrl}");
			ConsoleLog.WriteException(e);

			throw;
		}
	}

	private void ConfigureControllers(IServiceCollection services)
	{
		var builder = services.AddControllers(config => { })
							.AddNewtonsoftJson(build => { build.SerializerSettings.Converters.Add(new StringEnumConverter()); });

		var applicationParts = builder.PartManager.ApplicationParts;

		foreach (var assembly in ToolkitWebApplication.Settings.ContainerAssemblies) applicationParts.Add(new AssemblyPart(assembly));
	}

	private void SetUpSignalR(IApplicationBuilder app)
	{
		if (ToolkitWebApplication.Settings.Options.IsFlagNotSet(WebApplicationOptions.UseSignalR)) return;

		app.UseEndpoints(endpoints => endpoints.MapHub<ToolkitHub>(ToolkitWebApplication.Settings.SignalRPath));
	}

	private void SetUpStaticFiles(IApplicationBuilder app)
	{
		if (ToolkitWebApplication.Settings.Options.IsFlagNotSet(WebApplicationOptions.UseFileSystem)) return;
		if (ToolkitWebApplication.Settings.StaticFileLocation == null) return;

		var physicalFileProvider = new PhysicalFileProvider(ToolkitWebApplication.Settings.StaticFileLocation);
		var options = new DefaultFilesOptions { FileProvider = physicalFileProvider };

		app.UseDefaultFiles(options);
		app.UseStaticFiles(new StaticFileOptions { FileProvider = physicalFileProvider });
	}

	public class ValidateModelAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext actionContext)
		{
			if (actionContext.ModelState.IsValid == false) actionContext.Result = new BadRequestObjectResult(actionContext.ModelState);
		}
	}
}