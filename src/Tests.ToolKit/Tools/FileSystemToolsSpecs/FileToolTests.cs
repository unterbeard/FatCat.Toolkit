using System.IO.Abstractions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Tools;

namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public abstract class FileToolTests
{
	protected readonly string filePath;
	protected readonly IFileSystem fileSystem;
	protected readonly FileSystemTools tools;
	private bool fileExists = true;

	protected FileToolTests()
	{
		fileSystem = A.Fake<IFileSystem>();

		tools = new FileSystemTools(fileSystem);

		filePath = Faker.RandomString();

		A.CallTo(() => fileSystem.File.Exists(A<string>._))
		.ReturnsLazily(() => fileExists);
	}

	protected void SetFileDoesNotExist() => fileExists = false;
}