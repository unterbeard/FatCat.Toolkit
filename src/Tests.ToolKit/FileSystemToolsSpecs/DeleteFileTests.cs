using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class DeleteFileTests : FileToolsTests
{
	[Fact]
	public void CheckIfFileExists()
	{
		fileTools.DeleteFile(filePath);

		VerifyFileExistWasCalled();
	}

	[Fact]
	public void DeleteFileUsingFileSystem()
	{
		fileTools.DeleteFile(filePath);

		A.CallTo(() => fileSystem.File.Delete(filePath)).MustHaveHappened();
	}

	[Fact]
	public void IfFileDoesNotExistDoNotDelete()
	{
		SetFileDoesNotExist();

		fileTools.DeleteFile(filePath);

		A.CallTo(() => fileSystem.File.Delete(filePath)).MustNotHaveHappened();
	}

	[Fact]
	public void IfTheFileDoesNotExistReturnFalse()
	{
		SetFileDoesNotExist();

		fileTools.DeleteFile(filePath).Should().BeFalse();
	}

	[Fact]
	public void IfTheFileIsDeletedReturnTrue()
	{
		fileTools.DeleteFile(filePath).Should().BeTrue();
	}
}
