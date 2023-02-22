using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ProxySpike.Options;

namespace ProxySpike.Workers.ProxyPlaying;

public class ProxyWorker : ISpikeWorker<ProxyOptions>
{
	public Task DoWork(ProxyOptions options)
	{
		// Create a Kestrel web server, and tell it to use the Startup class
		// for the service configuration
		var args = Array.Empty<string>();

		var myHostBuilder = Host.CreateDefaultBuilder(args);

		myHostBuilder.ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<ProxyStartUp>());

		var myHost = myHostBuilder.Build();

		myHost.Run();

		return Task.CompletedTask;
	}
}