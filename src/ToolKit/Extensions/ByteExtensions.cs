using System.Text;

namespace FatCat.Toolkit.Extensions;

public static class ByteExtensions
{
	public static byte[] FromEncodedString(this string value) => Convert.FromBase64String(value);

	public static string ToEncodedString(this byte[] bytes) => Convert.ToBase64String(bytes);

	public static string ToReadableString(this byte[] bytes)
	{
		var finalString = new StringBuilder();

		for (var i = 0; i < bytes.Length; i++)
		{
			var b = bytes[i];

			finalString.Append($"{b:X2}");

			if ((i + 1) % 2 == 0)
			{
				finalString.Append(" ");
			}
		}

		return finalString.ToString();
	}
}
