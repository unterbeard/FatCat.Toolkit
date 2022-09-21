using FakeItEasy;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Mongo.DataRepositorySpecs;

public class DeleteItemTests : DataRepositoryTests
{
	[Fact]
	public async Task CallDeleteOneOnCollection()
	{
		var filterCapture = new EasyCapture<ExpressionFilterDefinition<TestingMongoObject>>();

		A.CallTo(() => collection.DeleteOneAsync(filterCapture, default))
		.Returns(new DeleteResult.Acknowledged(1));

		await repository.Delete(item);

		A.CallTo(() => collection.DeleteOneAsync(A<ExpressionFilterDefinition<TestingMongoObject>>._, default))
		.MustHaveHappened();

		filterCapture.Value.Should().NotBeNull();

		var filter = filterCapture.Value.Expression.Compile();

		filter(item)
			.Should()
			.BeTrue();
	}

	[Fact]
	public void ReturnDeletedItem()
	{
		repository.Delete(item)
				.Should()
				.BeEquivalentTo(item);
	}
}