using FatCat.Toolkit;

namespace Tests.FatCat.Toolkit.Learning;

public static class FuzzySearchingExtensions
{
	public static List<T> FuzzySearch<T>(this List<T> list, string search, Func<T, string> searchProperty)
	{
		var foundItems = new List<T>();

		foreach (var item in list)
		{
			var propertyValue = searchProperty(item);

			if (propertyValue.Contains(search, StringComparison.OrdinalIgnoreCase))
			{
				foundItems.Add(item);
			}
		}

		return foundItems;
	}
}

public class FuzzySearching
{
	private readonly List<SearchObject> searchList =
	[
		new SearchObject { FirstName = "Joe", LastName = "Burrow" },
		new SearchObject { FirstName = "Ja'Marr", LastName = "Chase" },
		new SearchObject { FirstName = "Joe", LastName = "Mixon" },
		new SearchObject { FirstName = "Joe", LastName = "Montana" },
		new SearchObject { FirstName = "Zack", LastName = "Taylor" },
		new SearchObject { FirstName = "Jason", LastName = "Taylor" },
		new SearchObject { FirstName = "Trader", LastName = "Joe" }
	];

	[Fact]
	public void CanFindAllTheJoeOnFirstName()
	{
		var search = "Joe";

		var foundJoe = searchList.FuzzySearch(search, x => x.FirstName);

		foundJoe.Count.Should().Be(3);
	}

	private class SearchObject : EqualObject
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}
