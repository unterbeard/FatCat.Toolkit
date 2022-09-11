using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public class EnsureFileTests : FileToolTests
{
	[Fact]
	public void CheckIfFileExists()
	{
		fileTools.EnsureFile(filePath);

		VerifyFileExistWasCalled();
	}

	[Fact]
	public void CreateFileIfItDoesNotExist()
	{
		SetFileDoesNotExist();

		fileTools.EnsureFile(filePath);

		A.CallTo(() => fileSystem.File.Create(filePath))
		.MustHaveHappened();
	}

	[Fact]
	public void DoNotCreateFileIfItIsFound()
	{
		fileTools.EnsureFile(filePath);

		A.CallTo(() => fileSystem.File.Create(A<string>._))
		.MustNotHaveHappened();
	}
}