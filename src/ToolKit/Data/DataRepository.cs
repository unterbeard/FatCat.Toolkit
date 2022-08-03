using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FatCat.Toolkit.Data;

public interface IDataRepository<T> where T : DataObject
{
	string DatabaseName { get; }

	Task<T> Create(T item);

	Task<List<T>> Create(List<T> items);

	Task<T> Delete(T item);

	Task<List<T>> Delete(List<T> items);

	Task<List<T>> GetAll();

	Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter);

	Task<T?> GetByFilter(Expression<Func<T, bool>> filter);

	Task<T?> GetById(string id);

	Task<T?> GetById(ObjectId id);

	Task<T?> GetFirst();

	Task<T> GetFirstOrCreate();

	Task<T> Update(T item);

	Task<List<T>> Update(List<T> items);
}

public class DataRepository<T> : IDataRepository<T> where T : DataObject, new()
{
	private readonly IDataConnection dataConnection;

	public IMongoCollection<T> Collection { get; }

	public string DatabaseName { get; }

	public DataRepository(IDataConnection dataConnection,
						IDataNames dataNames)
	{
		this.dataConnection = dataConnection;

		Collection = dataConnection.GetCollection<T>();
		DatabaseName = dataNames.GetDatabaseName<T>();
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