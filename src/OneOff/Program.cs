using FatCat.Toolkit.Console;
using LiteDB;
using Newtonsoft.Json;

ConsoleLog.LogCallerInformation = true;

try
{
	using var db = new LiteDatabase(@"C:\TempWorking\ToolkitSpike\LiteDatabaseSpike\ToolkitData.db");

	var collection = db.GetCollection<Customer>("customers");

	var customer = new Customer
					{
						Name = "John Doe",
						IsActive = true
					};

	var createdObject =  collection.Insert(customer);
	
	ConsoleLog.WriteCyan($"Created Object {createdObject}");

	var allCustomers = collection.Find(i => true).ToList();

	ConsoleLog.WriteCyan($"{JsonConvert.SerializeObject(allCustomers, Formatting.Indented)}");
}
catch (Exception ex) { ConsoleLog.WriteException(ex); }

public class Customer
{
	public int Id { get; set; }

	public bool IsActive { get; set; }

	public string? Name { get; set; }

	public List<string>? Phones { get; set; }
}