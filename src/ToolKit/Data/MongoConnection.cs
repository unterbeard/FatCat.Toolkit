using System.Collections.Concurrent;
using System.Reflection;
using Fasterflect;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace FatCat.Toolkit.Data;

public interface IMongoConnection
{
	IMongoDatabase GetDatabase(string databaseName);
}

public class MongoConnection : IMongoConnection
{
	private static readonly MongoClientSettings mongoClientSettings = new()
																	{
																		MinConnectionPoolSize = 100,
																		MaxConnectionPoolSize = 1001
																	};

	private readonly ConcurrentDictionary<string, IMongoDatabase> databases = new();

	private readonly MongoClient mongoClient = new(mongoClientSettings);

	public MongoConnection(List<Assembly> dataAssemblies)
	{
		ConventionRegistry.Register(nameof(IgnoreExtraElementsConvention), new ConventionPack { new IgnoreExtraElementsConvention(true) }, _ => true);
		ConventionRegistry.Register(nameof(EnumRepresentationConvention), new ConventionPack { new EnumRepresentationConvention(BsonType.String) }, _ => true);

		foreach (var assembly in dataAssemblies)
		{
			foreach (var mongoObjectType in assembly.TypesImplementing<DataObject>())
			{
				if (mongoObjectType.IsGenericTypeDefinition) continue;

				if (!BsonClassMap.IsClassMapRegistered(mongoObjectType)) BsonClassMap.LookupClassMap(mongoObjectType);
			}
		}
	}

	public IMongoDatabase GetDatabase(string databaseName)
	{
		if (databases.TryGetValue(databaseName, out var database)) return database;

		var newDatabaseConnection = mongoClient.GetDatabase(databaseName);

		databases.TryAdd(databaseName, newDatabaseConnection);

		return newDatabaseConnection;
	}
}