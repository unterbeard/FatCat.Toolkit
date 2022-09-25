using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using LiteDB;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class CreateListTests : ConnectionHandlingLiteDbRepositoryTests<List<LiteDbTestObject>>
{
	private readonly List<int> createdIds;
	private readonly List<LiteDbTestObject> itemsToCreate;

	public CreateListTests()
	{
		itemsToCreate = Faker.Create<List<LiteDbTestObject>>();

		createdIds = new List<int>();

		for (var i = 0; i < itemsToCreate.Count; i++) createdIds.Add(Faker.RandomInt(34, 443));

		var bsonList = createdIds.Select(i => new BsonValue(i)).ToList();

		A.CallTo(() => collection.Insert(A<LiteDbTestObject>._))
		.ReturnsNextFromSequence(bsonList);
	}

	[Fact]
	public async Task CallInsertForeachItem()
	{
		await RunTest();

		foreach (var item in itemsToCreate)
		{
			A.CallTo(() => collection.Insert(item))
			.MustHaveHappened();
		}
	}

	[Fact]
	public async Task PutTheIdOnEachItem()
	{
		var expectedResultList = itemsToCreate.DeepCopy();

		for (var i = 0; i < expectedResultList.Count; i++) { expectedResultList[i].Id = createdIds[i]; }

		var resultList = await RunTest();

		resultList.Should()
				.BeEquivalentTo(expectedResultList);
	}

	protected override async Task<List<LiteDbTestObject>> RunTest() => await repository.Create(itemsToCreate);
}