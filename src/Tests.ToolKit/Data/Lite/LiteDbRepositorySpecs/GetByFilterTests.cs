using FakeItEasy;
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

		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._))
		.MustHaveHappened();

		var expression = filterCapture.Value.Compile();

		expression(new LiteDbTestObject { SomeNumber = numberToFind })
			.Should()
			.BeTrue();

		expression(new LiteDbTestObject { SomeNumber = numberToFind - 1 })
			.Should()
			.BeFalse();
	}

	[Fact]
	public async Task IfNothingFoundReturnNull()
	{
		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._))
		.Returns(new List<LiteDbTestObject>());

		var result = await RunTest();

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void ReturnFirstItemFromList()
	{
		RunTest()
			.Should()
			.Be(testItem);
	}

	protected override Task<LiteDbTestObject> RunTest() => repository.GetByFilter(i => i.SomeNumber == numberToFind);
}