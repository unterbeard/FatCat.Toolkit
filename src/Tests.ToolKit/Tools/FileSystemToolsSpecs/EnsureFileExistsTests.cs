namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public class EnsureFileExistsTests : TestsToEnsureFileExists
{
	protected override void RunMethodToTest() { fileTools.EnsureFile(filePath); }
}