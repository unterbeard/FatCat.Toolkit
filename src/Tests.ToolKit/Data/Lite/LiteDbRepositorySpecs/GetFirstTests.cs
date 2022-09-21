using FakeItEasy;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetFirstTests : RequireCollectionLiteDbRepositoryTests<LiteDbTestObject>
{
	public GetFirstTests()
	{
		A.CallTo(() => collection.FindAll())
		.Returns(testItemList);
	}

	[Fact]
	public void CallFindAllOnCollection()
	{
		repository.GetFirst();

		A.CallTo(() => collection.FindAll())
		.MustHaveHappened();
	}

	[Fact]
	public async Task IfListIsEmptyReturnNull()
	{
		A.CallTo(() => collection.FindAll())
		.Returns(new List<LiteDbTestObject>());

		var result = await repository.GetFirst();

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void ReturnFirstItemInList()
	{
		repository.GetFirst()
				.Should()
				.Be(testItemList[0]);
	}

	protected override Task<LiteDbTestObject> RunTest() => repository.GetFirst();
}