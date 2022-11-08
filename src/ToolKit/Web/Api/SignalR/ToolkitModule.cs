using Autofac;
using FatCat.Toolkit.Console;

namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		ConsoleLog.WriteDarkYellow("ToolkitModule.Load");

		builder.RegisterType<ToolkitHubConnectionFactory>()
				.As<IToolkitHubConnectionFactory>()
				.SingleInstance();
	}
}