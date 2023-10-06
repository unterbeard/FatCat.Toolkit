using Newtonsoft.Json;

namespace FatCat.Toolkit.Web;

public static class Json
{
	private static readonly JsonConverter instance = new Lazy<JsonConverter>(() => new JsonConverter()).Value;

	public static string Serialize(object value) => instance.SerializeObject(value);

	public static string SerializeServiceModel(object value) =>
		instance.SerializeObject(value, Formatting.Indented, false);
}
