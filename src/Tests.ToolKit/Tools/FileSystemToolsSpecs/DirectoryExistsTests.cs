using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public class DirectoryExistsTests : FileToolTests
{
	[Fact]
	public void CheckIfDirectoryExistsInFileSystem()
	{
		tools.DirectoryExists(directoryPath);

		VerifyDirectoryExistsWasCalled();
	}

	

	[Fact]
	public void FalseIfDirectoryIsNotFound()
	{
		directoryExists = false;

		tools.DirectoryExists(directoryPath)
			.Should()
			.BeFalse();
	}

	[Fact]
	public void TrueIfDirectoryIsFound()
	{
		tools.DirectoryExists(directoryPath)
			.Should()
			.BeTrue();
	}
}