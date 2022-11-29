using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class FileExistsTests : FileToolTests
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

		fileTools.FileExists(filePath)
				.Should()
				.BeFalse();
	}

	[Fact]
	public void TrueIfFileExists()
	{
		fileTools.FileExists(filePath)
				.Should()
				.BeTrue();
	}
}