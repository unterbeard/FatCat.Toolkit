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
	protected ILiteDbConnection connection;
	protected readonly string databasePath;

	protected LiteDbRepositoryTests()
	{
		databasePath = Faker.RandomString();

		SetUpLiteDbConnection();

		repository = new LiteDbRepository<LiteDbTestObject>(connection) { DatabasePath = databasePath };

		testItem = Faker.Create<LiteDbTestObject>(afterCreate: i => i.Id = default);

		testItemList = Faker.Create<List<LiteDbTestObject>>();

		foreach (var item in testItemList) item.Id = default;
	}

	private void SetUpLiteDbConnection()
	{
		connection = A.Fake<ILiteDbConnection>();

		collection = A.Fake<ILiteCollection<LiteDbTestObject>>();

		A.CallTo(() => connection.GetCollection<LiteDbTestObject>())
		.Returns(collection);
	}
}