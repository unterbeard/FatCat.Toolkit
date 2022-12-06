using FatCat.Toolkit.Caching;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Threading;
using Humanizer;
using Newtonsoft.Json;

namespace OneOff;

public class CacheWorker
{
	private readonly IFatCatCache<TestCacheItem> cache;
	private readonly IThread thread;

	public CacheWorker(IFatCatCache<TestCacheItem> cache,
						IThread thread)
	{
		this.cache = cache;
		this.thread = thread;
	}

	public void DoWork()
	{
		var firstItem = new TestCacheItem
						{
							Name = "First Item",
							Number = 1
						};

		cache.Add(firstItem, 250.Milliseconds());

		var secondItem = new TestCacheItem
						{
							Name = "Second Item",
							Number = 2
						};

		cache.Add(secondItem);

		var allItems = cache.GetAll();

		ConsoleLog.WriteCyan($"{JsonConvert.SerializeObject(allItems, Formatting.Indented)}");

		thread.Sleep(5.Seconds()).Wait();

		var currentItems = cache.GetAll();

		ConsoleLog.WriteMagenta($"{JsonConvert.SerializeObject(currentItems, Formatting.Indented)}");
	}
}

public class TestCacheItem : ICacheItem
{
	public string CacheId => Name;

	public string Name { get; set; }

	public int Number { get; set; }
}