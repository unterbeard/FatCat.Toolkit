using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FatCat.Toolkit.Data.Mongo;

public interface IMongoRepository<T> : IDataRepository<T> where T : MongoObject
{
	string DatabaseName { get; }

	Task<T?> GetById(string id);

	Task<T?> GetById(ObjectId id);
}

public class MongoRepository<T> : IMongoRepository<T> where T : MongoObject, new()
{
	private readonly IMongoDataConnection mongoDataConnection;

	public IMongoCollection<T> Collection { get; }

	public string DatabaseName { get; }

	public MongoRepository(IMongoDataConnection mongoDataConnection,
							IMongoNames mongoNames)
	{
		this.mongoDataConnection = mongoDataConnection;

		Collection = mongoDataConnection.GetCollection<T>();
		DatabaseName = mongoNames.GetDatabaseName<T>();
	}

	public async Task<T> Create(T item)
	{
		await Collection.InsertOneAsync(item);

		return item;
	}

	public async Task<List<T>> Create(List<T> items)
	{
		foreach (var item in items) await Create(item);

		return items;
	}

	public async Task<T> Delete(T item)
	{
		await Collection.DeleteOneAsync(i => i.Id == item.Id);

		return item;
	}

	public async Task<List<T>> Delete(List<T> items)
	{
		foreach (var item in items) await Delete(item);

		return items;
	}

	public async Task<List<T>> GetAll()
	{
		var cursor = await Collection.FindAsync(i => true);

		return await cursor.ToListAsync();
	}

	public async Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter)
	{
		var cursor = await Collection.FindAsync(filter);

		return await cursor.ToListAsync();
	}

	public async Task<T?> GetByFilter(Expression<Func<T, bool>> filter)
	{
		var list = await GetAllByFilter(filter);

		return list.FirstOrDefault();
	}

	public async Task<T?> GetById(string id) => await GetByFilter(i => i.Id == new ObjectId(id));

	public async Task<T?> GetById(ObjectId id) => await GetByFilter(i => i.Id == id);

	public async Task<T?> GetFirst() => await GetByFilter(i => true);

	public async Task<T> GetFirstOrCreate()
	{
		var firstItem = await GetFirst();

		if (firstItem == null)
		{
			firstItem = new T();

			await Create(firstItem);
		}

		return firstItem;
	}

	public async Task<T> Update(T item)
	{
		await Collection.ReplaceOneAsync(i => i.Id == item.Id, item);

		return item;
	}

	public async Task<List<T>> Update(List<T> items)
	{
		foreach (var item in items) await Update(item);

		return items;
	}
}