using System.Net;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Data;
using FatCat.Toolkit.Data.Mongo;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Web;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using NullValueHandling = Newtonsoft.Json.NullValueHandling;

namespace FatCat.Toolkit.Communication;

public interface IWebCaller
{
	Uri BaseUri { get; }

	Task<WebResult> Delete(string url);

	Task<WebResult> Delete(string url, TimeSpan timeout);

	Task<WebResult> Get(string url);

	Task<WebResult> Get(string url, TimeSpan timeout);

	Task<WebResult> Post<T>(string url, T data) where T : EqualObject;

	Task<WebResult> Post<T>(string url, List<T> data) where T : EqualObject;

	Task<WebResult> Post(string url);

	Task<WebResult> Post(string url, string data);

	Task<WebResult> Post<T>(string url, T data, TimeSpan timeout) where T : EqualObject;

	Task<WebResult> Post<T>(string url, List<T> data, TimeSpan timeout) where T : EqualObject;

	Task<WebResult> Post(string url, TimeSpan timeout);

	Task<WebResult> Post(string url, string data, TimeSpan timeout);
}

public class WebCaller : IWebCaller
{
	public static TimeSpan DefaultTimeout { get; set; } = 30.Seconds();

	private readonly IToolkitLogger logger;

	public Uri BaseUri { get; }

	static WebCaller()
	{
		FlurlHttp.Configure(settings =>
							{
								var jsonSettings = new JsonSerializerSettings
													{
														NullValueHandling = NullValueHandling.Ignore,
														Converters = new List<JsonConverter>
																	{
																		new StringEnumConverter(),
																		new ObjectIdConverter()
																	}
													};

								settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
								settings.Timeout = DefaultTimeout;
							});
	}

	public WebCaller(Uri uri,
					IToolkitLogger logger)
	{
		this.logger = logger;
		BaseUri = uri;
	}

	public async Task<WebResult> Delete(string url) => await Delete(url, DefaultTimeout);

	public async Task<WebResult> Delete(string url, TimeSpan timeout)
	{
		try
		{
			var response = await CreateRequest(url)
								.WithTimeout(timeout)
								.DeleteAsync();

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException) { return WebResult.Timeout(); }
		catch (FlurlHttpException ex) { return ex.StatusCode == null ? WebResult.NotFound() : new WebResult((HttpStatusCode)ex.StatusCode, ex.Message); }
		catch (Exception ex)
		{
			logger.Error($"Exception of type of {ex.GetType().FullName}");

			throw;
		}
	}

	public Task<WebResult> Get(string url) => Get(url, DefaultTimeout);

	public async Task<WebResult> Get(string url, TimeSpan timeout)
	{
		try
		{
			ConsoleLog.WriteGreen($"Going to attempt to call {url}");

			var response = await CreateRequest(url)
								.WithTimeout(timeout)
								.GetAsync();

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException) { return WebResult.Timeout(); }
		catch (FlurlHttpException ex) { return ex.StatusCode == null ? WebResult.NotFound() : new WebResult((HttpStatusCode)ex.StatusCode, ex.Message); }
		catch (Exception ex)
		{
			logger.Error($"Exception of type of {ex.GetType().FullName}");

			throw;
		}
	}

	public Task<WebResult> Post<T>(string url, T data) where T : EqualObject => Post(url, data, DefaultTimeout);

	public Task<WebResult> Post<T>(string url, List<T> data) where T : EqualObject => Post(url, data, DefaultTimeout);

	public Task<WebResult> Post(string url) => Post(url, DefaultTimeout);

	public Task<WebResult> Post(string url, string data) => Post(url, data, DefaultTimeout);

	public async Task<WebResult> Post<T>(string url, T data, TimeSpan timeout) where T : EqualObject
	{
		try
		{
			var response = await CreateRequest(url)
								.WithTimeout(timeout)
								.PostJsonAsync(data);

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException) { return WebResult.Timeout(); }
		catch (FlurlHttpException ex) { return ex.StatusCode == null ? WebResult.NotFound() : new WebResult((HttpStatusCode)ex.StatusCode, ex.Message); }
		catch (Exception ex)
		{
			logger.Error($"Exception of type of {ex.GetType().FullName}");

			throw;
		}
	}

	public async Task<WebResult> Post<T>(string url, List<T> data, TimeSpan timeout) where T : EqualObject
	{
		try
		{
			var response = await CreateRequest(url)
								.WithTimeout(timeout)
								.PostJsonAsync(data);

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException) { return WebResult.Timeout(); }
		catch (FlurlHttpException ex) { return ex.StatusCode == null ? WebResult.NotFound() : new WebResult((HttpStatusCode)ex.StatusCode, ex.Message); }
		catch (Exception ex)
		{
			logger.Error($"Exception of type of {ex.GetType().FullName}");

			throw;
		}
	}

	public async Task<WebResult> Post(string url, TimeSpan timeout)
	{
		try
		{
			var response = await CreateRequest(url)
								.WithTimeout(timeout)
								.PostAsync();

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException) { return WebResult.Timeout(); }
		catch (FlurlHttpException ex) { return ex.StatusCode == null ? WebResult.NotFound() : new WebResult((HttpStatusCode)ex.StatusCode, ex.Message); }
		catch (Exception ex)
		{
			logger.Error($"Exception of type of {ex.GetType().FullName}");

			throw;
		}
	}

	public async Task<WebResult> Post(string url, string data, TimeSpan timeout)
	{
		try
		{
			var response = await CreateRequest(url)
								.WithTimeout(timeout)
								.PostJsonAsync(data);

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException) { return WebResult.Timeout(); }
		catch (FlurlHttpException ex) { return ex.StatusCode == null ? WebResult.NotFound() : new WebResult((HttpStatusCode)ex.StatusCode, ex.Message); }
		catch (Exception ex)
		{
			logger.Error($"Exception of type of {ex.GetType().FullName}");

			throw;
		}
	}

	private Url CreateRequest(string pathSegment)
	{
		var url = new Url(BaseUri);

		return url.AppendPathSegment(pathSegment);
	}
}