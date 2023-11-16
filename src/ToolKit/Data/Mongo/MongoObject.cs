using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FatCat.Toolkit.Data.Mongo;

public abstract class MongoObject : DataObject
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	[JsonConverter(typeof(ObjectIdConverter))]
	public ObjectId Id { get; set; }
}
