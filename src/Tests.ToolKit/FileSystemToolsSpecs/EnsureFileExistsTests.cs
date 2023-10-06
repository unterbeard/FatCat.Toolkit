namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class EnsureFileExistsTests : TestsToEnsureFileExists
{
	protected override Task RunMethodToTest()
	{
		fileTools.EnsureFile(filePath);

		return Task.CompletedTask;
	}
}
