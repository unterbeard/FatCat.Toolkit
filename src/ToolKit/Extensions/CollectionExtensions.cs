#nullable enable
using Fasterflect;

namespace FatCat.Toolkit.Extensions;

public static class CollectionExtensions
{
	public static bool ListsAreEqual<T>(this IEnumerable<T>? firstList, IEnumerable<T>? secondList)
	{
		if (firstList == null && secondList == null) return true;

		if (firstList == null || secondList == null) return false;

		var firstCopy = new List<T>(firstList);
		var secondCopy = new List<T>(secondList);

		if (typeof(T).Implements<IComparable<T>>() || typeof(T).Implements<IComparable>())
		{
			var orderFirstList = firstCopy.OrderBy(i => i).ToList();
			var orderSecondList = secondCopy.OrderBy(i => i).ToList();

			return orderFirstList.SequenceEqual(orderSecondList);
		}

		return firstCopy.SequenceEqual(secondCopy);
	}
}