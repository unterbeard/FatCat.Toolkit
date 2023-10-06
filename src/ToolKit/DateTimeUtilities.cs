namespace FatCat.Toolkit;

public interface IDateTimeUtilities
{
	DateTime LocalNow();

	DateTime UtcNow();
}

public class DateTimeUtilities : IDateTimeUtilities
{
	public DateTime LocalNow()
	{
		return DateTime.Now;
	}

	public DateTime UtcNow()
	{
		return DateTime.UtcNow;
	}
}
