using Autofac;
using Autofac.Core;
using FatCat.Toolkit.Data.Mongo;
using FatCat.Toolkit.Injection;

namespace FatCat.Toolkit.Data;

public class DataModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterInstance(new MongoConnection(SystemScope.ContainerAssemblies.ToList()))
				.As<IMongoConnection>()
				.SingleInstance();

		builder.RegisterGeneric(typeof(MongoRepository<>))
				.As(typeof(IMongoRepository<>))
				.OnActivated(MongoRepositoryActivated);
	}

	private void MongoRepositoryActivated(IActivatedEventArgs<object> args)
	{
		var connectMethodName = nameof(IMongoRepository<MongoObject>.Connect);

		var connectMethod = args.Instance.GetType().GetMethod(connectMethodName);

		var environmentRepository = args.Context.Resolve<IEnvironmentRepository>();

		var connectionString = environmentRepository.Get("MongoConnectionString");
		var databaseName = environmentRepository.Get("MongoDatabaseName");

		connectMethod!.Invoke(args.Instance, new object?[] { connectionString, databaseName });
	}
}