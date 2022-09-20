using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class DataRepositoryCtorTests : DataRepositoryTests
{
	[Fact]
	public void GetCollection()
	{
		A.CallTo(() => mongoDataConnection.GetCollection<TestingMongoObject>())
		.MustHaveHappened();
	}

	[Fact]
	public void GetDatabaseName()
	{
		A.CallTo(() => mongoNames.GetDatabaseName<TestingMongoObject>())
		.MustHaveHappened();
	}

	[Fact]
	public void SetCollectionOnRepository()
	{
		repository.Collection
				.Should()
				.Be(collection);
	}

	[Fact]
	public void SetDatabaseNameOnRepository()
	{
		repository.DatabaseName
				.Should()
				.Be(databaseName);
	}
}