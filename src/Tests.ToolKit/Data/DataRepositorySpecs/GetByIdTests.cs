using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Tests.Fog.Common.Data;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class GetByIdTests : DataRepositoryTests
{
	private readonly EasyCapture<ExpressionFilterDefinition<TestingDataObject>> expressionCapture;
	private readonly TestingDataObject filterItem;
	private readonly ObjectId id;

	public GetByIdTests()
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
		await repository.GetById(id.ToString());

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
		repository.GetById(id.ToString())
				.Should()
				.Be(filterItem);
	}
}