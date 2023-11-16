using FatCat.Toolkit.Data.Mongo;

namespace SampleDocker;

public class TestDataObject : MongoObject
{
	public string FirstName { get; set; }

	public int Number { get; set; }
}
