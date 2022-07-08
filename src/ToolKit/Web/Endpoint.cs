using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FatCat.Toolkit.Web;

public abstract class Endpoint : ControllerBase
{
	protected string? AuthToken => Request.Headers.TryGetValue("Authorization", out var values) ? values.FirstOrDefault() : string.Empty;

	protected WebResult BadRequest(string? message = null) => WebResult.BadRequest(message);

	protected WebResult BadRequest(string fieldName, string? messageId) => WebResult.BadRequest(fieldName, messageId);

	protected WebResult NotAcceptable(string message = null) => WebResult.NotAcceptable(message);

	protected WebResult NotFound(string message = null) => WebResult.NotFound(message);

	protected WebResult NotImplemented() => WebResult.NotImplemented();

	protected WebResult Ok(EqualObject model) => Ok(JsonConvert.SerializeObject(model));

	protected WebResult Ok<T>(List<T> list) where T : EqualObject => Ok(JsonConvert.SerializeObject(list));

	protected WebResult Ok<T>(IEnumerable<T> list) where T : EqualObject => Ok(JsonConvert.SerializeObject(list));

	/// <summary>
	///  If you call this with an empty string or null for content, it will return a 204.
	/// </summary>
	protected WebResult Ok(string results = null) => WebResult.Ok(results);

	protected async Task<string> ReadContent() => await Request.ReadContent();
}