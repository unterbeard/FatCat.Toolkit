using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Data;
using MongoDB.Driver;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public abstract class DataRepositoryTests
{
	protected readonly TestingDataObject item;
	protected readonly List<TestingDataObject> itemList;
	protected readonly DataRepository<TestingDataObject> repository;
	protected IMongoCollection<TestingDataObject> collection = null!;
	protected string databaseName = null!;
	protected IDataConnection dataConnection = null!;
	protected IDataNames dataNames = null!;

	protected DataRepositoryTests()
	{
		SetUpDataConnection();
		SetUpDataNames();

		item = Faker.Create<TestingDataObject>();
		itemList = Faker.Create<List<TestingDataObject>>(4);

		repository = new DataRepository<TestingDataObject>(dataConnection,
															dataNames);
	}

	private void SetUpDataConnection()
	{
		dataConnection = A.Fake<IDataConnection>();

		collection = A.Fake<IMongoCollection<TestingDataObject>>();

		A.CallTo(() => dataConnection.GetCollection<TestingDataObject>())
		.Returns(collection);
	}

	private void SetUpDataNames()
	{
		dataNames = A.Fake<IDataNames>();

		databaseName = Faker.RandomString();

		A.CallTo(() => dataNames.GetDatabaseName<TestingDataObject>())
		.Returns(databaseName);
	}
}