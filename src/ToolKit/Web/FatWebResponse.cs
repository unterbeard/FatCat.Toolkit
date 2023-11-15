using System.Net;

namespace FatCat.Toolkit.Web;

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