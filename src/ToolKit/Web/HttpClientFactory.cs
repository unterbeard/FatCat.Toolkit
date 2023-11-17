namespace FatCat.Toolkit.Web;

public static class HttpClientFactory
{
	private static HttpClient client = new();
	private static HttpClientHandler clientHandler;

	public static HttpClient Get()
	{
		return client;
	}

	public static void UseHttpClientHandler(HttpClientHandler handler = null)
	{
		clientHandler = handler ?? new HttpClientHandler();

		handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

		client = new HttpClient(clientHandler);
	}
}
