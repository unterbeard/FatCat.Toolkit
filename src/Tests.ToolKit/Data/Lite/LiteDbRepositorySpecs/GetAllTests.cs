using FakeItEasy;
using FatCat.Toolkit.Testing;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetAllTests : ConnectionHandlingLiteDbRepositoryTests<List<LiteDbTestObject>>
{
	public GetAllTests()
	{
		A.CallTo(() => collection.FindAll()).Returns(testItemList);
	}

	[Fact]
	public async Task GetAllFromCollection()
	{
		await RunTest();

		A.CallTo(() => collection.FindAll()).MustHaveHappened();
	}

	[Fact]
	public void ReturnListFromCollection()
	{
		RunTest().Should().BeEquivalentTo(testItemList);
	}

	protected override async Task<List<LiteDbTestObject>> RunTest()
	{
		return await repository.GetAll();
	}
}
