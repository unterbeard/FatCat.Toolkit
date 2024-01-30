using DebounceThrottle;

namespace FatCat.Toolkit.Debouncing;

public interface ICatBounce
{
	void Debounce(Action action);

	void Throttle(Action action);

	void ThrottleAsync(Func<Task> action);
}

public class CatBounce(TimeSpan interval) : ICatBounce
{
	private readonly DebounceDispatcher debounceDispatcher = new((int)interval.TotalMilliseconds);
	private readonly ThrottleDispatcher throttleDispatcher = new((int)interval.TotalMilliseconds);

	public void Debounce(Action action)
	{
		debounceDispatcher.Debounce(action);
	}

	public void Throttle(Action action)
	{
		throttleDispatcher.Throttle(action);
	}

	public void ThrottleAsync(Func<Task> action)
	{
		throttleDispatcher.ThrottleAsync(action).Wait();
	}
}
