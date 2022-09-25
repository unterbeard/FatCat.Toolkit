using FatCat.Fakes;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class SetDatabaseNameTests : LiteDbRepositoryTests
{
	private readonly string databasePath;

	public SetDatabaseNameTests()
	{
		repository.Collection = null;

		databasePath = Faker.RandomString();
	}

	[Fact]
	public void RetainTheDatabaseName()
	{
		repository.SetDatabasePath(databasePath);

		repository.DatabasePath
				.Should()
				.Be(databasePath);
	}
}