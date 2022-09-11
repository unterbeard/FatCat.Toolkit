using System.IO.Abstractions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Tools;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public class DeleteFileTests
{
	private readonly string filePath;
	private readonly IFileSystem fileSystem;
	private readonly FileSystemTools tools;
	private bool fileExists = true;

	public DeleteFileTests()
	{
		fileSystem = A.Fake<IFileSystem>();

		tools = new FileSystemTools(fileSystem);

		filePath = Faker.RandomString();

		A.CallTo(() => fileSystem.File.Exists(A<string>._))
		.ReturnsLazily(() => fileExists);
	}

	[Fact]
	public void CheckIfFileExists()
	{
		tools.DeleteFile(filePath);

		A.CallTo(() => fileSystem.File.Exists(filePath))
		.MustHaveHappened();
	}

	[Fact]
	public void DeleteFileUsingFileSystem()
	{
		tools.DeleteFile(filePath);

		A.CallTo(() => fileSystem.File.Delete(filePath))
		.MustHaveHappened();
	}

	[Fact]
	public void IfFileDoesNotExistDoNotDelete()
	{
		SetFileDoesNotExist();

		tools.DeleteFile(filePath);

		A.CallTo(() => fileSystem.File.Delete(filePath))
		.MustNotHaveHappened();
	}

	[Fact]
	public void IfTheFileDoesNotExistReturnFalse()
	{
		SetFileDoesNotExist();

		tools.DeleteFile(filePath)
			.Should()
			.BeFalse();
	}

	[Fact]
	public void IfTheFileIsDeletedReturnTrue()
	{
		tools.DeleteFile(filePath)
			.Should()
			.BeTrue();
	}

	private void SetFileDoesNotExist() => fileExists = false;
}