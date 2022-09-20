using Humanizer;

namespace FatCat.Toolkit.Data.Mongo;

public interface IMongoNames
{
	string GetCollectionName<T>() where T : MongoObject;

	string GetCollectionNameFromType(Type type);

	string GetDatabaseName<T>() where T : MongoObject;

	string GetDatabaseNameFromType(Type type);
}

public class MongoNames : IMongoNames
{
	public string GetCollectionName<T>() where T : MongoObject => GetCollectionNameFromType(typeof(T));

	public string GetCollectionNameFromType(Type type) => type.Name.Pluralize();

	public string GetDatabaseName<T>() where T : MongoObject => GetDatabaseNameFromType(typeof(T));

	public string GetDatabaseNameFromType(Type type)
	{
		var typeNamespace = type.Namespace;

		var namespaceParts = typeNamespace!.Split('.');

		return $"{namespaceParts[0]}{(namespaceParts.Length > 1 ? namespaceParts[1] : string.Empty)}";
	}
}