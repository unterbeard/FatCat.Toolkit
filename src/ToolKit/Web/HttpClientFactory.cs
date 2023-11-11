namespace FatCat.Toolkit.Web;

internal static class HttpClientFactory
{
	private static readonly HttpClient client = new();

	public static HttpClient Get()
	{
		return client;
	}

	public static HttpClient GetWithTimeout(TimeSpan timeout)
	{
		// client.Timeout = timeout;

		return client;
	}
}
