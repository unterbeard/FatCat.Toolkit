using Newtonsoft.Json;

namespace FatCat.Toolkit.Extensions;

public static class ObjectExtensions
{
	public static T? DeepCopy<T>(this T? objectToCopy) where T : class
	{
		if (objectToCopy == null) return null;

		var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

		var json = JsonConvert.SerializeObject(objectToCopy, settings);

		return JsonConvert.DeserializeObject<T>(json, settings);
	}
}