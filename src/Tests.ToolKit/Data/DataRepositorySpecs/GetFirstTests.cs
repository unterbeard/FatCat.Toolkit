using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using MongoDB.Driver;
using Tests.Fog.Common.Data;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class GetFirstTests : DataRepositoryTests
{
	private readonly EasyCapture<ExpressionFilterDefinition<TestingDataObject>> expressionCapture;
	private readonly TestingDataObject firstItem;

	public GetFirstTests()
	{
		firstItem = Faker.Create<TestingDataObject>();

		expressionCapture = new EasyCapture<ExpressionFilterDefinition<TestingDataObject>>();

		A.CallTo(() => collection.FindAsync<TestingDataObject>(expressionCapture!, default, default))
		.Returns(new TestingAsyncCursor<TestingDataObject>(new List<TestingDataObject> { firstItem }));
	}

	[Fact]
	public async Task FindAllItemsOnCollection()
	{
		await repository.GetFirst();

		A.CallTo(() => collection.FindAsync<TestingDataObject>(A<ExpressionFilterDefinition<TestingDataObject>>._!, default, default))
		.MustHaveHappened();

		var filter = expressionCapture.Value.Expression.Compile();

		filter(Faker.Create<TestingDataObject>())
			.Should()
			.BeTrue();
	}

	[Fact]
	public void ReturnFirstItem()
	{
		repository.GetFirst()
				.Should()
				.Be(firstItem);
	}
}