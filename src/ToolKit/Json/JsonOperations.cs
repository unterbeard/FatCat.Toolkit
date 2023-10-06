#nullable enable
using FatCat.Toolkit.Data.Mongo;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FatCat.Toolkit.Json;

public interface IJsonOperations
{
	T? Deserialize<T>(string json);

	string Serialize(EqualObject dataObject);
}

public class JsonOperations : IJsonOperations
{
	public static JsonSerializerSettings JsonSettings { get; } =
		new()
		{
			TypeNameHandling = TypeNameHandling.All,
			NullValueHandling = NullValueHandling.Ignore,
			Converters = new List<JsonConverter> { new StringEnumConverter(), new ObjectIdConverter() }
		};

	public T? Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, JsonSettings);

	public string Serialize(EqualObject dataObject) => JsonConvert.SerializeObject(dataObject, JsonSettings);
}
