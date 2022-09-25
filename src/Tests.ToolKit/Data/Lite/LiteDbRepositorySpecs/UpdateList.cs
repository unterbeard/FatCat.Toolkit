using FakeItEasy;
using FatCat.Toolkit.Testing;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class UpdateList : ConnectionHandlingLiteDbRepositoryTests<List<LiteDbTestObject>>
{
	[Fact]
	public async Task CallUpdateOnEachItem()
	{
		await RunTest();

		foreach (var item in testItemList)
		{
			A.CallTo(() => collection.Update(item))
			.MustHaveHappened();
		}
	}

	[Fact]
	public void ReturnItemUpdateList()
	{
		RunTest()
			.Should()
			.BeEquivalentTo(testItemList);
	}

	protected override async Task<List<LiteDbTestObject>> RunTest() => await repository.Update(testItemList);
}