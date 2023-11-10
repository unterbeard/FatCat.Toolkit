using System.Net;
using System.Web;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Data.Mongo;
using FatCat.Toolkit.Logging;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NullValueHandling = Newtonsoft.Json.NullValueHandling;

namespace FatCat.Toolkit.Web;

public interface IWebCaller
{
	Uri BaseUri { get; }

	Task<WebResult> Delete(string url);

	Task<WebResult> Delete(string url, TimeSpan timeout);

	Task<WebResult> Get(string url);

	Task<WebResult> Get(string url, TimeSpan timeout);

	Task<WebResult> Post<T>(string url, T data)
		where T : EqualObject;

	Task<WebResult> Post<T>(string url, List<T> data)
		where T : EqualObject;

	Task<WebResult> Post(string url);

	Task<WebResult> Post(string url, string data);

	Task<WebResult> Post<T>(string url, T data, TimeSpan timeout)
		where T : EqualObject;

	Task<WebResult> Post<T>(string url, List<T> data, TimeSpan timeout)
		where T : EqualObject;

	Task<WebResult> Post(string url, TimeSpan timeout);

	Task<WebResult> Post(string url, string data, TimeSpan timeout);

	void UserBearerToken(string token);
}

public class WebCaller : IWebCaller
{
	public static TimeSpan DefaultTimeout { get; set; } = 30.Seconds();

	private readonly IToolkitLogger logger;

	private string bearerToken;

	public Uri BaseUri { get; }

	static WebCaller()
	{
		FlurlHttp.Configure(settings =>
		{
			var jsonSettings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				Converters = new List<Newtonsoft.Json.JsonConverter>
				{
					new StringEnumConverter(),
					new ObjectIdConverter()
				}
			};

			settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
			settings.Timeout = DefaultTimeout;
		});
	}

	public WebCaller(Uri uri, IToolkitLogger logger)
	{
		this.logger = logger;
		BaseUri = uri;
	}

	public async Task<WebResult> Delete(string url) => await Delete(url, DefaultTimeout);

	public async Task<WebResult> Delete(string url, TimeSpan timeout)
	{
		try
		{
			var request = CreateRequest(url).WithTimeout(timeout).AllowHttpStatus("*");

			if (bearerToken is not null)
			{
				request.WithOAuthBearerToken(bearerToken);
			}

			var response = await request.DeleteAsync();

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException)
		{
			return WebResult.Timeout();
		}
		catch (FlurlHttpException ex)
		{
			return ex.StatusCode == null
				? WebResult.NotFound()
				: new WebResult((HttpStatusCode)ex.StatusCode, ex.Message);
		}
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
			var request = CreateRequest(url).WithTimeout(timeout).AllowHttpStatus("*");

			logger.Debug($"Getting from Url <{request.Url}>");

			if (bearerToken is not null)
			{
				request.WithOAuthBearerToken(bearerToken);
			}

			var response = await request.GetAsync();

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException)
		{
			return WebResult.Timeout();
		}
		catch (FlurlHttpException ex)
		{
			ConsoleLog.WriteRed($"Furl HttpException: {ex.Message}");

			return ex.StatusCode == null
				? WebResult.NotFound()
				: new WebResult((HttpStatusCode)ex.StatusCode, ex.Message);
		}
		catch (Exception ex)
		{
			logger.Error($"Exception of type of {ex.GetType().FullName}");

			throw;
		}
	}

	public Task<WebResult> Post<T>(string url, T data)
		where T : EqualObject => Post(url, data, DefaultTimeout);

	public Task<WebResult> Post<T>(string url, List<T> data)
		where T : EqualObject => Post(url, data, DefaultTimeout);

	public Task<WebResult> Post(string url) => Post(url, DefaultTimeout);

	public Task<WebResult> Post(string url, string data) => Post(url, data, DefaultTimeout);

	public async Task<WebResult> Post<T>(string url, T data, TimeSpan timeout)
		where T : EqualObject
	{
		try
		{
			var request = CreateRequest(url).WithTimeout(timeout).AllowHttpStatus("*");

			if (bearerToken is not null)
			{
				request.WithOAuthBearerToken(bearerToken);
			}

			var response = await request.PostJsonAsync(data);

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException)
		{
			return WebResult.Timeout();
		}
		catch (FlurlHttpException ex)
		{
			return ex.StatusCode == null
				? WebResult.NotFound()
				: new WebResult((HttpStatusCode)ex.StatusCode, ex.Message);
		}
		catch (Exception ex)
		{
			logger.Error($"Exception of type of {ex.GetType().FullName}");

			throw;
		}
	}

	public async Task<WebResult> Post<T>(string url, List<T> data, TimeSpan timeout)
		where T : EqualObject
	{
		try
		{
			var request = CreateRequest(url).WithTimeout(timeout).AllowHttpStatus("*");

			if (bearerToken is not null)
			{
				request.WithOAuthBearerToken(bearerToken);
			}

			var response = await request.PostJsonAsync(data);

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException)
		{
			return WebResult.Timeout();
		}
		catch (FlurlHttpException ex)
		{
			return ex.StatusCode == null
				? WebResult.NotFound()
				: new WebResult((HttpStatusCode)ex.StatusCode, ex.Message);
		}
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
			var request = CreateRequest(url).WithTimeout(timeout).AllowHttpStatus("*");

			if (bearerToken is not null)
			{
				request.WithOAuthBearerToken(bearerToken);
			}

			var response = await request.PostAsync();

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException)
		{
			return WebResult.Timeout();
		}
		catch (FlurlHttpException ex)
		{
			return ex.StatusCode == null
				? WebResult.NotFound()
				: new WebResult((HttpStatusCode)ex.StatusCode, ex.Message);
		}
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
			var request = CreateRequest(url).WithTimeout(timeout).AllowHttpStatus("*");

			if (bearerToken is not null)
			{
				request.WithOAuthBearerToken(bearerToken);
			}

			var response = await request.PostJsonAsync(data);

			return new WebResult(response.ResponseMessage);
		}
		catch (FlurlHttpTimeoutException)
		{
			return WebResult.Timeout();
		}
		catch (FlurlHttpException ex)
		{
			return ex.StatusCode == null
				? WebResult.NotFound()
				: new WebResult((HttpStatusCode)ex.StatusCode, ex.Message);
		}
		catch (Exception ex)
		{
			logger.Error($"Exception of type of {ex.GetType().FullName}");

			throw;
		}
	}

	public void UserBearerToken(string token)
	{
		bearerToken = token;
	}

	private Url CreateRequest(string pathSegment)
	{
		var startingBase = BaseUri.ToString();

		if (startingBase.EndsWith("/"))
		{
			startingBase = startingBase.Remove(startingBase.Length - 1);
		}

		var url = pathSegment.StartsWith("/") ? $"{startingBase}{pathSegment}" : $"{startingBase}/{pathSegment}";

		var requestUri = new Uri(url);

		var queryString = requestUri.Query;

		var queries = HttpUtility.ParseQueryString(queryString);

		var callingUrl = requestUri.RemoveQuery();

		if (queries.AllKeys.Length is not 0)
		{
			foreach (var key in queries.AllKeys)
			{
				var queryValue = queries[key];

				if (queryValue.Contains(","))
				{
					var values = queryValue.Split(',');

					foreach (var value in values)
					{
						callingUrl.QueryParams.Add(key, value);
					}
				}
				else
				{
					callingUrl.SetQueryParam(key, queryValue);
				}
			}
		}

		logger.Debug($"Create Request for <{callingUrl}>");

		return callingUrl;
	}
}
