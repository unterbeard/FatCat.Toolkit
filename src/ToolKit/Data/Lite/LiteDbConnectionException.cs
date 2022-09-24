namespace FatCat.Toolkit.Data.Lite;

public class LiteDbConnectionException : Exception
{
	public LiteDbConnectionException()
		: base("You must connect to LiteDb") { }
}