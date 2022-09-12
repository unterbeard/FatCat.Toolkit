using FatCat.Toolkit;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Extensions;

ConsoleLog.LogCallerInformation = true;

async Task<byte[]> GetHash(string s)
{
	var fileBytes = await File.ReadAllBytesAsync(s);

	var hashTools = new HashTools();

	var bytes = await hashTools.CalculateHash(fileBytes);
	return bytes;
}

try
{
	var testFilePath = @"D:\FogFIleChunkTesting\FileToTestWith.txt";
	var otherFilePath = @"D:\FogFIleChunkTesting\ReAssembledFileToTestWith.txt";

	var firstHash = await GetHash(testFilePath);

	ConsoleLog.WriteBlue($"Hash.Length <{firstHash.Length}> | {firstHash.ToReadableString()} | {testFilePath}");

	var secondHash = await GetHash(otherFilePath);

	ConsoleLog.WriteBlue($"Hash.Length <{secondHash.Length}> | {secondHash.ToReadableString()} | {otherFilePath}");
}
catch (Exception ex) { ConsoleLog.WriteException(ex); }