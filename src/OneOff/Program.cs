using System.Reflection;
using Autofac;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
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
			SystemScope.Initialize(
				new ContainerBuilder(),
				new List<Assembly>
				{
					typeof(OneOffModule).Assembly,
					typeof(Program).Assembly,
					typeof(ConsoleLog).Assembly
				},
				ScopeOptions.SetLifetimeScope
			);

			if (args.Any() && args[0].Equals("client", StringComparison.OrdinalIgnoreCase))
			{
				ConnectClient(args);
			}
			else
			{
				RunServer(args);
			}

			await Task.Delay(500.Milliseconds());
		}
		catch (Exception ex)
		{
			ConsoleLog.WriteException(ex);
		}
	}

	private static void ConnectClient(string[] args)
	{
		var consoleUtilities = SystemScope.Container.Resolve<IConsoleUtilities>();

		var clientWorker = SystemScope.Container.Resolve<ClientWorker>();

		clientWorker.DoWork(args);

		consoleUtilities.WaitForExit();
	}

	private static void RunServer(string[] args)
	{
		new ServerWorker().DoWork(args);
	}
}
