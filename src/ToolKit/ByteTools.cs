namespace FatCat.Toolkit;

public interface IByteTools
{
	byte[] FromBase64String(string text);

	string ToBase64String(byte[] bytes);
}

public class ByteTools : IByteTools
{
	public byte[] FromBase64String(string text)
	{
		return Convert.FromBase64String(text);
	}

	public string ToBase64String(byte[] bytes)
	{
		return Convert.ToBase64String(bytes);
	}
}
