using FatCat.Toolkit.Extensions;

namespace FatCat.Toolkit;

public interface ICollectionTools
{
	bool AreEqual<T>(IEnumerable<T> first, IEnumerable<T> second);
}

public class CollectionTools : ICollectionTools
{
	public bool AreEqual<T>(IEnumerable<T> first, IEnumerable<T> second) => first.ListsAreEqual(second);
}