using FakeItEasy;
using FatCat.Toolkit.Data.Lite;
using LiteDB;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public abstract class LiteDbRepositoryTests
{
	protected readonly LiteDbRepository<LiteDbTestObject> repository;
	protected ILiteCollection<LiteDbTestObject> collection;
	protected ILiteDbConnection<LiteDbTestObject> liteDbConnection;

	protected LiteDbRepositoryTests()
	{
		SetUpLiteDbConnection();

		repository = new LiteDbRepository<LiteDbTestObject>(liteDbConnection);
	}

	private void SetUpLiteDbConnection()
	{
		liteDbConnection = A.Fake<ILiteDbConnection<LiteDbTestObject>>();

		collection = A.Fake<ILiteCollection<LiteDbTestObject>>();

		A.CallTo(() => liteDbConnection.Connect(A<string>._))
		.Returns(collection);
	}
}