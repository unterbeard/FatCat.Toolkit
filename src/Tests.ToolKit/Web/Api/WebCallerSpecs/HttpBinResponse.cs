using FatCat.Toolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class HttpBinResponse : EqualObject
{
	public string AcceptHeader
	{
		get => Headers.GetValueOrDefault("Accept");
	}

	public string AuthorizationHeader
	{
		get => Headers.GetValueOrDefault("Authorization");
	}

	public string ContentTypeHeader
	{
		get => Headers.GetValueOrDefault("Content-Type");
	}

	[JsonProperty("headers")]
	public Dictionary<string, string> Headers { get; set; }

	[JsonProperty("origin")]
	public string Origin { get; set; }

	[JsonProperty("args")]
	public Dictionary<string, object> QueryParameters { get; set; }

	[JsonProperty("data")]
	public string RawData { get; set; }

	[JsonProperty("url")]
	public string Url { get; set; }

	public List<string> GetQueryParameterAsList(string name)
	{
		var jArray = QueryParameters[name] as JArray;

		return jArray.ToObject<List<string>>();
	}
}
