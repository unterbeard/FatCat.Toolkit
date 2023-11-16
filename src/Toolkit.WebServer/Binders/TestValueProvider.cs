using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FatCat.Toolkit.WebServer.Binders;

public class TestValueProvider : IValueProvider
{
	private readonly Dictionary<string, string> values = new();

	public void AddTestValues(string name, string value) { values.Add(name, value); }

	public bool ContainsPrefix(string prefix) => values.ContainsKey(prefix);

	public ValueProviderResult GetValue(string key)
	{
		if (values.TryGetValue(key, out var value)) { return new ValueProviderResult(value); }

		return ValueProviderResult.None;
	}

	public void RemoveTestValues(string name) { values.Remove(name); }
}