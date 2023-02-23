using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ProxySpike.Options;

namespace ProxySpike.Workers.ProxyPlaying;

/*
 *	What do we need to test to ensure we can use this as a proxy?
 *		- Redirect of get, post, and delete
 *		- Redirect of Meta Data to a different endpoint than everything else
 *		- Can do we logic in the redirect to completely replace what NetworkManager is dong?
 *
 *	How to configure in code rather than JSON
 * 
 * 
 */

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