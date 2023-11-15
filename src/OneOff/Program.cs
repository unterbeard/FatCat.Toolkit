using System.Reflection;
using Autofac;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.WebServer;
using OneOffLib;

namespace OneOff;

public static class Program
{
	private const int WebPort = 14555;

	public static string[] Args { get; set; }

	public static async Task Main(params string[] args)
	{
		await Task.CompletedTask;

		Args = args;

		ConsoleLog.LogCallerInformation = true;

		try
		{
			SystemScope.Initialize(
				new ContainerBuilder(),
				new List<Assembly>
				{
					typeof(OneOffModule).Assembly,
					typeof(Program).Assembly,
					typeof(ConsoleLog).Assembly,
					typeof(ToolkitWebServerModule).Assembly
				},
				ScopeOptions.SetLifetimeScope
			);

			RunServer(args);

			// var worker = SystemScope.Container.Resolve<UriWorker>();
			//
			// await worker.DoWork();

			var consoleUtilities = SystemScope.Container.Resolve<IConsoleUtilities>();

			consoleUtilities.WaitForExit();
		}
		catch (Exception ex)
		{
			ConsoleLog.WriteException(ex);
		}
	}

	private static void RunServer(string[] args)
	{
		new ServerWorker().DoWork(args);
	}
}
