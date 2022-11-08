using System.Reflection;
using Autofac;
using FatCat.Toolkit;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.Web.Api;
using FatCat.Toolkit.Web.Api.SignalR;
using Humanizer;

namespace OneOff;

public static class Program
{
	private const int WebPort = 14555;

	public static async Task Main(params string[] args)
	{
		ConsoleLog.LogCallerInformation = true;

		try
		{
			var isClient = args.Any() && args.Any(i => i.Equals("client", StringComparison.OrdinalIgnoreCase));

			if (isClient) ConnectClient();
			else RunServer();
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}

	private static void ConnectClient()
	{
		SystemScope.Initialize(new ContainerBuilder(), ScopeOptions.SetLifetimeScope);

		var consoleUtilities = SystemScope.Container.Resolve<IConsoleUtilities>();

		var thread = SystemScope.Container.Resolve<IThread>();
		var generator = SystemScope.Container.Resolve<IGenerator>();
		var hubFactory = SystemScope.Container.Resolve<IToolkitHubConnectionFactory>();

		thread.Run(async () =>
					{
						var hubUrl = $"https://localhost:{WebPort}/api/events";

						var hubConnection = await hubFactory.Connect(hubUrl);

						await thread.Sleep(1.Seconds());

						await hubConnection.Send(31, "Hello World");

						thread.Run(async () =>
									{
										await thread.Sleep(3.Seconds());

										var secondConnection = await hubFactory.Connect(hubUrl);

										await secondConnection.Send(32, "More Message");
									});
					});

		consoleUtilities.WaitForExit();
	}

	private static void RunServer()
	{
		var applicationSettings = new ApplicationSettings
								{
									Options = WebApplicationOptions.UseHttps | WebApplicationOptions.UseSignalR,
									CertificationLocation = @"C:\DevelopmentCert\DevelopmentCert.pfx",
									CertificationPassword = "basarab_cert",
									Port = WebPort,
									ContainerAssemblies = new List<Assembly> { Assembly.GetExecutingAssembly() },
									CorsUri = new List<Uri> { new($"https://localhost:{WebPort}") },
									OnWebApplicationStarted = Started
								};

		WebApplication.Run(applicationSettings);
	}

	private static void Started() { ConsoleLog.WriteGreen("Hey the web application has started!!!!!"); }
}