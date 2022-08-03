using FatCat.Toolkit.Data;
using FatCat.Toolkit.Web;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonConverter = Newtonsoft.Json.JsonConverter;

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
	static WebCaller()
	{
		FlurlHttp.Configure(settings =>
							{
								var jsonSettings = new JsonSerializerSettings()
													{
														NullValueHandling = NullValueHandling.Ignore,
														Converters = new List<JsonConverter>()
																	{
																		new StringEnumConverter(),
																		new ObjectIdConverter()
																	}
													};
							});
	}
	
	public Uri BaseUri { get; }

	public Task<WebResult> Delete(string url) => throw new NotImplementedException();

	public Task<WebResult> Delete(string url, TimeSpan timeout) => throw new NotImplementedException();

	public Task<WebResult> Get(string url) => throw new NotImplementedException();

	public Task<WebResult> Get(string url, TimeSpan timeout) => throw new NotImplementedException();

	public Task<WebResult> Post<T>(string url, T data) where T : EqualObject => throw new NotImplementedException();

	public Task<WebResult> Post<T>(string url, List<T> data) where T : EqualObject => throw new NotImplementedException();

	public Task<WebResult> Post(string url) => throw new NotImplementedException();

	public Task<WebResult> Post(string url, string data) => throw new NotImplementedException();

	public Task<WebResult> Post<T>(string url, T data, TimeSpan timeout) where T : EqualObject => throw new NotImplementedException();

	public Task<WebResult> Post<T>(string url, List<T> data, TimeSpan timeout) where T : EqualObject => throw new NotImplementedException();

	public Task<WebResult> Post(string url, TimeSpan timeout) => throw new NotImplementedException();

	public Task<WebResult> Post(string url, string data, TimeSpan timeout) => throw new NotImplementedException();
}