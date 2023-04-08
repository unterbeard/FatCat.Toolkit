using FatCat.Toolkit;
using FatCat.Toolkit.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SampleDocker;

public class GetSampleDataEndpoint : Endpoint
{
	private readonly IConfiguration configuration;

	public GetSampleDataEndpoint(IConfiguration configuration) => this.configuration = configuration;

	[HttpGet("api/SampleData")]
	public WebResult GetSampleData()
	{
		var response = new SampleResponse
						{
							ConfigValue = configuration["PlayingSetting"],
							SomeMetaData = "Some Meta Data"
						};

		return Ok(response);
	}
}

public class SampleResponse : EqualObject
{
	public string ConfigValue { get; set; }

	public string SomeMetaData { get; set; }
}