using FakeItEasy;
using FatCat.Toolkit.Data.Lite;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public abstract class ConnectionHandlingLiteDbRepositoryTests<T> : LiteDbRepositoryTests
{
	[Fact]
	public void ConnectToTheDatabase()
	{
		RunTest();

		A.CallTo(() => connection.Connect(databasePath))
		.MustHaveHappened();
	}

	[Fact]
	public void DisposeOfTheConnection()
	{
		RunTest();

		A.CallTo(() => connection.Dispose())
		.MustHaveHappened();
	}

	[Fact]
	public void GetCollectionFromTheConnection()
	{
		RunTest();

		A.CallTo(() => connection.GetCollection<LiteDbTestObject>())
		.MustHaveHappened();
	}

	[Fact]
	public void IfDatabaseNameIsNullThrowConnectionException()
	{
		repository.DatabasePath = null;

		#pragma warning disable xUnit1031
		var testAction = () => RunTest().Wait();
		#pragma warning restore xUnit1031

		testAction
			.Should()
			.Throw<LiteDbConnectionException>();
	}

	protected abstract Task<T> RunTest();
}