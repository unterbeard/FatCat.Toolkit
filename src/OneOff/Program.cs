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
			SystemScope.Initialize(new ContainerBuilder(), ScopeOptions.SetLifetimeScope);

			var worker = SystemScope.Container.Resolve<CacheWorker>();

			worker.DoWork();
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