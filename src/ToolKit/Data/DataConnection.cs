using MongoDB.Driver;

namespace FatCat.Toolkit.Data;

public interface IDataConnection
{
	IMongoCollection<T> GetCollection<T>() where T : DataObject;
}

public class DataConnection : IDataConnection
{
	private readonly IDataNames dataNames;
	private readonly IMongoConnection mongoConnection;

	public DataConnection(IDataNames dataNames,
						IMongoConnection mongoConnection)
	{
		this.dataNames = dataNames;
		this.mongoConnection = mongoConnection;
	}

	public IMongoCollection<T> GetCollection<T>() where T : DataObject
	{
		var database = GetDatabase<T>();

		return database.GetCollection<T>(dataNames.GetCollectionName<T>());
	}

	private IMongoDatabase GetDatabase<T>() where T : DataObject
	{
		var databaseName = dataNames.GetDatabaseName<T>();

		return mongoConnection.GetDatabase(databaseName);
	}
}