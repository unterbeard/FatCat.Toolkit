using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Caching;

namespace FatCat.Toolkit.Testing;

public class FakeCache<T> : IFatCatCache<T> where T : class, ICacheItem
{
	public IFatCatCache<T> Cache { get; }

	public T CacheItem { get; set; }

	public List<T> CacheList { get; set; }

	public FakeCache()
	{
		Cache = A.Fake<IFatCatCache<T>>();
		CacheItem = Faker.Create<T>();

		A.CallTo(() => Cache.Get(A<string>._))
		.ReturnsLazily(() => CacheItem);

		CacheList = Faker.Create<List<T>>();

		A.CallTo(() => Cache.GetAll())
		.ReturnsLazily(() => CacheList);
	}

	public void Add(T cacheItem) => Cache.Add(cacheItem);

	public void Add(List<T> cacheItems) => Cache.Add(cacheItems);

	public void Clear() => Cache.Clear();

	public T? Get(string cacheId) => Cache.Get(cacheId);

	public IList<T> GetAll() => Cache.GetAll();

	public bool InCache(string cacheId) => Cache.InCache(cacheId);

	public void Remove(string cacheId) => Cache.Remove(cacheId);

	public void SetItemInCache()
	{
		A.CallTo(() => Cache.InCache(A<string>._))
		.Returns(true);
	}

	public void SetItemNotInCache()
	{
		A.CallTo(() => Cache.InCache(A<string>._))
		.Returns(false);
	}

	public void VerifyAdd(T? expectedItem = null)
	{
		if (expectedItem == null)
		{
			A.CallTo(() => Cache.Add(A<T>._))
			.MustHaveHappened();
		}
		else
		{
			A.CallTo(() => Cache.Add(expectedItem))
			.MustHaveHappened();
		}
	}
}