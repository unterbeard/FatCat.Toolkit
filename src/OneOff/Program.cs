using System.Reflection;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Data;
using FatCat.Toolkit.Data.Mongo;

namespace OneOff;

public static class Program
{
	public static async Task Main(params string[] args)
	{
		ConsoleLog.LogCallerInformation = true;

		try
		{
			var mongoConnectionString = @"mongodb://localhost:27017";
			var databaseName = "CustomName34";

			var mongoRepository = new MongoRepository<Customer>(new MongoDataConnection(new MongoNames(new DataNames()), new MongoConnection(new List<Assembly> { typeof(Program).Assembly })),
																new MongoNames(new DataNames()));

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