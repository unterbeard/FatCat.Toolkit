using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Data;
using FatCat.Toolkit.Data.Mongo;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace Tests.FatCat.Toolkit.Data;

public class DataConnectionTests
{
	private readonly MongoDataConnection connection;
	private string collectionName = null!;
	private string databaseName = null!;
	private IMongoNames mongoNames = null!;
	private IMongoCollection<TestingMongoObject> mongoCollection = null!;
	private IMongoConnection mongoConnection = null!;
	private IMongoDatabase mongoDatabase = null!;

	public DataConnectionTests()
	{
		SetUpDataNames();
		SetUpMongoConnection();

		connection = new MongoDataConnection(mongoNames!, mongoConnection!);
	}

	[Fact]
	public void GetCollectionFromDatabase()
	{
		connection.GetCollection<TestingMongoObject>();

		A.CallTo(() => mongoDatabase.GetCollection<TestingMongoObject>(collectionName, null))
		.MustHaveHappened();
	}

	[Fact]
	public void GetCollectionName()
	{
		connection.GetCollection<TestingMongoObject>();

		A.CallTo(() => mongoNames.GetCollectionName<TestingMongoObject>())
		.MustHaveHappened();
	}

	[Fact]
	public void GetDatabase()
	{
		connection.GetCollection<TestingMongoObject>();

		A.CallTo(() => mongoConnection.GetDatabase(databaseName))
		.MustHaveHappened();
	}

	[Fact]
	public void GetDatabaseName()
	{
		connection.GetCollection<TestingMongoObject>();

		A.CallTo(() => mongoNames.GetDatabaseName<TestingMongoObject>())
		.MustHaveHappened();
	}

	[Fact]
	public void ReturnCollectionFromMongoDatabase()
	{
		connection.GetCollection<TestingMongoObject>()
				.Should()
				.Be(mongoCollection);
	}

	private void SetUpCollectionName()
	{
		collectionName = Faker.RandomString();

		A.CallTo(() => mongoNames.GetCollectionName<TestingMongoObject>())
		.Returns(collectionName);
	}

	private void SetUpDatabaseName()
	{
		databaseName = Faker.RandomString();

		A.CallTo(() => mongoNames.GetDatabaseName<TestingMongoObject>())
		.Returns(databaseName);
	}

	private void SetUpDataNames()
	{
		mongoNames = A.Fake<IMongoNames>();

		SetUpDatabaseName();
		SetUpCollectionName();
	}

	private void SetUpMongoCollection()
	{
		mongoCollection = A.Fake<IMongoCollection<TestingMongoObject>>();

		A.CallTo(() => mongoDatabase.GetCollection<TestingMongoObject>(A<string>._, A<MongoCollectionSettings>._))
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