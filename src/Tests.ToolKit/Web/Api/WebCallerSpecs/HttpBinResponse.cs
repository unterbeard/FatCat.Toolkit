using FatCat.Toolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class HttpBinResponse : EqualObject
{
	public string BearerToken
	{
		get => Headers.TryGetValue("Authorization", out var value) ? value : null;
	}

	public string ContentType
	{
		get => Headers.TryGetValue("Content-Type", out var value) ? value : null;
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
