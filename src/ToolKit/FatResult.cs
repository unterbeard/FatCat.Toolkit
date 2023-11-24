namespace FatCat.Toolkit;

public class FatResult<T> : EqualObject
{
	public T Data { get; set; }

	public bool IsSuccessful { get; set; }

	public bool IsUnsuccessful
	{
		get => !IsSuccessful;
	}

	public string Message { get; set; }
}
