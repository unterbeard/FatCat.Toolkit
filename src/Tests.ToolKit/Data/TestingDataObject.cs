using FatCat.Toolkit.Data;

namespace Tests.FatCat.Toolkit.Data;

public class TestingDataObject : DataObject
{
	public string Name { get; set; }

	public int Number { get; set; }

	public DateTime SomeDate { get; set; }
}