using Autofac;

namespace FatCat.Toolkit.WebServer.SignalR;

public class ToolkitModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<ToolkitHubClientFactory>().As<IToolkitHubClientFactory>().SingleInstance();

		builder.RegisterType<ToolkitHubServer>().As<IToolkitHubServer>().SingleInstance();
	}
}
