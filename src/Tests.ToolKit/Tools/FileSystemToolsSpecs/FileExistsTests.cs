using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public class FileExistsTests : FileToolTests
{
	[Fact]
	public void CheckIfFileExists()
	{
		tools.FileExists(filePath);

		VerifyFileExistWasCalled();
	}

	[Fact]
	public void FalseIfFileIsNotFound()
	{
		SetFileDoesNotExist();

		tools.FileExists(filePath)
			.Should()
			.BeFalse();
	}

	[Fact]
	public void TrueIfFileExists()
	{
		tools.FileExists(filePath)
			.Should()
			.BeTrue();
	}
}