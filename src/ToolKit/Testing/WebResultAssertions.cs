using System.Net;
using FatCat.Toolkit.Web;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;

namespace FatCat.Toolkit.Testing;

public static class WebResultAssertionsExtensions
{
	public static WebResultAssertions Should(this Task<WebResult> task)
	{
		var result = task.Result;

		return new WebResultAssertions(result);
	}

	public static WebResultAssertions Should(this WebResult webResult) => new(webResult);
}

public class WebResultAssertions : ReferenceTypeAssertions<WebResult, WebResultAssertions>
{
	protected override string Identifier => "Web Results assertions";

	public WebResultAssertions(WebResult result) : base(result) { }

	public WebResultAssertions Be(WebResult expectedResult)
	{
		new ObjectAssertions(Subject).BeEquivalentTo(expectedResult);

		return this;
	}

	public WebResultAssertions Be<T>(T expectedValue)
	{
		Subject
			.Should()
			.NotBeNull();

		Subject
			.To<T>()
			.Should()
			.BeEquivalentTo(expectedValue);

		return this;
	}

	public WebResultAssertions BeBadRequest() => HaveStatusCode(HttpStatusCode.BadRequest);

	public WebResultAssertions BeBadRequest(string fieldName, string messageId)
	{
		var expectedResult = WebResult.BadRequest(fieldName, messageId);

		return HaveStatusCode(HttpStatusCode.BadRequest)
			.Be(expectedResult);
	}

	public WebResultAssertions BeBadRequest(string messageId)
	{
		var expectedResult = WebResult.BadRequest(messageId);

		return HaveStatusCode(HttpStatusCode.BadRequest)
			.Be(expectedResult);
	}

	public WebResultAssertions BeConflict() => HaveStatusCode(HttpStatusCode.Conflict);

	public WebResultAssertions BeEmptyListOf<T>()
	{
		Subject
			.Should()
			.NotBeNull();

		var list = Subject.To<List<T>>();

		list
			.Should()
			.BeEmpty();

		return this;
	}

	public WebResultAssertions BeEquivalentTo(WebResult expectedResult)
	{
		new ObjectAssertions(Subject).BeEquivalentTo(expectedResult);

		return this;
	}

	public WebResultAssertions BeEquivalentTo<T>(T expectedValue)
	{
		Subject
			.To<T>()
			.Should()
			.BeEquivalentTo(expectedValue);

		return this;
	}

	public WebResultAssertions BeNotAcceptable() => HaveStatusCode(HttpStatusCode.NotAcceptable);

	public WebResultAssertions BeNotFound() => HaveStatusCode(HttpStatusCode.NotFound);

	public WebResultAssertions BeOk() => HaveOneOfStatusCode(new[] { HttpStatusCode.OK, HttpStatusCode.NoContent });

	public WebResultAssertions BeSuccessful()
	{
		Subject
			.Should()
			.NotBeNull();

		Subject
			.IsSuccessful
			.Should()
			.BeTrue(Subject.Content);

		return this;
	}

	public WebResultAssertions BeUnsuccessful()
	{
		Subject
			.Should()
			.NotBeNull();

		Subject
			.IsUnsuccessful
			.Should()
			.BeTrue(Subject.Content);

		return this;
	}

	public WebResultAssertions For<T>(Action<T> action)
	{
		Subject
			.Should()
			.NotBeNull();

		action(Subject.To<T>()!);

		return this;
	}

	public WebResultAssertions ForList<T>(Action<List<T>> action)
	{
		Subject
			.Should()
			.NotBeNull();

		action(Subject.To<List<T>>()!);

		return this;
	}

	public WebResultAssertions HaveContent(string content)
	{
		Subject
			.Should()
			.NotBeNull();

		Subject
			.Content
			.Should()
			.Be(content);

		return this;
	}

	public WebResultAssertions HaveContentEquivalentTo<TContentType>(TContentType expectedContent) { return HaveContentEquivalentTo(expectedContent, config => config); }

	public WebResultAssertions HaveContentEquivalentTo<TContentType>(TContentType expectedContent, Func<EquivalencyAssertionOptions<TContentType>, EquivalencyAssertionOptions<TContentType>> config)
	{
		Subject
			.Should()
			.NotBeNull("WebResult should never be null");

		Subject
			.Should()
			.HaveStatusCode(HttpStatusCode.OK, "you cannot test for content from an unsuccessful status code: {0}", Subject.StatusCode);

		var actualContent = Subject.To<TContentType>();

		actualContent
			.Should()
			.BeEquivalentTo(expectedContent);

		return this;
	}

	public WebResultAssertions HaveContentTypeOf(string contentType)
	{
		Subject
			.Should()
			.NotBeNull();

		Subject
			.ContentType
			.Should()
			.Be(contentType);

		return this;
	}

	public WebResultAssertions HaveNoContent() => HaveStatusCode(HttpStatusCode.NoContent);

	public WebResultAssertions HaveStatusCode(HttpStatusCode statusCode, string? because = null, params object[] becauseArgs) => HaveOneOfStatusCode(new[] { statusCode }, because, becauseArgs);

	public WebResultAssertions WithMessage(string expectedMessage, string? because = null, params object[] becauseArgs)
	{
		Subject.Content.Should().MatchEquivalentOf(expectedMessage, because, becauseArgs);

		return this;
	}

	private WebResultAssertions HaveOneOfStatusCode(HttpStatusCode[] acceptableStatusCodes, string? because = null, params object[] becauseArgs)
	{
		Subject
			.Should()
			.NotBeNull();

		Subject
			.StatusCode
			.Should()
			.BeOneOf(acceptableStatusCodes, because, becauseArgs);

		return this;
	}
}