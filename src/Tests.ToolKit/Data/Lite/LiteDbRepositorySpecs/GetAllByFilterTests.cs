using FakeItEasy;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetAllByFilterTests : FilterLiteDbRepositoryTests<List<LiteDbTestObject>>
{
	protected override List<LiteDbTestObject> ItemsToReturn => testItemList;

	[Fact]
	public async Task GetAllByFilter()
	{
		await RunTest();

		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._))
		.MustHaveHappened();

		var expression = filterCapture.Value.Compile();

		var filterItem = new LiteDbTestObject { SomeNumber = numberToFind };

		expression(filterItem)
			.Should()
			.BeTrue();

		expression(new LiteDbTestObject { SomeNumber = numberToFind - 1 })
			.Should()
			.BeFalse();
	}

	[Fact]
	public void ReturnListFromCollection()
	{
		RunTest()
			.Should()
			.BeEquivalentTo(testItemList);
	}

	protected override Task<List<LiteDbTestObject>> RunTest() => repository.GetAllByFilter(i => i.SomeNumber == numberToFind);
}