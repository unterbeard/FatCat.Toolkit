#nullable enable
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

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
	private readonly MemoryCache memoryCache;

	public FatCatCache() => memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

	public virtual void Add(T cacheItem)
	{
		if (InCache(cacheItem.CacheId)) Remove(cacheItem.CacheId);

		memoryCache.Set(cacheItem.CacheId, cacheItem, CreateEntryOptions());
	}

	public virtual void Add(List<T> cacheItems) => cacheItems.ForEach(Add);

	public void Clear()
	{
		var allItems = GetAll();

		foreach (var item in allItems) Remove(item.CacheId);
	}

	public T? Get(string cacheId)
	{
		if (memoryCache.TryGetValue(cacheId, out var item)) return item as T;

		return null;
	}

	public IList<T> GetAll()
	{
		var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance)!;

		var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(memoryCache) as dynamic;

		var cacheCollectionValues = new List<ICacheEntry>();

		foreach (var cacheItem in cacheEntriesCollection!)
		{
			ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);

			cacheCollectionValues.Add(cacheItemValue);
		}

		return cacheCollectionValues.Select(i => i.Value).Cast<T>().ToList();
	}

	public bool InCache(string cacheId) => memoryCache.TryGetValue(cacheId, out _);

	public void Remove(string cacheId) => memoryCache.Remove(cacheId);

	private static MemoryCacheEntryOptions CreateEntryOptions() => new() { Priority = CacheItemPriority.NeverRemove };
}