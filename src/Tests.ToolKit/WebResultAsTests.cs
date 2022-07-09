using FatCat.Fakes;
using FatCat.Toolkit;
using FatCat.Toolkit.Web;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit;

public class WebResultAsTests
{
	[Fact]
	public void CanConvertList()
	{
		var itemList = Faker.Create<List<TestItem>>();

		var result = WebResult.Ok(itemList);

		result.To<List<TestItem>>()
			.Should()
			.BeEquivalentTo(itemList);
	}

	[Fact]
	public void CanConvertSingleObject()
	{
		var testItem = Faker.Create<TestItem>();

		var result = WebResult.Ok(testItem);

		result.To<TestItem>()
			.Should()
			.BeEquivalentTo(testItem);
	}

	private class TestItem : EqualObject
	{
		public DateTime DateTime { get; set; }

		public string? FirstName { get; set; }

		public int Number { get; set; }
	}
}