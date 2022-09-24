using LiteDB;

namespace FatCat.Toolkit.Data.Lite;

public interface ILiteDbCollection<T> : IDisposable where T : LiteDbObject
{
	ILiteCollection<T> GetCollection(string fullDatabasePath);
}

public class LiteDbCollection<T> : ILiteDbCollection<T> where T : LiteDbObject
{
	private readonly IDataNames dataNames;
	private LiteDatabase? database;
	private ILiteCollection<T>? liteCollection;

	public LiteDbCollection(IDataNames dataNames) => this.dataNames = dataNames;

	public ILiteCollection<T> GetCollection(string fullDatabasePath)
	{
		if (liteCollection != null) return liteCollection;

		database = new LiteDatabase(fullDatabasePath);

		var collectionName = dataNames.GetCollectionName<T>();

		liteCollection = database.GetCollection<T>(collectionName);

		return liteCollection;
	}

	public void Dispose() => database?.Dispose();
}