namespace FatCat.Toolkit.Data.Mongo;

public class EnvironmentConnectionInformation : IMongoConnectionInformation
{
	private readonly IEnvironmentRepository environmentRepository;

	public EnvironmentConnectionInformation(IEnvironmentRepository environmentRepository) =>
		this.environmentRepository = environmentRepository;

	public string GetConnectionString() => environmentRepository.Get("MongoConnectionString");

	public string GetDatabaseName() => environmentRepository.Get("MongoDatabaseName");
}
