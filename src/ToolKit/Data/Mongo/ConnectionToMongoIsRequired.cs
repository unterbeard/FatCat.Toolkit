namespace FatCat.Toolkit.Data.Mongo;

public class ConnectionToMongoIsRequired()
	: Exception("A connection to mongo is required.  Ensure you call Connect before using repository");
