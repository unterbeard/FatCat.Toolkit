namespace FatCat.Toolkit;

public interface IDateTimeUtilities
{
	DateTime UtcNow();
}

public class DateTimeUtilities : IDateTimeUtilities
{
	public DateTime UtcNow() => DateTime.UtcNow;
}