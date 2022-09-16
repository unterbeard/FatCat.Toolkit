using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class GetSingleItemByFilter : DataRepositoryTests
{
	private readonly EasyCapture<ExpressionFilterDefinition<TestingDataObject>> expressionCapture;
	private readonly TestingDataObject filterItem;
	private readonly int filterNumber;

	public GetSingleItemByFilter()
	{
		filterNumber = Faker.RandomInt();

		filterItem = Faker.Create<TestingDataObject>(afterCreate: i => i.Number = filterNumber);

		expressionCapture = new EasyCapture<ExpressionFilterDefinition<TestingDataObject>>();

		A.CallTo(() => collection.FindAsync<TestingDataObject>(expressionCapture!, default, default))
		.Returns(new TestingAsyncCursor<TestingDataObject>(new List<TestingDataObject> { filterItem }));
	}

	[Fact]
	public async Task CallFindAsyncWithFilter()
	{
		await repository.GetByFilter(i => i!.Number == filterNumber);

		A.CallTo(() => collection.FindAsync<TestingDataObject>(A<ExpressionFilterDefinition<TestingDataObject>>._!, default, default))
		.MustHaveHappened();

		expressionCapture.Value
						.Should()
						.NotBeNull();

		var filter = expressionCapture.Value.Expression.Compile();

		foreach (var currentItem in itemList)
		{
			filter(currentItem!)
				.Should()
				.BeFalse();
		}

		filter(filterItem!)
			.Should()
			.BeTrue();
	}

	[Fact]
	public void ReturnFilterItem()
	{
		repository.GetByFilter(i => i!.Number == filterNumber)
				.Should()
				.Be(filterItem);
	}
}