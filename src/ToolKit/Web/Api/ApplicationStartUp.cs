using Autofac;
using Autofac.Extensions.DependencyInjection;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.Web.Api.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FatCat.Toolkit.Web.Api;

internal class ApplicationStartUp
{
	public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
	{
		app.Use(CaptureMiddlewareExceptions);

		app.UseFileServer();

		SetUpStaticFiles(app);

		app.UseHttpsRedirection();

		app.UseRouting();

		app.UseCors("CorsPolicy");

		if (ToolkitWebApplication.Settings.Options.IsFlagSet(WebApplicationOptions.UseAuthentication))
		{
			ConsoleLog.WriteMagenta("Adding Authentication?????????????????????");

			app.UseAuthentication();
			app.UseAuthorization();
		}

		app.UseEndpoints(endpoints => endpoints.MapControllers());

		app.Use(async (context, next) => await next.Invoke());

		SystemScope.Container.LifetimeScope = app.ApplicationServices.GetAutofacRoot();

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

			AddAuthentication(services);

			services.AddSignalR(options =>
								{
									options.MaximumReceiveMessageSize = int.MaxValue;
									options.EnableDetailedErrors = true;
								});
		}
		catch (Exception ex)
		{
			if (SystemScope.Container.TryResolve<IToolkitLogger>(out var logger)) logger!.Exception(ex);
		}
	}

	private void AddAuthentication(IServiceCollection services)
	{
		if (ToolkitWebApplication.Settings.Options.IsFlagNotSet(WebApplicationOptions.UseAuthentication)) return;

		if (ToolkitWebApplication.Settings.ToolkitTokenParameters == null) throw new NullReferenceException(nameof(ToolkitWebApplication.Settings.ToolkitTokenParameters));

		ConsoleLog.WriteMagenta("Adding Authentication");

		var authenticationBuilder =
			services
				.AddAuthentication(options =>
									{
										options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
										options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
										options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
									})
				.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

		authenticationBuilder
			.AddJwtBearer(options =>
						{
							options.RequireHttpsMetadata = false;
							options.SaveToken = true;

							var toolkitTokenParameters = ToolkitWebApplication.Settings.ToolkitTokenParameters!;

							options.TokenValidationParameters = toolkitTokenParameters.Get();

							options.Events = OAuthExtensions.GetTokenBearerEvents();
						});

		services
			.AddAuthorization(options =>
							{
								// options.AddServerToServerPolicy();
								options.AddPermissionsPolicies();
							});
	}

	private void AddCors(IServiceCollection services)
	{
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

		var localHostCors = originTemplate.Replace("+", "localhost");
		var machineCors = originTemplate.Replace("+", Environment.MachineName.ToLower());
		var domainCors = originTemplate.Replace("+", applicationTools.GetHost().ToLower());

		var originsToAdd = new List<string>
							{
								localHostCors,
								machineCors,
								domainCors
							};

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

			if (SystemScope.Container.TryResolve<IToolkitLogger>(out var logger)) logger!.Information($"Could not complete call to {displayUrl}");
		}
		catch (Exception e)
		{
			var displayUrl = context.Request.GetDisplayUrl();

			if (SystemScope.Container.TryResolve<IToolkitLogger>(out var logger))
			{
				logger!.Warning($"Error calling {displayUrl}");
				logger!.Exception(e);
			}

			throw;
		}
	}

	private void ConfigureControllers(IServiceCollection services)
	{
		var builder = services.AddControllers(config =>
											{
												if (ToolkitWebApplication.Settings.Options.IsFlagSet(WebApplicationOptions.UseAuthentication))
												{
													var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
																.RequireAuthenticatedUser()
																.Build();

													config.Filters.Add(new AuthorizeFilter(policy));
												}
											})
							.AddNewtonsoftJson(build => { build.SerializerSettings.Converters.Add(new StringEnumConverter()); });

		var applicationParts = builder.PartManager.ApplicationParts;

		foreach (var assembly in ToolkitWebApplication.Settings.ContainerAssemblies) applicationParts.Add(new AssemblyPart(assembly));
	}

	private void SetUpSignalR(IApplicationBuilder app)
	{
		if (ToolkitWebApplication.Settings.Options.IsFlagNotSet(WebApplicationOptions.UseSignalR)) return;

		app.UseEndpoints(endpoints =>
						{
							var endpointOption = endpoints.MapHub<ToolkitHub>(ToolkitWebApplication.Settings.SignalRPath);

							if (ToolkitWebApplication.Settings.Options.IsFlagSet(WebApplicationOptions.UseAuthentication)) endpointOption.RequireAuthorization();
						});
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