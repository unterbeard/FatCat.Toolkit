namespace FatCat.Toolkit;

/// <summary>
///  Will write a {ApplicationName}.log file to executing directory to be used as a quick and easy way
///  to log messages to the file system without a lot of setup
/// </summary>
public interface ISimpleLogger
{
	
}

public class SimpleLogger : ISimpleLogger { }