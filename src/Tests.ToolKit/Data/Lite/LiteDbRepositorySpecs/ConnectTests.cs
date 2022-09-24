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
	public void ConnectToTheDatabase()
	{
		repository.Connect(databasePath);

		A.CallTo(() => connection.Connect(databasePath))
		.MustHaveHappened();
	}

	[Fact]
	public void GetTheCollection()
	{
		repository.Connect(databasePath);

		A.CallTo(() => connection.GetCollection<LiteDbTestObject>())
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