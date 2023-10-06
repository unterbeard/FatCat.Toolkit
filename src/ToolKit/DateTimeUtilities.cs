namespace FatCat.Toolkit;

public interface IDateTimeUtilities
{
	DateTime LocalNow();

	DateTime UtcNow();
}

public class DateTimeUtilities : IDateTimeUtilities
{
	public DateTime LocalNow() => DateTime.Now;

	public DateTime UtcNow() => DateTime.UtcNow;
}
