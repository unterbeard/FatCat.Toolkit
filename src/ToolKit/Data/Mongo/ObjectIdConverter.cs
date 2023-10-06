#nullable enable
using MongoDB.Bson;
using Newtonsoft.Json;

namespace FatCat.Toolkit.Data.Mongo;

public class ObjectIdConverter : JsonConverter
{
	public override bool CanRead => true;

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(ObjectId);
	}

	public override object ReadJson(
		JsonReader reader,
		Type objectType,
		object? existingValue,
		JsonSerializer serializer
	)
	{
		if (reader.Value == null)
		{
			return new ObjectId();
		}

		var objectIdString = reader.Value.ToString();

		return new ObjectId(objectIdString);
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
		if (value == null)
		{
			writer.WriteNull();

			return;
		}

		var objectId = (ObjectId)value;

		var finalString = objectId.ToString();

		writer.WriteValue(finalString);
	}
}
