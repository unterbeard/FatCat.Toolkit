using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
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
using ILogger = Serilog.ILogger;

namespace FatCat.Toolkit.Web.Api;

public class ApplicationStartUp
{

	public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
	{
		app.Use(CaptureMiddlewareExceptions);

		app.UseFileServer();

		SetUpStaticFiles(app);

		app.UseHttpsRedirection();

		app.UseRouting();

		app.UseCors("CorsPolicy");

		app.UseEndpoints(endpoints => endpoints.MapControllers());

		SystemScope.Container.LifetimeScope = app.ApplicationServices.GetAutofacRoot();

		SetUpSignalR(app);
	}

	public void ConfigureContainer(ContainerBuilder builder) => SystemScope.Initialize(builder, new List<Assembly>
																								{
																									typeof(CommonModule).Assembly,
																									Assembly.GetEntryAssembly()!
																								});

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
														AddCorsForUri(ApplicationRunner.Uri, configurePolicy);

														if (Configuration.UserInterfaceDevelopmentPort != ushort.MaxValue) AddCorsForUri(new Uri($"http://localhost:{Configuration.UserInterfaceDevelopmentPort}/"), configurePolicy);
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

		foreach (var corsOrigin in originsToAdd) QuickLog.WriteDarkCyan($"   Using Origin <{corsOrigin}>");

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

			Logger.Information($"Could not complete call to {displayUrl}");
		}
		catch (Exception e)
		{
			var displayUrl = context.Request.GetDisplayUrl();

			Logger.Error($"Error calling {displayUrl}");
			Logger.Exception(e);

			throw;
		}
	}

	private void ConfigureControllers(IServiceCollection services)
	{
		var builder = services.AddControllers(config => { })
							.AddNewtonsoftJson(build => { build.SerializerSettings.Converters.Add(new StringEnumConverter()); });

		var applicationParts = builder.PartManager.ApplicationParts;

		applicationParts.Add(new AssemblyPart(typeof(ApplicationStartUp).Assembly));
		applicationParts.Add(new AssemblyPart(Configuration.ServiceAssembly));
	}

	private void SetUpSignalR(IApplicationBuilder app) => app.UseEndpoints(endpoints => { endpoints.MapHub<FogHub>("/api/events"); });

	private void SetUpStaticFiles(IApplicationBuilder app)
	{
		var applicationTools = new ApplicationTools();

		var rootWebFolder = Path.Combine(applicationTools.ExecutingDirectory!, "userInterface");

		QuickLog.WriteMagenta($"First WebRootFolder <{rootWebFolder}> | CurrentDirectory <{Directory.GetCurrentDirectory()}>");

		if (!Directory.Exists(rootWebFolder)) rootWebFolder = Path.Combine(Directory.GetCurrentDirectory(), "userInterface");
		if (!Directory.Exists(rootWebFolder)) rootWebFolder = Environment.GetEnvironmentVariable("FogHazeDevelopmentUiLocation");

		QuickLog.WriteDarkCyan($"WebRootFolder <{rootWebFolder}>");

		if (!Directory.Exists(rootWebFolder))
		{
			//throw new InvalidOperationException($"Folder does not exist: <{rootWebFolder}>");
			ConsoleLog.WriteRed($"Did not find a folder at <{rootWebFolder}>");

			return;
		}

		var physicalFileProvider = new PhysicalFileProvider(rootWebFolder);
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