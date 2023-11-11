using FatCat.Fakes;
using FatCat.Toolkit;
using FatCat.Toolkit.Web;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class PostTests : WebCallerTests
{
	protected override string BasicPath => "/post";

	[Fact]
	public async Task PostWithAListOfData()
	{
		var dataList = Faker.Create<List<TestData>>();

		var result = await webCaller.Post(BasicPath, dataList);

		result.IsSuccessful.Should().BeTrue();

		var jsonResponse = result.To<HttpBinDataJsonResponse<List<TestData>>>();

		jsonResponse.Json.Should().BeEquivalentTo(dataList);
	}

	[Fact]
	public async Task PostWithJsonData()
	{
		var data = Faker.Create<TestData>();

		var json = JsonConvert.SerializeObject(data);

		var result = await webCaller.Post(BasicPath, json);

		result.IsSuccessful.Should().BeTrue();

		var jsonResponse = result.To<HttpBinDataJsonResponse<string>>();

		var responseData = JsonConvert.DeserializeObject<TestData>(jsonResponse.Json);

		responseData.Should().BeEquivalentTo(data);
	}

	[Fact]
	public async Task PostWithStringData()
	{
		var data = "this is just some data";

		var result = await webCaller.Post(BasicPath, data);

		result.IsSuccessful.Should().BeTrue();

		response = result.To<HttpBinResponse>();

		response.RawData.Should().Be($"\"{data}\"");
	}

	protected override Task<WebResult> MakeCallToWeb(string path) { return webCaller.Post(path); }

	public class TestData : EqualObject
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Number { get; set; }

		public string ToJson() { return JsonConvert.SerializeObject(this); }
	}
}