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

		#pragma warning disable xUnit1031
		var exceptionAction = () => TestMethod().Wait();
		#pragma warning restore xUnit1031

		exceptionAction.Should().Throw<ConnectionToMongoIsRequired>();
	}

	protected abstract Task TestMethod();
}