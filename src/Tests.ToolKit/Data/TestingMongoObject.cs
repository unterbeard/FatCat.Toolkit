using FatCat.Toolkit.Data;
using FatCat.Toolkit.Data.Mongo;

namespace Tests.FatCat.Toolkit.Data;

public class TestingMongoObject : MongoObject
{
	public string Name { get; set; }

	public int Number { get; set; }

	public DateTime SomeDate { get; set; }
}