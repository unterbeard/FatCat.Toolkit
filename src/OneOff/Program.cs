using System.Diagnostics;
using System.Reflection;
using Autofac;
using FatCat.Fakes;
using FatCat.Toolkit;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.Web.Api;
using FatCat.Toolkit.Web.Api.SignalR;
using Humanizer;
using Newtonsoft.Json;

namespace OneOff;

public static class Program
{
	private const int WebPort = 14555;

	public static void Main(params string[] args)
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

						await hubConnection.SendNoResponse(new ToolkitMessage
															{
																MessageId = 5,
																Data = $"Some Data {Faker.RandomString()}"
															});

						thread.Run(async () =>
									{
										await thread.Sleep(3.Seconds());

										var secondConnection = await hubFactory.Connect(hubUrl);

										await secondConnection.SendNoResponse(new ToolkitMessage
																			{
																				MessageId = 2,
																				Data = $"Hello World {Faker.RandomString()}"
																			});

										await thread.Sleep(3.Seconds());

										ConsoleLog.WriteCyan("Going to send a message and wait for a response");

										var watch = Stopwatch.StartNew();

										var response = await secondConnection.Send(new ToolkitMessage
																					{
																						MessageId = 2 * 1000,
																						Data = $"Go More {Faker.RandomString()}"
																					});

										watch.Stop();

										ConsoleLog.WriteMagenta($"Response: {JsonConvert.SerializeObject(response)}");
										ConsoleLog.WriteGreen($"Took <{watch.Elapsed}>");

										consoleUtilities.Exit();
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
									OnWebApplicationStarted = Started,
								};

		applicationSettings.ClientHubMessage += (messageId, data) =>
												{
													ConsoleLog.WriteDarkCyan($"MessageId <{messageId}> | Data <{data}>");

													return Task.FromResult($"MessageBack {DateTime.Now:h:mm:ss tt zzs}");
												};

		WebApplication.Run(applicationSettings);
	}

	private static void Started() { ConsoleLog.WriteGreen("Hey the web application has started!!!!!"); }
}