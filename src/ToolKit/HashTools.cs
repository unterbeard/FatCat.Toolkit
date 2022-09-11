using System.Security.Cryptography;

namespace FatCat.Toolkit;

public interface IHashTools
{
	Task<byte[]> CalculateHash(byte[] data);

	Task<string> CalculateHash(string data);
}

public class HashTools : IHashTools
{
	public async Task<byte[]> CalculateHash(byte[] data) => await new ByteHashProcessor().CalculateHash(data);

	public async Task<string> CalculateHash(string data) => await new StringHashProcessor().CalculateHash(data);

	private class ByteHashProcessor : IDisposable
	{
		private readonly MemoryStream memoryStream;

		public ByteHashProcessor() => memoryStream = new MemoryStream();

		public async Task<byte[]> CalculateHash(byte[] data)
		{
			WriteBytesToMemoryStream(data);

			var hash = await GetMd5Hash();

			return hash;
		}

		public void Dispose() => memoryStream.Dispose();

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

		public StringHashProcessor() => memoryStream = new MemoryStream();

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