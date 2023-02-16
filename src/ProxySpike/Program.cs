using Autofac;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Threading;
using ProxySpike.Workers;

namespace ProxySpike;

public static class Program
{
	private static IConsoleUtilities consoleUtilities;

	public static void Main(params string[] args)
	{
		try
		{
			SystemScope.Initialize(new ContainerBuilder(), ScopeOptions.SetLifetimeScope);

			consoleUtilities = SystemScope.Container.Resolve<IConsoleUtilities>();

			ConsoleLog.Write("Starting Proxy Spike Program");

			var thread = SystemScope.Container.Resolve<IThread>();

			thread.Run(async () =>
						{
							var worker = SystemScope.Container.Resolve<WebServerWorker>();

							await worker.DoWork();
						});

			consoleUtilities.WaitForExit();
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}
}