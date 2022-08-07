using FatCat.Toolkit.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FatCat.Toolkit.Json;

public interface IJsonOperations
{
	T? FromJson<T>(string json) where T : EqualObject;

	string ToJson(EqualObject dataObject);
}

public class JsonOperations : IJsonOperations
{
	public static JsonSerializerSettings JsonSettings { get; } = new()
																{
																	TypeNameHandling = TypeNameHandling.All,
																	Converters = new List<JsonConverter>
																				{
																					new StringEnumConverter(),
																					new ObjectIdConverter()
																				}
																};

	public T? FromJson<T>(string json) where T : EqualObject => JsonConvert.DeserializeObject<T>(json, JsonSettings);

	public string ToJson(EqualObject dataObject) => JsonConvert.SerializeObject(dataObject, JsonSettings);
}