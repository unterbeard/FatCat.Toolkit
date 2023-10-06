using FatCat.Fakes;
using FatCat.Toolkit;
using FatCat.Toolkit.Caching;
using Xunit;

namespace Tests.FatCat.Toolkit.Caching;

public class TestCacheItem : EqualObject, ICacheItem
{
	public string CacheId => SomeId;

	public string SomeId { get; set; }
}

public class CacheTests
{
	[Fact]
	public void CanCacheItems()
	{
		var items = Faker.Create<List<TestCacheItem>>();

		var cache = new FatCatCache<TestCacheItem>();

		foreach (var item in items)
		{
			cache.Add(item);
		}

		var allItems = cache.GetAll();
	}
}
