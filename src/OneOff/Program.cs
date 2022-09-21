using FatCat.Toolkit.Console;
using FatCat.Toolkit.Data;
using FatCat.Toolkit.Data.Lite;
using Newtonsoft.Json;

ConsoleLog.LogCallerInformation = true;

try
{
	using var liteRepository = new LiteDbRepository<Customer>(new LiteDbConnection<Customer>(new DataNames()));

	var databaseFullPath = @"C:\TempWorking\ToolkitSpike\LiteDatabaseSpike\ToolkitData.db";
	
	liteRepository.Connect(databaseFullPath);

	var customer = new Customer
					{
						Name = "John Doe",
						IsActive = true
					};

	var createdObject = await liteRepository.Create(customer);

	ConsoleLog.WriteCyan($"Created Object {JsonConvert.SerializeObject(createdObject, Formatting.Indented)}");
}
catch (Exception ex) { ConsoleLog.WriteException(ex); }

public class Customer : LiteDbObject
{
	public bool IsActive { get; set; }

	public string? Name { get; set; }

	public List<string>? Phones { get; set; }
}