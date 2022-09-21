using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Extensions;
using FluentAssertions;
using LiteDB;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class CreateTests : RequireCollectionLiteDbRepositoryTests<LiteDbTestObject>
{
	private readonly int createdId;

	public CreateTests()
	{
		createdId = Faker.RandomInt(2, 2500);

		A.CallTo(() => collection.Insert(A<LiteDbTestObject>._))
		.Returns(new BsonValue(createdId));
	}

	[Fact]
	public async Task InsertObjectIntoCollection()
	{
		await RunTest();

		A.CallTo(() => collection.Insert(testItem))
		.MustHaveHappened();
	}

	[Fact]
	public async Task ReturnTestObjectWithIdPopulated()
	{
		var expectedObject = testItem.DeepCopy();

		expectedObject.Id = createdId;

		var result = await RunTest();

		result.Should()
			.Be(expectedObject);
	}

	protected override async Task<LiteDbTestObject> RunTest() => await repository.Create(testItem);
}