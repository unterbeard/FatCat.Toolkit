using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Data.Lite;
using LiteDB;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public abstract class LiteDbRepositoryTests
{
	protected readonly LiteDbRepository<LiteDbTestObject> repository;
	protected readonly LiteDbTestObject testItem;
	protected readonly List<LiteDbTestObject> testItemList;
	protected ILiteCollection<LiteDbTestObject> collection;
	protected ILiteDbConnection<LiteDbTestObject> liteDbConnection;

	protected LiteDbRepositoryTests()
	{
		SetUpLiteDbConnection();

		repository = new LiteDbRepository<LiteDbTestObject>(liteDbConnection) { Collection = collection };

		testItem = Faker.Create<LiteDbTestObject>(afterCreate: i => i.Id = default);

		testItemList = Faker.Create<List<LiteDbTestObject>>();

		foreach (var item in testItemList) item.Id = default;
	}

	private void SetUpLiteDbConnection()
	{
		liteDbConnection = A.Fake<ILiteDbConnection<LiteDbTestObject>>();

		collection = A.Fake<ILiteCollection<LiteDbTestObject>>();

		A.CallTo(() => liteDbConnection.Connect(A<string>._))
		.Returns(collection);
	}
}