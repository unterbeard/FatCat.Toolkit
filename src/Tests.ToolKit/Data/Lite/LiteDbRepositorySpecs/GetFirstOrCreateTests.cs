using FakeItEasy;
using FatCat.Toolkit.Testing;
using LiteDB;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetFirstOrCreateTests : ConnectionHandlingLiteDbRepositoryTests<LiteDbTestObject>
{
	public GetFirstOrCreateTests()
	{
		A.CallTo(() => collection.FindAll()).Returns(new[] { testItem });

		A.CallTo(() => collection.Insert(A<LiteDbTestObject>._)).Returns(new BsonValue(1));
	}

	[Fact]
	public async Task GetAllFromRepository()
	{
		await RunTest();

		A.CallTo(() => collection.FindAll()).MustHaveHappened();
	}

	[Fact]
	public async Task IfItemIsFoundThenItIsNotCreated()
	{
		await RunTest();

		A.CallTo(() => collection.Insert(A<LiteDbTestObject>._)).MustNotHaveHappened();
	}

	[Fact]
	public async Task IfItemIsNotFoundThenCreateDefault()
	{
		A.CallTo(() => collection.FindAll()).Returns(new List<LiteDbTestObject>());

		await RunTest();

		A.CallTo(() => collection.Insert(A<LiteDbTestObject>._)).MustHaveHappened();
	}

	[Fact]
	public void ReturnTheItem()
	{
		RunTest().Should().Be(testItem);
	}

	protected override Task<LiteDbTestObject> RunTest()
	{
		return repository.GetFirstOrCreate();
	}
}
