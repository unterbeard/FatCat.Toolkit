namespace FatCat.Toolkit.Web;

internal static class HttpClientFactory
{
	private static readonly HttpClient client = new();

	public static HttpClient Get()
	{
		return client;
	}
}
