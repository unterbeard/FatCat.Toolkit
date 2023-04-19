using System.Reflection;
using Autofac;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Threading;
using Humanizer;
using OneOffLib;

namespace OneOff;

public static class Program
{
	private const int WebPort = 14555;

	public static async Task Main(params string[] args)
	{
		ConsoleLog.LogCallerInformation = true;

		try
		{
			SystemScope.Initialize(new ContainerBuilder(),
									new List<Assembly>
									{
										typeof(OneOffModule).Assembly,
										typeof(Program).Assembly,
										typeof(ConsoleLog).Assembly
									},
									ScopeOptions.SetLifetimeScope);

			// var thread = SystemScope.Container.Resolve<IThread>();
			//
			// thread.Run(async () =>
			// 			{
			// 				var worker = SystemScope.Container.Resolve<ModuleLoaderWorker>();
			//
			// 				await worker.DoWork();
			// 			});

			//
			// RunServer();
			//
			// // if (args.Any() && args[0].Equals("client", StringComparison.OrdinalIgnoreCase)) ConnectClient();
			// // else RunServer();

			ConnectClient(args);

			await Task.Delay(500.Milliseconds());
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}

	private static void ConnectClient(string[] args)
	{
		var consoleUtilities = SystemScope.Container.Resolve<IConsoleUtilities>();

		var clientWorker = SystemScope.Container.Resolve<ClientWorker>();

		clientWorker.DoWork(args);

		consoleUtilities.WaitForExit();
	}

	private static void RunServer() => new ServerWorker().DoWork(WebPort);
}