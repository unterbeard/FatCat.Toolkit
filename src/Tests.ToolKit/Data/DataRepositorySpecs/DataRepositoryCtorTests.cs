using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class DataRepositoryCtorTests : DataRepositoryTests
{
	[Fact]
	public void GetCollection()
	{
		A.CallTo(() => dataConnection.GetCollection<TestingDataObject>())
		.MustHaveHappened();
	}

	[Fact]
	public void GetDatabaseName()
	{
		A.CallTo(() => dataNames.GetDatabaseName<TestingDataObject>())
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