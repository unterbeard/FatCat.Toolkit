using FatCat.Toolkit.Extensions;

namespace FatCat.Toolkit.Utilities;

public interface IByteUtilities
{
	string ToReadableString(byte[] bytes);
}

public class ByteUtilities : IByteUtilities
{
	public string ToReadableString(byte[] bytes)
	{
		return bytes.ToReadableString();
	}
}
