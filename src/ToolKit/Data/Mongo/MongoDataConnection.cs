using MongoDB.Driver;

namespace FatCat.Toolkit.Data.Mongo;

public interface IMongoDataConnection
{
	IMongoCollection<T> GetCollection<T>() where T : MongoObject;
}

public class MongoDataConnection : IMongoDataConnection
{
	private readonly IMongoNames mongoNames;
	private readonly IMongoConnection mongoConnection;

	public MongoDataConnection(IMongoNames mongoNames,
								IMongoConnection mongoConnection)
	{
		this.mongoNames = mongoNames;
		this.mongoConnection = mongoConnection;
	}

	public IMongoCollection<T> GetCollection<T>() where T : MongoObject
	{
		var database = GetDatabase<T>();

		return database.GetCollection<T>(mongoNames.GetCollectionName<T>());
	}

	private IMongoDatabase GetDatabase<T>() where T : MongoObject
	{
		var databaseName = mongoNames.GetDatabaseName<T>();

		return mongoConnection.GetDatabase(databaseName);
	}
}