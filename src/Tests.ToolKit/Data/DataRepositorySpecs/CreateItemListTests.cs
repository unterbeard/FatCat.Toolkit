using FakeItEasy;
using FatCat.Toolkit.Testing;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class CreateItemListTests : DataRepositoryTests
{
	[Fact]
	public async Task CallInsertOneForEachItemInList()
	{
		await repository.Create(itemList);

		foreach (var currentItem in itemList)
		{
			A.CallTo(() => collection.InsertOneAsync(currentItem, default, default))
			.MustHaveHappened();
		}
	}

	[Fact]
	public void ReturnListOfCreatedItems()
	{
		repository.Create(itemList)
				.Should()
				.BeEquivalentTo(itemList);
	}
}