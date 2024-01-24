using System.Security.Cryptography;

namespace FatCat.Toolkit;

public interface IHashTools
{
	Task<byte[]> CalculateHash(byte[] data);

	Task<string> CalculateHash(string data);

	bool HashEquals(byte[] hash1, byte[] hash2);
}

public class HashTools : IHashTools
{
	public async Task<byte[]> CalculateHash(byte[] data)
	{
		return await new ByteHashProcessor().CalculateHash(data);
	}

	public async Task<string> CalculateHash(string data)
	{
		return await new StringHashProcessor().CalculateHash(data);
	}

	public bool HashEquals(byte[] hash1, byte[] hash2)
	{
		return hash1.SequenceEqual(hash2);
	}

	private class ByteHashProcessor : IDisposable
	{
		private readonly MemoryStream memoryStream = new();

		public async Task<byte[]> CalculateHash(byte[] data)
		{
			WriteBytesToMemoryStream(data);

			var hash = await GetMd5Hash();

			return hash;
		}

		public void Dispose()
		{
			memoryStream.Dispose();
		}

		private async Task<byte[]> GetMd5Hash()
		{
			using var md5 = MD5.Create();

			return await md5.ComputeHashAsync(memoryStream);
		}

		private void WriteBytesToMemoryStream(byte[] value)
		{
			var streamWriter = new BinaryWriter(memoryStream);

			streamWriter.Write(value);

			memoryStream.Seek(0, SeekOrigin.Begin);
		}
	}

	private class StringHashProcessor
	{
		private readonly MemoryStream memoryStream;

		public StringHashProcessor()
		{
			memoryStream = new MemoryStream();
		}

		public async Task<string> CalculateHash(string value)
		{
			await WriteToMemoryStream(value);
			var hash = await GetMd5Hash();

			return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
		}

		private async Task<byte[]> GetMd5Hash()
		{
			using var md5 = MD5.Create();

			var hash = await md5.ComputeHashAsync(memoryStream);
			return hash;
		}

		private async Task WriteToMemoryStream(string value)
		{
			var streamWriter = new StreamWriter(memoryStream);

			await streamWriter.WriteAsync(value);

			memoryStream.Seek(0, SeekOrigin.Begin);
		}
	}
}
