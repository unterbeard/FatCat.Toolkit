using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class GetFirstTests : DataRepositoryTests
{
	private readonly EasyCapture<ExpressionFilterDefinition<TestingMongoObject>> expressionCapture;
	private readonly TestingMongoObject firstItem;

	public GetFirstTests()
	{
		firstItem = Faker.Create<TestingMongoObject>();

		expressionCapture = new EasyCapture<ExpressionFilterDefinition<TestingMongoObject>>();

		A.CallTo(() => collection.FindAsync<TestingMongoObject>(expressionCapture!, default, default))
		.Returns(new TestingAsyncCursor<TestingMongoObject>(new List<TestingMongoObject> { firstItem }));
	}

	[Fact]
	public async Task FindAllItemsOnCollection()
	{
		await repository.GetFirst();

		A.CallTo(() => collection.FindAsync<TestingMongoObject>(A<ExpressionFilterDefinition<TestingMongoObject>>._!, default, default))
		.MustHaveHappened();

		var filter = expressionCapture.Value.Expression.Compile();

		filter(Faker.Create<TestingMongoObject>())
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