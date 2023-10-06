#nullable enable
using System.Collections.Concurrent;
using System.Reflection;
using Fasterflect;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace FatCat.Toolkit.Data.Mongo;

public interface IMongoConnection
{
	IMongoDatabase GetDatabase(string databaseName, string? connectionString);
}

public class MongoConnection : IMongoConnection
{
	private const string NotSetConnectionString = "NOT_SET";

	private static readonly MongoClientSettings defaultMongoClientSettings = new();

	private readonly ConcurrentDictionary<string, MongoClient> connections = new();

	private readonly ConcurrentDictionary<string, IMongoDatabase> databases = new();

	public MongoConnection(List<Assembly> dataAssemblies)
	{
		ConventionRegistry.Register(
			nameof(IgnoreExtraElementsConvention),
			new ConventionPack { new IgnoreExtraElementsConvention(true) },
			_ => true
		);
		// Yse if want to represent enums as strings
		// ConventionRegistry.Register(nameof(EnumRepresentationConvention), new ConventionPack { new EnumRepresentationConvention(BsonType.String) }, _ => true);

		foreach (var assembly in dataAssemblies)
		{
			foreach (var mongoObjectType in assembly.TypesImplementing<MongoObject>())
			{
				if (mongoObjectType.IsGenericTypeDefinition)
					continue;

				if (!BsonClassMap.IsClassMapRegistered(mongoObjectType))
					BsonClassMap.LookupClassMap(mongoObjectType);
			}
		}
	}

	public IMongoDatabase GetDatabase(string databaseName, string? connectionString)
	{
		if (databases.TryGetValue(databaseName, out var database))
			return database;

		if (connectionString == null)
			connectionString = NotSetConnectionString;

		if (!connections.TryGetValue(connectionString, out var mongoClient))
		{
			var settings =
				connectionString == NotSetConnectionString
					? defaultMongoClientSettings
					: MongoClientSettings.FromConnectionString(connectionString);

			mongoClient = new MongoClient(settings);
		}

		var newDatabaseConnection = mongoClient.GetDatabase(databaseName);

		databases.TryAdd(databaseName, newDatabaseConnection);

		return newDatabaseConnection;
	}
}
