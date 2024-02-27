using FatCat.Toolkit;
using FatCat.Toolkit.Cryptography;

namespace Tests.FatCat.Toolkit.Cryptography;

public class FatCatAesEncryptionTests
{
	private readonly FatCatAesEncryption encryption = new();
	private readonly AesKeyGenerator keyGenerator = new(new Generator());
	private readonly byte[] openData = Faker.RandomBytes(1024 * 8);

	[Theory]
	[InlineData(AesKeySize.Aes128)]
	[InlineData(AesKeySize.Aes192)]
	[InlineData(AesKeySize.Aes256)]
	public void CanDecryptDataBackToTheSameOpen(AesKeySize keySize)
	{
		var key = keyGenerator.CreateKey(keySize);
		var iv = keyGenerator.CreateIv();

		var encryptedData = encryption.Encrypt(openData, key, iv);

		var decryptedData = encryption.Decrypt(encryptedData, key, iv);

		decryptedData.Should().BeEquivalentTo(openData);
	}

	[Theory]
	[InlineData(AesKeySize.Aes128)]
	[InlineData(AesKeySize.Aes192)]
	[InlineData(AesKeySize.Aes256)]
	public void CanEncryptAByteArray(AesKeySize keySize)
	{
		var key = keyGenerator.CreateKey(keySize);
		var iv = keyGenerator.CreateIv();

		var encryptedData = encryption.Encrypt(openData, key, iv);

		encryptedData.Should().NotBeEquivalentTo(openData);
	}
}
