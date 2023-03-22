using Autofac;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Threading;

namespace OneOff;

public static class Program
{
	private const int WebPort = 14555;

	public static void Main(params string[] args)
	{
		ConsoleLog.LogCallerInformation = true;

		try
		{
			SystemScope.Initialize(new ContainerBuilder(), ScopeOptions.SetLifetimeScope);

			var thread = SystemScope.Container.Resolve<IThread>();

			thread.Run(async () =>
						{
							var worker = SystemScope.Container.Resolve<WebCallerWorker>();

							await worker.DoWork();
						});

			RunServer();

			// if (args.Any() && args[0].Equals("client", StringComparison.OrdinalIgnoreCase)) ConnectClient();
			// else RunServer();
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}

	private static void ConnectClient()
	{
		var consoleUtilities = SystemScope.Container.Resolve<IConsoleUtilities>();

		var clientWorker = SystemScope.Container.Resolve<ClientWorker>();

		clientWorker.DoWork(WebPort);

		consoleUtilities.WaitForExit();
	}

	private static void RunServer() => new ServerWorker().DoWork(WebPort);
}