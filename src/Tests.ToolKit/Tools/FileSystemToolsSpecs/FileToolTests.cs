using System.IO.Abstractions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Tools;

namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public abstract class FileToolTests
{
	protected bool directoryExists = true;
	protected readonly string filePath;
	protected readonly string directoryPath;
	protected readonly IFileSystem fileSystem;
	protected readonly FileSystemTools tools;
	private bool fileExists = true;

	protected FileToolTests()
	{
		fileSystem = A.Fake<IFileSystem>();

		tools = new FileSystemTools(fileSystem);

		filePath = Faker.RandomString();
		directoryPath = Faker.RandomString();

		SetUpFileExists();
		SetUpDirectoryExists();
	}

	protected void SetFileDoesNotExist() => fileExists = false;

	protected void VerifyFileExistWasCalled()
	{
		A.CallTo(() => fileSystem.File.Exists(filePath))
		.MustHaveHappened();
	}

	private void SetUpDirectoryExists()
	{
		A.CallTo(() => fileSystem.Directory.Exists(A<string>._))
		.ReturnsLazily(() => directoryExists);
	}

	private void SetUpFileExists()
	{
		A.CallTo(() => fileSystem.File.Exists(A<string>._))
		.ReturnsLazily(() => fileExists);
	}
}