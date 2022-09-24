using System.Linq.Expressions;
using LiteDB;

namespace FatCat.Toolkit.Data.Lite;

public interface ILiteDbRepository<T> : IDisposable, IDataRepository<T> where T : LiteDbObject
{
	void Connect(string databaseFullPath);

	T? GetById(int id);
}

public class LiteDbRepository<T> : ILiteDbRepository<T> where T : LiteDbObject, new()
{
	private readonly ILiteDbCollection<T> liteDbCollection;

	public ILiteCollection<T>? Collection { get; set; }

	public LiteDbRepository(ILiteDbCollection<T> liteDbCollection) => this.liteDbCollection = liteDbCollection;

	public void Connect(string databaseFullPath) => Collection = liteDbCollection.GetCollection(databaseFullPath);

	public Task<T> Create(T item)
	{
		if (Collection == null) throw new LiteDbCollectionException();

		var createdId = Collection.Insert(item);

		item.Id = createdId.AsInt32;

		return Task.FromResult(item);
	}

	public async Task<List<T>> Create(List<T> items)
	{
		var resultList = new List<T>();

		foreach (var item in items)
		{
			var createdItem = await Create(item);

			resultList.Add(createdItem);
		}

		return resultList;
	}

	public Task<T> Delete(T item)
	{
		EnsureCollection();

		var bsonId = GetIdBsonValue(item);

		Collection?.Delete(bsonId);

		return Task.FromResult(item);
	}

	public async Task<List<T>> Delete(List<T> items)
	{
		foreach (var item in items) await Delete(item);

		return items;
	}

	public void Dispose() => liteDbCollection.Dispose();

	public async Task<List<T>> GetAll()
	{
		EnsureCollection();

		await Task.CompletedTask;

		var result = Collection?.FindAll().ToList();

		return result ?? new List<T>();
	}

	public Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter)
	{
		EnsureCollection();

		var result = Collection?.Find(filter)!;

		return Task.FromResult(result.ToList());
	}

	public async Task<T?> GetByFilter(Expression<Func<T, bool>> filter)
	{
		var result = await GetAllByFilter(filter);

		return result.FirstOrDefault();
	}

	public T? GetById(int id) => GetByFilter(i => i.Id == id).Result;

	public async Task<T?> GetFirst()
	{
		var allItems = await GetAll();

		return allItems.FirstOrDefault();
	}

	public async Task<T> GetFirstOrCreate()
	{
		var item = await GetFirst();

		if (item == null) item = await Create(new T());

		return item;
	}

	public Task<T> Update(T item)
	{
		EnsureCollection();

		Collection?.Update(item);

		return Task.FromResult(item);
	}

	public async Task<List<T>> Update(List<T> items)
	{
		foreach (var item in items) await Update(item);

		return items;
	}

	private void EnsureCollection()
	{
		if (Collection == null) throw new LiteDbCollectionException();
	}

	private BsonValue GetIdBsonValue(T item) => new(item.Id);
}