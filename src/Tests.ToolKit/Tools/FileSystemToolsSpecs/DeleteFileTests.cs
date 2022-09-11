using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public class DeleteFileTests : FileToolTests
{
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
}