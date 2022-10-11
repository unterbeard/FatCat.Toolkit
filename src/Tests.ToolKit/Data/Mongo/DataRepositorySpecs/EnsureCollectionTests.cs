using FatCat.Toolkit.Data.Mongo;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Mongo.DataRepositorySpecs;

public abstract class EnsureCollectionTests : DataRepositoryTests
{
	[Fact]
	public void EnsureCollection()
	{
		repository.Collection = null;

		var exceptionAction = () => TestMethod().Wait();

		exceptionAction
			.Should()
			.Throw<ConnectionToMongoIsRequired>();
	}

	protected abstract Task TestMethod();
}