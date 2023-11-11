using System.Collections.Concurrent;

namespace FatCat.Toolkit.Web;

internal static class HttpClientFactory
{
	private static readonly ConcurrentDictionary<TimeSpan, HttpClient> clients = new();

	public static HttpClient GetWithTimeout(TimeSpan timeout)
	{
		if (clients.TryGetValue(timeout, out var client))
		{
			return client;
		}

		var newClient = new HttpClient { Timeout = timeout };

		clients.TryAdd(timeout, newClient);

		return newClient;
	}
}
