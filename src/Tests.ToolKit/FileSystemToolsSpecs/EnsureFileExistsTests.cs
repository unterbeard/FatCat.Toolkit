namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class EnsureFileExistsTests : TestsToEnsureFileExists
{
	protected override void RunMethodToTest() { fileTools.EnsureFile(filePath); }
}