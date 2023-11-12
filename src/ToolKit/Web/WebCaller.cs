using System.Net.Http.Headers;
using System.Text;
using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;
using Humanizer;

namespace FatCat.Toolkit.Web;

public interface IWebCaller
{
	Uri BaseUri { get; }

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
	private static TimeSpan DefaultTimeout { get; } = 30.Seconds();

	private readonly IJsonOperations jsonOperations;
	private readonly IToolkitLogger logger;

	private string bearerToken;

	public Uri BaseUri { get; }

	public WebCaller(Uri uri, IJsonOperations jsonOperations, IToolkitLogger logger)
	{
		this.jsonOperations = jsonOperations;
		this.logger = logger;
		BaseUri = uri;
	}

	public async Task<WebResult> Delete(string url)
	{
		return await Delete(url, DefaultTimeout);
	}

	public async Task<WebResult> Delete(string url, TimeSpan timeout)
	{
		var httpClient = HttpClientFactory.GetWithTimeout(timeout);

		EnsureBearerToken(httpClient);

		var response = await httpClient.DeleteAsync(GetFullUrl(url));

		return new WebResult(response);
	}

	public Task<WebResult> Get(string url)
	{
		return Get(url, DefaultTimeout);
	}

	public async Task<WebResult> Get(string url, TimeSpan timeout)
	{
		var httpClient = HttpClientFactory.GetWithTimeout(timeout);

		EnsureBearerToken(httpClient);

		var response = await httpClient.GetAsync(GetFullUrl(url));

		return new WebResult(response);
	}

	public Task<WebResult> Post<T>(string url, T data)
	{
		return Post(url, data, DefaultTimeout);
	}

	public Task<WebResult> Post<T>(string url, List<T> data)
	{
		return Post(url, data, DefaultTimeout);
	}

	public Task<WebResult> Post(string url)
	{
		return Post(url, DefaultTimeout);
	}

	public Task<WebResult> Post(string url, string data)
	{
		return Post(url, data, DefaultTimeout);
	}

	public async Task<WebResult> Post(string url, string data, string contentType)
	{
		return await Post(url, data, DefaultTimeout, contentType);
	}

	public async Task<WebResult> Post<T>(string url, T data, TimeSpan timeout)
	{
		var json = jsonOperations.Serialize(data);

		return await DoPostWithJson(url, timeout, json);
	}

	public async Task<WebResult> Post<T>(string url, List<T> data, TimeSpan timeout)
	{
		var json = jsonOperations.Serialize(data);

		return await DoPostWithJson(url, timeout, json);
	}

	public async Task<WebResult> Post(string url, TimeSpan timeout)
	{
		var httpClient = HttpClientFactory.GetWithTimeout(timeout);

		EnsureBearerToken(httpClient);

		var response = await httpClient.PostAsync(GetFullUrl(url), null);

		return new WebResult(response);
	}

	public async Task<WebResult> Post(string url, string data, TimeSpan timeout, string contentType)
	{
		var httpClient = HttpClientFactory.GetWithTimeout(timeout);

		EnsureBearerToken(httpClient);

		var response = await httpClient.PostAsync(
			GetFullUrl(url),
			new StringContent(data, Encoding.UTF8, contentType)
		);

		return new WebResult(response);
	}

	public async Task<WebResult> Post(string url, string data, TimeSpan timeout)
	{
		var httpClient = HttpClientFactory.GetWithTimeout(timeout);

		EnsureBearerToken(httpClient);

		var response = await httpClient.PostAsync(GetFullUrl(url), new StringContent(data, Encoding.UTF8));

		return new WebResult(response);
	}

	public void UserBearerToken(string token)
	{
		bearerToken = token;
	}

	private async Task<WebResult> DoPostWithJson(string url, TimeSpan timeout, string json)
	{
		var httpClient = HttpClientFactory.GetWithTimeout(timeout);

		EnsureBearerToken(httpClient);

		var response = await httpClient.PostAsync(
			GetFullUrl(url),
			new StringContent(json, Encoding.UTF8, "application/json")
		);

		return new WebResult(response);
	}

	private void EnsureBearerToken(HttpClient httpClient)
	{
		if (bearerToken is not null)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
		}
	}

	private Uri GetFullUrl(string url)
	{
		return new Uri(BaseUri, url);
	}
}
