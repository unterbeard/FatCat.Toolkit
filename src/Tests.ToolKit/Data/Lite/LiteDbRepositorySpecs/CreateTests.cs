using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class CreateTests : LiteDbRepositoryTests
{
	[Fact]
	public async Task InsertObjectIntoCollection()
	{
		await repository.Create(testObject);

		A.CallTo(() => collection.Insert(testObject))
		.MustHaveHappened();
	}
}