#nullable enable
using System.Collections;
using FatCat.Toolkit.Extensions;
using Newtonsoft.Json;

namespace FatCat.Toolkit.Web;

internal static class ContentConverter
{
	private static Dictionary<Type, Func<string, object>> TypeSetters { get; } = new();

	public static T? ConvertTo<T>(this WebResult result)
	{
		if (result.Content.IsNullOrEmpty())
		{
			return ToDefaultValue<T>();
		}

		if (TypeSetters.ContainsKey(typeof(T)))
		{
			return (T)TypeSetters[typeof(T)](result.Content!);
		}

		return JsonConvert.DeserializeObject<T>(result.Content!);
	}

	static ContentConverter()
	{
		TypeSetters.Add(typeof(string), SetString);
		TypeSetters.Add(typeof(int), SetInt);
		TypeSetters.Add(typeof(double), SetDouble);
		TypeSetters.Add(typeof(DateTime), SetDateTime);
		TypeSetters.Add(typeof(bool), SetBool);
		TypeSetters.Add(typeof(Guid), SetGuid);
		TypeSetters.Add(typeof(TimeSpan), SetTimespan);
	}

	private static T CreateEmptyList<T>(Type typeToCreate)
	{
		var itemType = typeToCreate.GetGenericArguments()[0];

		var genericListType = typeof(List<>);

		var subCombinedType = genericListType.MakeGenericType(itemType);
		var listAsInstance = Activator.CreateInstance(subCombinedType);

		return (T)listAsInstance!;
	}

	private static bool IsList(Type type)
	{
		return type.IsGenericType && type.Implements(typeof(IEnumerable));
	}

	private static object SetBool(string content)
	{
		return bool.Parse(content);
	}

	private static object SetDateTime(string content)
	{
		return DateTime.Parse(content);
	}

	private static object SetDouble(string content)
	{
		return double.Parse(content);
	}

	private static object SetGuid(string content)
	{
		return Guid.Parse(content);
	}

	private static object SetInt(string content)
	{
		return int.Parse(content);
	}

	private static object SetString(string content)
	{
		return content;
	}

	private static object SetTimespan(string content)
	{
		return TimeSpan.Parse(content);
	}

	private static T? ToDefaultValue<T>()
	{
		var typeToCreate = typeof(T);

		return IsList(typeToCreate) ? CreateEmptyList<T>(typeToCreate) : default;
	}
}
