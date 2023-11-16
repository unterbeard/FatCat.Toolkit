namespace FatCat.Toolkit.Data.Mongo;

public interface IMongoConnectionInformation
{
	string GetConnectionString();

	string GetDatabaseName();
}