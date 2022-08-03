using FakeItEasy;
using FatCat.Toolkit.Testing;
using MongoDB.Driver;
using Tests.Fog.Common.Data;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class GetAllTests : DataRepositoryTests
{
	public GetAllTests()
	{
		A.CallTo(() => collection.FindAsync<TestingDataObject>(A<ExpressionFilterDefinition<TestingDataObject>>._, default, default))
		.Returns(new TestingAsyncCursor<TestingDataObject>(itemList));
	}

	[Fact]
	public async Task FindFromCollection()
	{
		await repository.GetAll();

		A.CallTo(() => collection.FindAsync<TestingDataObject>(A<ExpressionFilterDefinition<TestingDataObject>>._, default, default))
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