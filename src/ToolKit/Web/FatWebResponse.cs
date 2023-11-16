using System.Net;
using Newtonsoft.Json;

namespace FatCat.Toolkit.Web;

public class FatWebResponse<T> : EqualObject
	where T : class
{
	public static FatWebResponse<T> Ok(T data) => new(HttpStatusCode.OK, data);

	public FatWebResponse BaseResponse { get; }

	public string Content
	{
		get => BaseResponse.Content;
	}

	public string ContentType
	{
		get => BaseResponse.ContentType;
	}

	public T Data
	{
		get => BaseResponse.IsSuccessful ? BaseResponse.To<T>() : null;
	}

	public bool IsSuccessful
	{
		get => BaseResponse.IsSuccessful;
	}

	public bool IsUnsuccessful
	{
		get => !IsSuccessful;
	}

	public HttpStatusCode StatusCode
	{
		get => BaseResponse.StatusCode;
	}

	public FatWebResponse(HttpStatusCode statusCode) =>
		BaseResponse = new FatWebResponse { StatusCode = statusCode };

	public FatWebResponse(FatWebResponse webResponse) => BaseResponse = webResponse;

	public FatWebResponse(HttpStatusCode statusCode, T data) =>
		BaseResponse = new FatWebResponse { StatusCode = statusCode, Content = JsonConvert.SerializeObject(data) };

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

	public bool IsSuccessful
	{
		get => (int)StatusCode >= 200 && (int)StatusCode <= 299;
	}

	public bool IsUnsuccessful
	{
		get => !IsSuccessful;
	}

	public HttpStatusCode StatusCode { get; set; }

	public FatWebResponse() { }

	public FatWebResponse(HttpResponseMessage response)
	{
		StatusCode = response.StatusCode;

		httpContent = response.Content;
	}

	public T To<T>() => JsonContentConverter.ConvertTo<T>(Content);
}
