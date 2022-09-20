using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Extensions;
using FluentAssertions;
using LiteDB;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class CreateTests : LiteDbRepositoryTests
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
		await repository.Create(testObject);

		A.CallTo(() => collection.Insert(testObject))
		.MustHaveHappened();
	}

	[Fact]
	public async Task ReturnTestObjectWithIdPopulated()
	{
		var expectedObject = testObject.DeepCopy();

		expectedObject.Id = createdId;

		var result = await repository.Create(testObject);

		result.Should()
			.Be(expectedObject);
	}
}