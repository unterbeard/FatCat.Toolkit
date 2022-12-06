using Autofac;
using FatCat.Toolkit.Caching;

namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<ToolkitHubClientFactory>()
				.As<IToolkitHubClientFactory>()
				.SingleInstance();

		builder.RegisterType<ToolkitHubServer>()
				.As<IToolkitHubServer>()
				.SingleInstance();

		builder.RegisterGeneric(typeof(FatCatCache<>))
				.As(typeof(IFatCatCache<>))
				.SingleInstance();
	}
}