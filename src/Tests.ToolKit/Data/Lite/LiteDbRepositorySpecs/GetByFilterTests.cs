using FatCat.Toolkit.Testing;
using FluentAssertions;
using Xunit;

// ReSharper disable VirtualMemberCallInConstructor

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetByFilterTests : FilterLiteDbRepositoryTests<LiteDbTestObject>
{
	protected override List<LiteDbTestObject> ItemsToReturn => new() { testItem };

	[Fact]
	public async Task FindOnCollectionWithFilter()
	{
		await RunTest();

		VerifyFilterCallOnCollectionMade();
	}

	[Fact]
	public async Task IfNothingFoundReturnNull()
	{
		SetUpFindWithEmptyCollection();

		var result = await RunTest();

		result.Should().BeNull();
	}

	[Fact]
	public void ReturnFirstItemFromList()
	{
		RunTest().Should().Be(testItem);
	}

	protected override Task<LiteDbTestObject> RunTest() =>
		repository.GetByFilter(i => i.SomeNumber == numberToFind);
}
