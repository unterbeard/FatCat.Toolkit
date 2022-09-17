using System.Diagnostics;
using System.IO.Abstractions;
using FatCat.Toolkit;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Extensions;

ConsoleLog.LogCallerInformation = true;

async Task<byte[]> GetHash(byte[] bytesToHash)
{
	var hashTools = new HashTools();

	var watch = Stopwatch.StartNew();

	var bytes = await hashTools.CalculateHash(bytesToHash);

	watch.Stop();

	ConsoleLog.WriteMagenta($"Time to compute hash {watch.Elapsed}");

	return bytes;
}

try
{
	var fileTools = new FileSystemTools(new FileSystem());

	var testFilePath = @"C:\FogFileChunkTesting\FileToTestWith.txt";
	var otherFilePath = @"C:\FogFileChunkTesting\FileToTestWith.txt";

	var firstHash = await GetHash(await fileTools.ReadAllBytes(testFilePath));

	ConsoleLog.WriteBlue($"Hash.Length <{firstHash.Length}> | {firstHash.ToReadableString()} | {testFilePath}");

	var secondHash = await GetHash(await fileTools.ReadAllBytes(otherFilePath));

	ConsoleLog.WriteBlue($"Hash.Length <{secondHash.Length}> | {secondHash.ToReadableString()} | {otherFilePath}");
}
catch (Exception ex) { ConsoleLog.WriteException(ex); }