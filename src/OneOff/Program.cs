using Autofac;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;

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

		var clientWorker = SystemScope.Container.Resolve<ClientWorker>();

		clientWorker.DoWork(WebPort);

		consoleUtilities.WaitForExit();
	}

	private static void RunServer() => new ServerWorker().DoWork(WebPort);
}