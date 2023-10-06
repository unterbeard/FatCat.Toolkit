using System.Text;
using Castle.Core.Internal;
using FatCat.Toolkit.Extensions;

namespace FatCat.Toolkit;

public interface IByteTools
{
	string FromBase64Encoded(byte[] bytes);

	byte[] ToBase64Encoded(string text);
}

public class ByteTools : IByteTools
{
	public string FromBase64Encoded(byte[] bytes)
	{
		if (bytes.IsNullOrEmpty())
		{
			return string.Empty;
		}

		var base64Text = Convert.ToBase64String(bytes);

		var plainTextBytes = Convert.FromBase64String(base64Text);

		return Encoding.UTF8.GetString(plainTextBytes);
	}

	public byte[] ToBase64Encoded(string text)
	{
		if (text.IsNullOrEmpty())
		{
			return Array.Empty<byte>();
		}

		var plainTextBytes = Encoding.UTF8.GetBytes(text);

		var base64Text = Convert.ToBase64String(plainTextBytes);

		return Convert.FromBase64String(base64Text);
	}
}
