using System.Security.Cryptography;

namespace FatCat.Toolkit.Cryptography;

public interface IFatCatAesEncryption
{
	byte[] Decrypt(byte[] cypherData, byte[] key, byte[] iv);

	byte[] Encrypt(byte[] openData, byte[] key, byte[] iv);
}

public class FatCatAesEncryption : IFatCatAesEncryption
{
	public byte[] Decrypt(byte[] cypherData, byte[] key, byte[] iv)
	{
		using var aesAlg = Aes.Create();

		aesAlg.Key = key;
		aesAlg.IV = iv;
		var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

		using var msDecrypt = new MemoryStream(cypherData);

		using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

		using var msPlain = new MemoryStream();

		csDecrypt.CopyTo(msPlain);

		return msPlain.ToArray();
	}

	public byte[] Encrypt(byte[] openData, byte[] key, byte[] iv)
	{
		using var aesAlg = Aes.Create();

		aesAlg.Key = key;
		aesAlg.IV = iv;

		var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

		using var msEncrypt = new MemoryStream();

		using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
		{
			csEncrypt.Write(openData, 0, openData.Length);
		}

		return msEncrypt.ToArray();
	}
}
