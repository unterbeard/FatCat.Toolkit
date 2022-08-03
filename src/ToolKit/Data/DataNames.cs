using Humanizer;

namespace FatCat.Toolkit.Data;

public interface IDataNames
{
	string GetCollectionName<T>() where T : DataObject;

	string GetCollectionNameFromType(Type type);

	string GetDatabaseName<T>() where T : DataObject;

	string GetDatabaseNameFromType(Type type);
}

public class DataNames : IDataNames
{
	public string GetCollectionName<T>() where T : DataObject => GetCollectionNameFromType(typeof(T));

	public string GetCollectionNameFromType(Type type) => type.Name.Pluralize();

	public string GetDatabaseName<T>() where T : DataObject => GetDatabaseNameFromType(typeof(T));

	public string GetDatabaseNameFromType(Type type)
	{
		var typeNamespace = type.Namespace;

		var namespaceParts = typeNamespace!.Split('.');

		return $"{namespaceParts[0]}{(namespaceParts.Length > 1 ? namespaceParts[1] : string.Empty)}";
	}
}