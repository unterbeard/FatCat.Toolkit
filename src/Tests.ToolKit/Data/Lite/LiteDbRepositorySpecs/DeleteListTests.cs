using FakeItEasy;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using LiteDB;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class DeleteListTests : ConnectionHandlingLiteDbRepositoryTests<List<LiteDbTestObject>>
{
	private readonly EasyCapture<BsonValue> deleteCapture;

	public DeleteListTests()
	{
		deleteCapture = new EasyCapture<BsonValue>();

		A.CallTo(() => collection.Delete(deleteCapture)).Returns(true);
	}

	[Fact]
	public async Task DeleteEachItemFromList()
	{
		await RunTest();

		A.CallTo(() => collection.Delete(A<BsonValue>._)).MustHaveHappened(testItemList.Count, Times.Exactly);

		deleteCapture.Values.Count.Should().Be(testItemList.Count);

		foreach (var item in testItemList)
		{
			deleteCapture.Values.Select(i => i.AsInt32).Should().Contain(item.Id);
		}
	}

	[Fact]
	public async Task ReturnListOfItemsDeleted()
	{
		var resultList = await RunTest();

		resultList.Should().BeEquivalentTo(testItemList);
	}

	protected override async Task<List<LiteDbTestObject>> RunTest() => await repository.Delete(testItemList);
}
