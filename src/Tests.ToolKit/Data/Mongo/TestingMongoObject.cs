using FatCat.Toolkit.Data.Mongo;

namespace Tests.FatCat.Toolkit.Data.Mongo;

public class TestingMongoObject : MongoObject
{
	public string Name { get; set; }

	public int Number { get; set; }

	public DateTime SomeDate { get; set; }
}
