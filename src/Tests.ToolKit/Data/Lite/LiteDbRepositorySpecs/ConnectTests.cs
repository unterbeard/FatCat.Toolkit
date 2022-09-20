using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Data.Lite;
using FluentAssertions;
using LiteDB;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class ConnectTests
{
	private readonly string databasePath;
	private readonly LiteDbRepository<LiteDbTestObject> repository;
	private ILiteCollection<LiteDbTestObject> collection;
	private ILiteDbConnection<LiteDbTestObject> liteDbConnection;

	public ConnectTests()
	{
		SetUpLiteDbConnection();

		repository = new LiteDbRepository<LiteDbTestObject>(liteDbConnection);

		databasePath = Faker.RandomString();
	}

	[Fact]
	public void ConnectToDbConnection()
	{
		repository.Connect(databasePath);

		A.CallTo(() => liteDbConnection.Connect(databasePath))
		.MustHaveHappened();
	}

	[Fact]
	public void SaveTheCollectionOnTheRepository()
	{
		repository.Connect(databasePath);

		repository.Collection
				.Should()
				.Be(collection);
	}

	private void SetUpLiteDbConnection()
	{
		liteDbConnection = A.Fake<ILiteDbConnection<LiteDbTestObject>>();

		collection = A.Fake<ILiteCollection<LiteDbTestObject>>();

		A.CallTo(() => liteDbConnection.Connect(A<string>._))
		.Returns(collection);
	}
}