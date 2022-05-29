namespace FatCat.Toolkit.Extensions;

internal static class CollectionExtensions
{
	public static bool ListsAreEqual<T>(this IEnumerable<T>? firstList, IEnumerable<T>? secondList)
	{
		if (firstList == null && secondList == null) return true;

		if (firstList == null || secondList == null) return false;

		var firstCopy = new List<T>(firstList);
		var secondCopy = new List<T>(secondList);

		return firstCopy.SequenceEqual(secondCopy);
	}
}