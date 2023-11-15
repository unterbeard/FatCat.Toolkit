using System.Net;

namespace FatCat.Toolkit.Web;

public class FatWebResponse<T> : EqualObject
	where T : class
{
	public FatWebResponse BaseResponse { get; }

	public string Content => BaseResponse.Content;

	public string ContentType => BaseResponse.ContentType;

	public T Data => BaseResponse.IsSuccessful ? BaseResponse.To<T>() : null;

	public bool IsSuccessful => BaseResponse.IsSuccessful;

	public bool IsUnsuccessful => !IsSuccessful;

	public HttpStatusCode StatusCode => BaseResponse.StatusCode;

	public FatWebResponse(HttpStatusCode statusCode) =>
		BaseResponse = new FatWebResponse { StatusCode = statusCode };

	public FatWebResponse(FatWebResponse webResponse) => BaseResponse = webResponse;

	public override string ToString() =>
		$"FatWebResponse | StatusCode <{StatusCode}> | Type {typeof(T).FullName} | {Content}";
}

public class FatWebResponse : EqualObject
{
	public static FatWebResponse Timeout() => new() { StatusCode = HttpStatusCode.RequestTimeout };

	private readonly HttpContent httpContent;

	private string content;

	public string Content
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

	public FatWebResponse() { }

	public FatWebResponse(HttpResponseMessage response)
	{
		StatusCode = response.StatusCode;

		httpContent = response.Content;
	}

	public T To<T>() => JsonContentConverter.ConvertTo<T>(Content);
}
