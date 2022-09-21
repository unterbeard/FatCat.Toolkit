using System.Linq.Expressions;
using LiteDB;

namespace FatCat.Toolkit.Data.Lite;

public interface ILiteDbRepository<T> : IDataRepository<T> where T : LiteDbObject
{
	void Connect(string databaseFullPath);

	T GetById(int id);
}

public class LiteDbRepository<T> : ILiteDbRepository<T> where T : LiteDbObject
{
	private readonly ILiteDbConnection<T> liteDbConnection;

	public ILiteCollection<T>? Collection { get; set; }

	public LiteDbRepository(ILiteDbConnection<T> liteDbConnection) => this.liteDbConnection = liteDbConnection;

	public void Connect(string databaseFullPath) => Collection = liteDbConnection.Connect(databaseFullPath);

	public Task<T> Create(T item)
	{
		if (Collection == null) throw new LiteDbConnectionException();

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

	public async Task<List<T>> GetAll()
	{
		EnsureCollection();

		await Task.CompletedTask;

		var result = Collection?.FindAll().ToList();

		return result ?? new List<T>();
	}

	public Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter) => throw new NotImplementedException();

	public Task<T?> GetByFilter(Expression<Func<T, bool>> filter) => throw new NotImplementedException();

	public T GetById(int id) => throw new NotImplementedException();

	public Task<T?> GetById(string id) => throw new NotImplementedException();

	public Task<T?> GetFirst() => throw new NotImplementedException();

	public Task<T> GetFirstOrCreate() => throw new NotImplementedException();

	public Task<T> Update(T item) => throw new NotImplementedException();

	public Task<List<T>> Update(List<T> items) => throw new NotImplementedException();

	private void EnsureCollection()
	{
		if (Collection == null) throw new LiteDbConnectionException();
	}

	private BsonValue GetIdBsonValue(T item) => new(item.Id);
}