using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Data;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Tests.FatCat.Toolkit.Data;

public class DataConnectionTests
{
	private readonly DataConnection connection;
	private string collectionName = null!;
	private string databaseName = null!;
	private IDataNames dataNames = null!;
	private IMongoCollection<TestingDataObject> mongoCollection = null!;
	private IMongoConnection mongoConnection = null!;
	private IMongoDatabase mongoDatabase = null!;

	public DataConnectionTests()
	{
		SetUpDataNames();
		SetUpMongoConnection();

		connection = new DataConnection(dataNames!, mongoConnection!);
	}

	[Fact]
	public void GetCollectionFromDatabase()
	{
		connection.GetCollection<TestingDataObject>();

		A.CallTo(() => mongoDatabase.GetCollection<TestingDataObject>(collectionName, null))
		.MustHaveHappened();
	}

	[Fact]
	public void GetCollectionName()
	{
		connection.GetCollection<TestingDataObject>();

		A.CallTo(() => dataNames.GetCollectionName<TestingDataObject>())
		.MustHaveHappened();
	}

	[Fact]
	public void GetDatabase()
	{
		connection.GetCollection<TestingDataObject>();

		A.CallTo(() => mongoConnection.GetDatabase(databaseName))
		.MustHaveHappened();
	}

	[Fact]
	public void GetDatabaseName()
	{
		connection.GetCollection<TestingDataObject>();

		A.CallTo(() => dataNames.GetDatabaseName<TestingDataObject>())
		.MustHaveHappened();
	}

	[Fact]
	public void ReturnCollectionFromMongoDatabase()
	{
		connection.GetCollection<TestingDataObject>()
				.Should()
				.Be(mongoCollection);
	}

	private void SetUpCollectionName()
	{
		collectionName = Faker.RandomString();

		A.CallTo(() => dataNames.GetCollectionName<TestingDataObject>())
		.Returns(collectionName);
	}

	private void SetUpDatabaseName()
	{
		databaseName = Faker.RandomString();

		A.CallTo(() => dataNames.GetDatabaseName<TestingDataObject>())
		.Returns(databaseName);
	}

	private void SetUpDataNames()
	{
		dataNames = A.Fake<IDataNames>();

		SetUpDatabaseName();
		SetUpCollectionName();
	}

	private void SetUpMongoCollection()
	{
		mongoCollection = A.Fake<IMongoCollection<TestingDataObject>>();

		A.CallTo(() => mongoDatabase.GetCollection<TestingDataObject>(A<string>._, A<MongoCollectionSettings>._))
		.Returns(mongoCollection);
	}

	private void SetUpMongoConnection()
	{
		mongoConnection = A.Fake<IMongoConnection>();

		SetUpMongoDatabase();
		SetUpMongoCollection();
	}

	private void SetUpMongoDatabase()
	{
		mongoDatabase = A.Fake<IMongoDatabase>();

		A.CallTo(() => mongoConnection.GetDatabase(A<string>._))
		.Returns(mongoDatabase);
	}
}