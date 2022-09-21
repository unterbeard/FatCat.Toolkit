using System.Linq.Expressions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetByFilterTests : RequireCollectionLiteDbRepositoryTests<LiteDbTestObject>
{
	private readonly EasyCapture<Expression<Func<LiteDbTestObject, bool>>> filterCapture;
	private readonly int numberToFind;

	public GetByFilterTests()
	{
		numberToFind = Faker.RandomInt();

		filterCapture = new EasyCapture<Expression<Func<LiteDbTestObject, bool>>>();

		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._))
		.Returns(new[] { testItem });
	}

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