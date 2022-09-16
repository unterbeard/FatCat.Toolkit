using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public abstract class TestsToEnsureFileExists : FileToolTests
{
	[Fact]
	public void CheckIfDirectoryExists()
	{
		RunMethodToTest();

		VerifyDirectoryExistsWasCalled();
	}

	[Fact]
	public void CheckIfFileExists()
	{
		RunMethodToTest();

		VerifyFileExistWasCalled();
	}

	[Fact]
	public void CreateFileIfItDoesNotExist()
	{
		SetFileDoesNotExist();

		RunMethodToTest();

		A.CallTo(() => fileSystem.File.Create(filePath))
		.MustHaveHappened();
	}

	[Fact]
	public void DoNotCreateFileIfItIsFound()
	{
		RunMethodToTest();

		A.CallTo(() => fileSystem.File.Create(A<string>._))
		.MustNotHaveHappened();
	}

	[Fact]
	public void IfDirectoryDoesNotExistCreate()
	{
		directoryExists = false;

		RunMethodToTest();

		A.CallTo(() => fileSystem.Directory.CreateDirectory(directoryPath))
		.MustHaveHappened();
	}

	[Fact]
	public void IfDirectoryIsFoundDoNotCreate()
	{
		RunMethodToTest();

		A.CallTo(() => fileSystem.Directory.CreateDirectory(A<string>._))
		.MustNotHaveHappened();
	}

	protected abstract Task RunMethodToTest();
}