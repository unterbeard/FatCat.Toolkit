using FakeItEasy;
using FatCat.Toolkit.Testing;
using MongoDB.Driver;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class GetAllTests : DataRepositoryTests
{
	public GetAllTests()
	{
		A.CallTo(() => collection.FindAsync<TestingMongoObject>(A<ExpressionFilterDefinition<TestingMongoObject>>._, default, default))
		.Returns(new TestingAsyncCursor<TestingMongoObject>(itemList));
	}

	[Fact]
	public async Task FindFromCollection()
	{
		await repository.GetAll();

		A.CallTo(() => collection.FindAsync<TestingMongoObject>(A<ExpressionFilterDefinition<TestingMongoObject>>._, default, default))
		.MustHaveHappened();
	}

	[Fact]
	public void ReturnTheItems()
	{
		repository.GetAll()
				.Should()
				.BeEquivalentTo(itemList);
	}
}