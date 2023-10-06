namespace FatCat.Toolkit.Data.Mongo;

public class EnvironmentConnectionInformation : IMongoConnectionInformation
{
	private readonly IEnvironmentRepository environmentRepository;

	public EnvironmentConnectionInformation(IEnvironmentRepository environmentRepository)
	{
		this.environmentRepository = environmentRepository;
	}

	public string GetConnectionString()
	{
		return environmentRepository.Get("MongoConnectionString");
	}

	public string GetDatabaseName()
	{
		return environmentRepository.Get("MongoDatabaseName");
	}
}
