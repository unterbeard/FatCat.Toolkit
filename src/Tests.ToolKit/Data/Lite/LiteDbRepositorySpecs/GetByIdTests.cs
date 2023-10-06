using FakeItEasy;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetByIdTests : FilterLiteDbRepositoryTests<LiteDbTestObject>
{
	protected override List<LiteDbTestObject> ItemsToReturn => new() { testItem };

	[Fact]
	public async Task GetByFilterOnCollection()
	{
		await RunTest();

		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._)).MustHaveHappened();

		var expression = filterCapture.Value.Compile();

		var filterItem = new LiteDbTestObject { Id = numberToFind };

		expression(filterItem).Should().BeTrue();

		expression(new LiteDbTestObject { Id = numberToFind - 1 }).Should().BeFalse();
	}

	[Fact]
	public async Task IfCollectionEmptyThenNullIsReturned()
	{
		SetUpFindWithEmptyCollection();

		var result = await RunTest();

		result.Should().BeNull();
	}

	[Fact]
	public void ReturnItemFromCollection()
	{
		RunTest().Should().Be(testItem);
	}

	protected override Task<LiteDbTestObject> RunTest()
	{
		return Task.FromResult(repository.GetById(numberToFind));
	}
}
