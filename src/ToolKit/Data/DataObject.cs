using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FatCat.Toolkit.Data;

public abstract class DataObject : EqualObject
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	[System.Text.Json.Serialization.JsonConverter(typeof(ObjectIdConverter))]
	public ObjectId Id { get; set; }

	protected DataObject() => Id = ObjectId.GenerateNewId();
}