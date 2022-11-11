#nullable enable
using System.Collections.Concurrent;

namespace FatCat.Toolkit.Caching;

public interface IFatCatCache<T> where T : class, ICacheItem
{
	void Add(T cacheItem);

	void Add(List<T> cacheItems);

	void Clear();

	T? Get(string cacheId);

	IList<T> GetAll();

	bool InCache(string cacheId);

	public bool InCache(T cacheItem) => InCache(cacheItem.CacheId);

	void Remove(string cacheId);

	public void Remove(T cacheItem) => Remove(cacheItem.CacheId);
}

public class FatCatCache<T> : IFatCatCache<T> where T : class, ICacheItem
{
	private readonly ConcurrentDictionary<string, T> cache = new();

	public void Add(T cacheItem) => cache.AddOrUpdate(cacheItem.CacheId, cacheItem, (key, value) => cacheItem);

	public void Add(List<T> cacheItems)
	{
		foreach (var item in cacheItems) Add(item);
	}

	public void Clear() => cache.Clear();

	public T? Get(string cacheId)
	{
		cache.TryGetValue(cacheId, out var item);

		return item;
	}

	public IList<T> GetAll() => cache.Values.ToList();

	public bool InCache(string cacheId) => cache.ContainsKey(cacheId);

	public void Remove(string cacheId) => cache.TryRemove(cacheId, out _);
}