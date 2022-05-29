using System.Net;
using FatCat.Toolkit.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FatCat.Toolkit.Web;

public class WebResult : IActionResult
{
	public static WebResult BadRequest(ModelStateDictionary modelState) => new(HttpStatusCode.BadRequest, modelState);

	public static WebResult BadRequest(string fieldName, string messageId)
	{
		var modelState = new ModelStateDictionary();

		if (messageId.IsNotNullOrEmpty()) modelState.AddModelError(fieldName, messageId);

		return BadRequest(modelState);
	}

	public static WebResult BadRequest(string messageId) => BadRequest("General", messageId);

	public static WebResult NotAcceptable(string? content = null) => new(HttpStatusCode.NotAcceptable, content);

	public static WebResult NotFound(ModelStateDictionary modelState) => new(HttpStatusCode.NotFound, modelState);

	public static WebResult NotFound(string fieldName, string messageId)
	{
		var modelState = new ModelStateDictionary();

		modelState.AddModelError(fieldName, messageId);

		return NotFound(modelState);
	}

	public static WebResult NotFound(string? content = null) => new(HttpStatusCode.NotFound, content);

	public static WebResult NotImplemented(string? content = null) => new(HttpStatusCode.NotImplemented, content);

	public static WebResult Ok(string? content = null) => new(content.IsNullOrEmpty() ? HttpStatusCode.NoContent : HttpStatusCode.OK, content);

	public static WebResult Ok(EqualObject? returnObject) => new(returnObject == null ? HttpStatusCode.NoContent : HttpStatusCode.OK, returnObject!);

	public static WebResult Ok(IEnumerable<EqualObject> returnList) => new(returnList);

	public static WebResult Ok(List<EqualObject> returnList) => new(returnList);

	public static WebResult PreconditionFailed(string? content = null) => new(HttpStatusCode.PreconditionFailed, content);

	public static WebResult Timeout() => new(HttpStatusCode.RequestTimeout);

	private readonly HttpContent? httpContent;

	private string? content;

	public string? Content
	{
		get
		{
			content ??= httpContent?.ReadAsStringAsync().Result;

			return content;
		}
		set => content = value;
	}

	public string ContentType { get; set; } = "application/json; charset=UTF-8";

	public bool IsSuccessful => (int)StatusCode >= 200 && (int)StatusCode <= 299;

	public bool IsUnsuccessful => !IsSuccessful;

	public HttpStatusCode StatusCode { get; set; }

	public WebResult(HttpResponseMessage response)
	{
		StatusCode = response.StatusCode;

		httpContent = response.Content;
	}

	public WebResult(SimpleResponse response) : this(response.HttpStatusCode, response.Text) { }

	public WebResult(EqualObject? resultObject) : this(resultObject == null ? HttpStatusCode.NoContent : HttpStatusCode.OK, resultObject!) { }

	/// <summary>
	///  If you call this with an empty string or null for content, it will return a 204.
	/// </summary>
	public WebResult(string? content = null) : this(content.IsNullOrEmpty() ? HttpStatusCode.NoContent : HttpStatusCode.OK, content) { }

	public WebResult(HttpStatusCode statusCode, EqualObject resultObject) : this(statusCode, Json.Serialize(resultObject)) { }

	public WebResult(HttpStatusCode statusCode, IEnumerable<EqualObject> returnList) : this(statusCode, Json.Serialize(returnList)) { }

	public WebResult(IEnumerable<EqualObject> returnList) : this(HttpStatusCode.OK, Json.Serialize(returnList)) { }

	public WebResult(HttpStatusCode statusCode, ModelStateDictionary modelState) : this(statusCode, Json.Serialize(new SerializableError(modelState))) { }

	public WebResult(HttpStatusCode statusCode, string? content)
	{
		Content = content;
		StatusCode = statusCode;
	}

	public WebResult(HttpStatusCode statusCode) => StatusCode = statusCode == HttpStatusCode.OK ? HttpStatusCode.NoContent : statusCode;

	public WebResult() { }

	public Task<WebResult> AsTask() => Task.FromResult(this);

	public async Task ExecuteResultAsync(ActionContext context)
	{
		var result = new ContentResult
					{
						Content = Content,
						StatusCode = (int?)StatusCode,
						ContentType = ContentType
					};

		await result.ExecuteResultAsync(context);
	}

	public override string ToString() => $"StatusCode := <{StatusCode}> | {Content}";
}