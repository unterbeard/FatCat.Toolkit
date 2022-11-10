#nullable enable
using System.Linq.Expressions;
using LiteDB;

namespace FatCat.Toolkit.Data.Lite;

public interface ILiteDbRepository<T> : IDisposable, IDataRepository<T> where T : LiteDbObject
{
	T? GetById(int id);

	void SetDatabasePath(string databaseFullPath);
}

public class LiteDbRepository<T> : ILiteDbRepository<T> where T : LiteDbObject, new()
{
	private readonly ILiteDbConnection connection;

	public ILiteCollection<T>? Collection { get; set; }

	public string? DatabasePath { get; set; }

	public LiteDbRepository(ILiteDbConnection connection) => this.connection = connection;

	public Task<T> Create(T item)
	{
		Connect();

		var createdId = Collection?.Insert(item)!;

		item.Id = createdId.AsInt32;

		Disconnect();

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
		Connect();

		var bsonId = GetIdBsonValue(item);

		Collection?.Delete(bsonId);

		Disconnect();

		return Task.FromResult(item);
	}

	public async Task<List<T>> Delete(List<T> items)
	{
		foreach (var item in items) await Delete(item);

		return items;
	}

	public void Dispose() => Disconnect();

	public async Task<List<T>> GetAll()
	{
		Connect();

		await Task.CompletedTask;

		var result = Collection?.FindAll().ToList();

		Disconnect();

		return result ?? new List<T>();
	}

	public Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter)
	{
		Connect();

		var result = Collection?.Find(filter)!;

		Disconnect();

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

	public void SetDatabasePath(string databaseFullPath) => DatabasePath = databaseFullPath;

	public Task<T> Update(T item)
	{
		Connect();

		Collection?.Update(item);

		Disconnect();

		return Task.FromResult(item);
	}

	public async Task<List<T>> Update(List<T> items)
	{
		foreach (var item in items) await Update(item);

		return items;
	}

	private void Connect()
	{
		if (DatabasePath == null) throw new LiteDbConnectionException();

		connection.Connect(DatabasePath);

		Collection = connection.GetCollection<T>();
	}

	private void Disconnect()
	{
		Collection = null;

		connection.Dispose();
	}

	private BsonValue GetIdBsonValue(T item) => new(item.Id);
}