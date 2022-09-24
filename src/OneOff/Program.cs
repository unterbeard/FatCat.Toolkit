using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Data;
using FatCat.Toolkit.Data.Lite;
using Newtonsoft.Json;

ConsoleLog.LogCallerInformation = true;

try
{
	using var liteRepository = new LiteDbRepository<Customer>(new LiteDbConnection(new DataNames()));

	var databaseFullPath = @"C:\TempWorking\ToolkitSpike\LiteDatabaseSpike\ToolkitData.db";

	liteRepository.Connect(databaseFullPath);

	var customer = Faker.Create<Customer>(afterCreate: i => i.Id = default);

	var createdObject = await liteRepository.Create(customer);

	ConsoleLog.WriteCyan($"Created Object {JsonConvert.SerializeObject(createdObject, Formatting.Indented)}");

	var allObjects = liteRepository.Collection.FindAll()!;

	ConsoleLog.WriteMagenta($"{JsonConvert.SerializeObject(allObjects, Formatting.Indented)}");

	const string nameToFind = "9462bc67a";

	var foundItems = await liteRepository.GetAllByFilter(i => i.Name == nameToFind);

	ConsoleLog.WriteBlue($"{JsonConvert.SerializeObject(foundItems, Formatting.Indented)}");
}
catch (Exception ex) { ConsoleLog.WriteException(ex); }

public class Customer : LiteDbObject
{
	public bool IsActive { get; set; }

	public string Name { get; set; }

	public List<string> Phones { get; set; }
}