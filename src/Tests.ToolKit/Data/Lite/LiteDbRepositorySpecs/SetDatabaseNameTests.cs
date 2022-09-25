using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class SetDatabaseNameTests : LiteDbRepositoryTests
{
	public SetDatabaseNameTests() => repository.DatabasePath = null;

	[Fact]
	public void RetainTheDatabaseName()
	{
		repository.SetDatabasePath(databasePath);

		repository.DatabasePath
				.Should()
				.Be(databasePath);
	}
}