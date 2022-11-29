using FatCat.Toolkit.Testing;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetAllByFilterTests : FilterLiteDbRepositoryTests<List<LiteDbTestObject>>
{
	protected override List<LiteDbTestObject> ItemsToReturn => testItemList;

	[Fact]
	public async Task GetAllByFilter()
	{
		await RunTest();

		VerifyFilterCallOnCollectionMade();
	}

	[Fact]
	public void ReturnListFromCollection()
	{
		RunTest()
			.Should()
			.BeEquivalentTo(testItemList);
	}

	protected override Task<List<LiteDbTestObject>> RunTest() => repository.GetAllByFilter(i => i.SomeNumber == numberToFind);
}