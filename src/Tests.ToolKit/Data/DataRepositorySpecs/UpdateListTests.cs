using FakeItEasy;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class UpdateListTests : DataRepositoryTests
{
	private readonly EasyCapture<ExpressionFilterDefinition<TestingDataObject>> updateFilterCapture;

	public UpdateListTests()
	{
		updateFilterCapture = new EasyCapture<ExpressionFilterDefinition<TestingDataObject>>();
		var updateCapture = new EasyCapture<TestingDataObject>();

		A.CallTo(() => collection.ReplaceOneAsync(updateFilterCapture, updateCapture, A<ReplaceOptions>._, default))
		.Returns(new ReplaceOneResult.Acknowledged(1, 1, default));
	}

	[Fact]
	public async Task CallReplaceOneToUpdate()
	{
		await repository.Update(itemList);

		foreach (var currentItem in itemList)
		{
			A.CallTo(() => collection.ReplaceOneAsync(A<ExpressionFilterDefinition<TestingDataObject>>._, currentItem, A<ReplaceOptions>._, default))
			.MustHaveHappened();
		}

		var filter = updateFilterCapture.Values.FirstOrDefault()!.Expression.Compile();

		foreach (var currentItem in itemList)
		{
			filter(currentItem)
				.Should()
				.BeTrue();
		}
	}

	[Fact]
	public void ReturnUpdatedItem()
	{
		repository.Update(itemList)
				.Should()
				.Be(itemList);
	}
}