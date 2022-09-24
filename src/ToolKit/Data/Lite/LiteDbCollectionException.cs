namespace FatCat.Toolkit.Data.Lite;

public class LiteDbCollectionException : Exception
{
	public LiteDbCollectionException()
		: base("You must get the collection from LiteDb to use LiteDbRepository") { }
}