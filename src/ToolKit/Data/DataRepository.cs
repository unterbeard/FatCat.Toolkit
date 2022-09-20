using System.Linq.Expressions;

namespace FatCat.Toolkit.Data;

public interface IDataRepository<T> where T : DataObject
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