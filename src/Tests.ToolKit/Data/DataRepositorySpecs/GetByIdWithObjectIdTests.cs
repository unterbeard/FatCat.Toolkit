using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class GetByIdWithObjectIdTests : DataRepositoryTests
{
	private readonly EasyCapture<ExpressionFilterDefinition<TestingDataObject>> expressionCapture;
	private readonly TestingDataObject filterItem;
	private readonly ObjectId id;

	public GetByIdWithObjectIdTests()
	{
		id = ObjectId.GenerateNewId();

		filterItem = Faker.Create<TestingDataObject>(afterCreate: i => i.Id = id);

		expressionCapture = new EasyCapture<ExpressionFilterDefinition<TestingDataObject>>();

		A.CallTo(() => collection.FindAsync<TestingDataObject>(expressionCapture!, default, default))
		.Returns(new TestingAsyncCursor<TestingDataObject>(new List<TestingDataObject> { filterItem }));
	}

	[Fact]
	public async Task CallFindAsyncWithFilter()
	{
		await repository.GetById(id);

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
		repository.GetById(id)
				.Should()
				.Be(filterItem);
	}
}