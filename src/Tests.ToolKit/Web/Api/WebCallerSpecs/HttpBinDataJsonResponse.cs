using FatCat.Toolkit;
using Newtonsoft.Json;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class HttpBinDataJsonResponse<T> : HttpBinResponse
{
	[JsonProperty("json")]
	public T Json { get; set; }
}
