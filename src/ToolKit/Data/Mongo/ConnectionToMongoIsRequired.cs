namespace FatCat.Toolkit.Data.Mongo;

public class ConnectionToMongoIsRequired : Exception
{
	public ConnectionToMongoIsRequired()
		: base("A connection to mongo is required.  Ensure you call Connect before using repository") { }
}
