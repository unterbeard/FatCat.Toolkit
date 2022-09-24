using LiteDB;

namespace FatCat.Toolkit.Data.Lite;

public interface ILiteDbConnection : IDisposable
{
	void Connect(string databasePath);

	ILiteCollection<T> GetCollection<T>() where T : LiteDbObject;
}

public class LiteDbConnection : ILiteDbConnection
{
	private readonly IDataNames dataNames;

	private LiteDatabase? database;

	public LiteDbConnection(IDataNames dataNames) => this.dataNames = dataNames;

	public void Connect(string databasePath)
	{
		if (database != null) return;

		database = new LiteDatabase(databasePath);
	}

	public void Dispose() => database?.Dispose();

	public ILiteCollection<T> GetCollection<T>() where T : LiteDbObject
	{
		if (database == null) throw new LiteDbConnectionException();

		var collectionName = dataNames.GetCollectionName<T>();

		var liteCollection = database.GetCollection<T>(collectionName);

		return liteCollection;
	}
}