using System.Net.Http.Headers;
using System.Text;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;
using Humanizer;

namespace FatCat.Toolkit.Web;

public interface IWebCaller
{
	string AcceptHeader { get; set; }

	Uri BaseUri { get; }

	TimeSpan Timeout { get; set; }

	Task<WebResult> Delete(string url);

	Task<WebResult> Delete(string url, TimeSpan timeout);

	Task<WebResult> Get(string url);

	Task<WebResult> Get(string url, TimeSpan timeout);

	Task<WebResult> Post<T>(string url, T data);

	Task<WebResult> Post<T>(string url, List<T> data);

	Task<WebResult> Post(string url);

	Task<WebResult> Post(string url, string data);

	Task<WebResult> Post(string url, string data, string contentType);

	Task<WebResult> Post<T>(string url, T data, TimeSpan timeout);

	Task<WebResult> Post<T>(string url, List<T> data, TimeSpan timeout);

	Task<WebResult> Post(string url, TimeSpan timeout);

	Task<WebResult> Post(string url, string data, TimeSpan timeout, string contentType);

	void UserBearerToken(string token);
}

public class WebCaller : IWebCaller
{
	private readonly IJsonOperations jsonOperations;
	private readonly IToolkitLogger logger;

	private string bearerToken;

	public string AcceptHeader { get; set; } = "application/json";

	public Uri BaseUri { get; }

	public TimeSpan Timeout { get; set; } = 30.Seconds();

	public WebCaller(Uri uri, IJsonOperations jsonOperations, IToolkitLogger logger)
	{
		this.jsonOperations = jsonOperations;
		this.logger = logger;
		BaseUri = uri;
	}

	public async Task<WebResult> Delete(string url)
	{
		return await Delete(url, Timeout);
	}

	public async Task<WebResult> Delete(string url, TimeSpan timeout)
	{
		return await SendWebRequest(HttpMethod.Delete, url, timeout);
	}

	public Task<WebResult> Get(string url)
	{
		return Get(url, Timeout);
	}

	public async Task<WebResult> Get(string url, TimeSpan timeout)
	{
		return await SendWebRequest(HttpMethod.Get, url, timeout);
	}

	public Task<WebResult> Post<T>(string url, T data)
	{
		return Post(url, data, Timeout);
	}

	public Task<WebResult> Post<T>(string url, List<T> data)
	{
		return Post(url, data, Timeout);
	}

	public Task<WebResult> Post(string url)
	{
		return Post(url, Timeout);
	}

	public Task<WebResult> Post(string url, string data)
	{
		return Post(url, data, Timeout);
	}

	public async Task<WebResult> Post(string url, string data, string contentType)
	{
		return await Post(url, data, Timeout, contentType);
	}

	public async Task<WebResult> Post<T>(string url, T data, TimeSpan timeout)
	{
		var json = jsonOperations.Serialize(data);

		return await SendWebRequest(HttpMethod.Post, url, timeout, json, "application/json");
	}

	public async Task<WebResult> Post<T>(string url, List<T> data, TimeSpan timeout)
	{
		var json = jsonOperations.Serialize(data);

		return await SendWebRequest(HttpMethod.Post, url, timeout, json, "application/json");
	}

	public async Task<WebResult> Post(string url, TimeSpan timeout)
	{
		return await SendWebRequest(HttpMethod.Post, url, timeout);
	}

	public async Task<WebResult> Post(string url, string data, TimeSpan timeout, string contentType)
	{
		return await SendWebRequest(HttpMethod.Post, url, timeout, data, contentType);
	}

	public async Task<WebResult> Post(string url, string data, TimeSpan timeout)
	{
		return await SendWebRequest(HttpMethod.Post, url, timeout, data);
	}

	public void UserBearerToken(string token)
	{
		bearerToken = token;
	}

	private void EnsureBearerToken(HttpClient httpClient)
	{
		if (bearerToken is not null)
		{
			logger.Debug($"Adding Bearer Token := <{bearerToken}>");

			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
		}
	}

	private Uri GetFullUrl(string url)
	{
		return new Uri(BaseUri, url);
	}

	private async Task<WebResult> SendWebRequest(
		HttpMethod httpMethod,
		string url,
		TimeSpan timeout,
		string data = null,
		string contentType = null
	)
	{
		logger.Debug("Creating http client");

		var httpClient = HttpClientFactory.Get();

		EnsureBearerToken(httpClient);

		var requestUri = GetFullUrl(url);

		logger.Debug($"Creating request message to Uri := <{requestUri}>");

		var requestMessage = new HttpRequestMessage(httpMethod, requestUri);

		if (data.IsNotNullOrEmpty())
		{
			logger.Debug($"Adding data of length := <{data.Length}> | Content Type := <{contentType}>");

			requestMessage.Content = new StringContent(data, Encoding.UTF8, contentType);
		}

		logger.Debug($"Timeout is := <{timeout}>");

		using var tokenSource = new CancellationTokenSource(timeout);

		try
		{
			logger.Debug($"Sending request to <{requestUri}>");

			var response = await httpClient.SendAsync(requestMessage, tokenSource.Token);

			logger.Debug("Creating web result from response");

			var result = new WebResult(response);

			logger.Debug($"Request to <{requestUri}> | StatusCode := <{result.StatusCode}>");

			bearerToken = null;

			return result;
		}
		catch (TaskCanceledException)
		{
			logger.Debug($"Request to {requestUri} timed out");

			return WebResult.Timeout();
		}
	}
}
