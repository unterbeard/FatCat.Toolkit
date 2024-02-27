namespace FatCat.Toolkit.Cryptography;

public interface IAesKeyGenerator
{
	byte[] CreateIv();

	byte[] CreateKey(AesKeySize keySize);
}

public class AesKeyGenerator(IGenerator generator) : IAesKeyGenerator
{
	public byte[] CreateIv()
	{
		return generator.Bytes(16).ToArray();
	}

	public byte[] CreateKey(AesKeySize keySize)
	{
		return generator.Bytes((int)keySize / 8).ToArray();
	}
}
