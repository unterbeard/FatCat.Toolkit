using Autofac;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Data.Mongo;
using FatCat.Toolkit.Injection;

namespace OneOff;

public static class Program
{
	public static async Task Main(params string[] args)
	{
		ConsoleLog.LogCallerInformation = true;

		try
		{
			var builder = new ContainerBuilder();

			SystemScope.Initialize(builder, ScopeOptions.SetLifetimeScope);

			var mongoConnectionString = @"mongodb://localhost:27017";
			var databaseName = "CustomName34";

			var mongoRepository = SystemScope.Container.Resolve<IMongoRepository<Customer>>();

			mongoRepository.Connect(mongoConnectionString, databaseName);

			var testObject = Faker.Create<Customer>();

			await mongoRepository.Create(testObject);
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}
}

public class Customer : MongoObject
{
	public bool IsActive { get; set; }

	public string Name { get; set; }

	public List<string> Phones { get; set; }
}