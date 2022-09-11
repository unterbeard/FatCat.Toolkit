using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class GetAllByFilterTests : DataRepositoryTests
{
	private readonly EasyCapture<ExpressionFilterDefinition<TestingDataObject>> expressionCapture;
	private readonly List<TestingDataObject> filterItems;
	private readonly int filterNumber;

	public GetAllByFilterTests()
	{
		filterNumber = Faker.RandomInt();

		filterItems = Faker.Create<List<TestingDataObject>>(3);

		foreach (var filterItem in filterItems) filterItem.Number = filterNumber;

		expressionCapture = new EasyCapture<ExpressionFilterDefinition<TestingDataObject>>();

		A.CallTo(() => collection.FindAsync<TestingDataObject>(expressionCapture, default, default))
		.Returns(new TestingAsyncCursor<TestingDataObject>(filterItems));
	}

	[Fact]
	public async Task CallFindAsyncWithFilter()
	{
		await repository.GetAllByFilter(i => i.Number == filterNumber);

		A.CallTo(() => collection.FindAsync<TestingDataObject>(A<ExpressionFilterDefinition<TestingDataObject>>._, default, default))
		.MustHaveHappened();

		expressionCapture.Value
						.Should()
						.NotBeNull();

		var filter = expressionCapture.Value.Expression.Compile();

		foreach (var currentItem in itemList)
		{
			filter(currentItem)
				.Should()
				.BeFalse();
		}

		foreach (var currentFilterItem in filterItems)
		{
			filter(currentFilterItem)
				.Should()
				.BeTrue();
		}
	}

	[Fact]
	public void ReturnFilteredList()
	{
		repository.GetAllByFilter(i => i.Number == filterNumber)
				.Should()
				.BeEquivalentTo(filterItems);
	}
}