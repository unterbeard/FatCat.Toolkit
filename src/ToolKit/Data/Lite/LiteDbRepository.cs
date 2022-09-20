using System.Linq.Expressions;

namespace FatCat.Toolkit.Data.Lite;

public interface ILiteDbRepository<T> : IDataRepository<T> where T : DataObject
{
	void LoadDatabase(string databaseFullPath);
}

public class LiteDbRepository<T> : ILiteDbRepository<T> where T : DataObject
{
	public Task<T> Create(T item) => throw new NotImplementedException();

	public Task<List<T>> Create(List<T> items) => throw new NotImplementedException();

	public Task<T> Delete(T item) => throw new NotImplementedException();

	public Task<List<T>> Delete(List<T> items) => throw new NotImplementedException();

	public Task<List<T>> GetAll() => throw new NotImplementedException();

	public Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter) => throw new NotImplementedException();

	public Task<T?> GetByFilter(Expression<Func<T, bool>> filter) => throw new NotImplementedException();

	public Task<T?> GetById(string id) => throw new NotImplementedException();

	public Task<T?> GetFirst() => throw new NotImplementedException();

	public Task<T> GetFirstOrCreate() => throw new NotImplementedException();

	public void LoadDatabase(string databaseFullPath) => throw new NotImplementedException();

	public Task<T> Update(T item) => throw new NotImplementedException();

	public Task<List<T>> Update(List<T> items) => throw new NotImplementedException();
}