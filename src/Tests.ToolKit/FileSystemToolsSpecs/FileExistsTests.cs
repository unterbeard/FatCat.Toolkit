namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class FileExistsTests : FileToolsTests
{
	[Fact]
	public void CheckIfFileExists()
	{
		fileTools.FileExists(filePath);

		VerifyFileExistWasCalled();
	}

	[Fact]
	public void FalseIfFileIsNotFound()
	{
		SetFileDoesNotExist();

		fileTools.FileExists(filePath).Should().BeFalse();
	}

	[Fact]
	public void TrueIfFileExists()
	{
		fileTools.FileExists(filePath).Should().BeTrue();
	}
}
