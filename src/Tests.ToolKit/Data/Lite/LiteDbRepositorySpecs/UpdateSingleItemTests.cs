using FakeItEasy;
using FatCat.Toolkit.Testing;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class UpdateSingleItemTests : ConnectionHandlingLiteDbRepositoryTests<LiteDbTestObject>
{
	[Fact]
	public async Task CallUpdateOnCollection()
	{
		await RunTest();

		A.CallTo(() => collection.Update(testItem))
		.MustHaveHappened();
	}

	[Fact]
	public void ReturnTheUpdatedItem()
	{
		RunTest()
			.Should()
			.Be(testItem);
	}

	protected override async Task<LiteDbTestObject> RunTest() => await repository.Update(testItem);
}