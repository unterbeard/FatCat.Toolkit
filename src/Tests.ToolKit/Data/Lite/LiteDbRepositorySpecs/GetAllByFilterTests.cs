using System.Linq.Expressions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetAllByFilterTests : RequireCollectionLiteDbRepositoryTests<List<LiteDbTestObject>>
{
	private readonly EasyCapture<Expression<Func<LiteDbTestObject, bool>>> filterCapture;
	private readonly int numberToFind;

	public GetAllByFilterTests()
	{
		numberToFind = Faker.RandomInt();

		filterCapture = new EasyCapture<Expression<Func<LiteDbTestObject, bool>>>();

		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._))
		.Returns(testItemList);
	}

	[Fact]
	public async Task GetAllByFilter()
	{
		await RunTest();

		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._))
		.MustHaveHappened();

		var expression = filterCapture.Value.Compile();

		var filterItem = new LiteDbTestObject { SomeNumber = numberToFind };

		expression(filterItem)
			.Should()
			.BeTrue();

		expression(new LiteDbTestObject { SomeNumber = numberToFind - 1 })
			.Should()
			.BeFalse();
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