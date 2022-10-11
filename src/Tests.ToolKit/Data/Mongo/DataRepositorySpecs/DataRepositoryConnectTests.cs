using FakeItEasy;
using FatCat.Fakes;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Mongo.DataRepositorySpecs;

public class DataRepositoryConnectTests : DataRepositoryTests
{
	private readonly string connectionString;

	public DataRepositoryConnectTests() => connectionString = Faker.RandomString();

	[Fact]
	public void GetCollection()
	{
		repository.Connect(connectionString);

		A.CallTo(() => mongoDataConnection.GetCollection<TestingMongoObject>(connectionString))
		.MustHaveHappened();
	}

	[Fact]
	public void GetDatabaseName()
	{
		repository.Connect(connectionString);

		A.CallTo(() => mongoNames.GetDatabaseName<TestingMongoObject>())
		.MustHaveHappened();
	}

	[Fact]
	public void SetCollectionOnRepository()
	{
		repository.Connect(connectionString);

		repository.Collection
				.Should()
				.Be(collection);
	}

	[Fact]
	public void SetDatabaseNameOnRepository()
	{
		repository.Connect(connectionString);

		repository.DatabaseName
				.Should()
				.Be(databaseName);
	}
}