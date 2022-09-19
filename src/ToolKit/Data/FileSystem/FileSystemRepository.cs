using System.Linq.Expressions;

namespace FatCat.Toolkit.Data.FileSystem;

public interface IFileSystemRepository<T> where T : FileSystemDataObject
{
	Task<T> Create(T item);

	Task<List<T>> Create(List<T> items);

	Task<T> Delete(T item);

	Task<List<T>> Delete(List<T> items);

	Task<List<T>> GetAll();

	Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter);

	Task<T?> GetByFilter(Expression<Func<T, bool>> filter);

	Task<T?> GetById(string id);

	Task<T?> GetFirst();

	Task<T> GetFirstOrCreate();

	Task<T> Update(T item);

	Task<List<T>> Update(List<T> items);
}

public class FileSystemRepository<T> : IFileSystemRepository<T> where T : FileSystemDataObject
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

	public Task<T> Update(T item) => throw new NotImplementedException();

	public Task<List<T>> Update(List<T> items) => throw new NotImplementedException();
}