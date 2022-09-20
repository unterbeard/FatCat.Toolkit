using FakeItEasy;
using FatCat.Fakes;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class ConnectTests : LiteDbRepositoryTests
{
	private readonly string databasePath;

	public ConnectTests()
	{
		repository.Collection = null;
		
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
}