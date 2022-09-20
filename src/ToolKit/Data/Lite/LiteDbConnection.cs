using LiteDB;

namespace FatCat.Toolkit.Data.Lite;

public interface ILiteDbConnection<T> : IDisposable where T : LiteDbObject
{
	ILiteCollection<T> Connect(string fullDatabasePath);
}

public class LiteDbConnection<T> : ILiteDbConnection<T> where T : LiteDbObject
{
	private readonly IDataNames dataNames;
	private LiteDatabase? database;
	private ILiteCollection<T>? liteCollection;

	public LiteDbConnection(IDataNames dataNames) => this.dataNames = dataNames;

	public ILiteCollection<T> Connect(string fullDatabasePath)
	{
		if (liteCollection != null) return liteCollection;

		database = new LiteDatabase(fullDatabasePath);

		var collectionName = dataNames.GetCollectionName<T>();

		liteCollection = database.GetCollection<T>(collectionName);

		return liteCollection;
	}

	public void Dispose() => database?.Dispose();
}