using Tests.FatCat.Toolkit.Data.FIleSystem.FileSystemRepositorySpecs;

namespace Tests.FatCat.Toolkit.Data.FIleSystem;

public class ExistsTests : FileSystemRepositoryTests
{
	[Fact]
	public void CheckIfFileExists()
	{
		repository.Exists();

		A.CallTo(() => fileSystem.File.Exists(TestFileDataObjectPath)).MustHaveHappened();
	}

	[Fact]
	public void ReturnsFalseIfFileDoesNotExist()
	{
		SetUpFileExists(false);

		repository.Exists().Should().BeFalse();
	}

	[Fact]
	public void ReturnsTrueIfFileExists()
	{
		SetUpFileExists(true);

		repository.Exists().Should().BeTrue();
	}

	private void SetUpFileExists(bool value)
	{
		A.CallTo(() => fileSystem.File.Exists(A<string>._)).Returns(value);
	}
}
