namespace Tests.FatCat.Toolkit.Data.FIleSystem.FileSystemRepositorySpecs;

public class SaveTests : FileSystemRepositoryTests
{
	private readonly TestFileDataObject dataToSave;
	private string saveJson;

	public SaveTests()
	{
		dataToSave = Faker.Create<TestFileDataObject>();

		SetUpToJson();
	}

	[Fact]
	public async Task ConvertObjectToJson()
	{
		await repository.Save(dataToSave);

		A.CallTo(() => jsonHelper.Serialize(dataToSave)).MustHaveHappened();
	}

	[Fact]
	public async Task IfDirectoryDoesNotExistCreate()
	{
		A.CallTo(() => fileSystem.Directory.Exists(A<string>._)).Returns(false);

		await repository.Save(dataToSave);

		A.CallTo(() => fileSystem.Directory.CreateDirectory(DataDirectory)).MustHaveHappened();
	}

	[Fact]
	public async Task SaveJsonToFile()
	{
		await repository.Save(dataToSave);

		A.CallTo(() => fileSystem.File.WriteAllTextAsync(TestFileDataObjectPath, saveJson, default))
			.MustHaveHappened();
	}

	[Fact]
	public async Task TestFileDataObjectIsSavedOnRepository()
	{
		await repository.Save(dataToSave);

		repository.Data.Should().Be(dataToSave);
	}

	[Fact]
	public async Task VerifyDataDirectoryExists()
	{
		await repository.Save(dataToSave);

		A.CallTo(() => fileSystem.Directory.Exists(DataDirectory)).MustHaveHappened();
	}

	private void SetUpToJson()
	{
		saveJson = Faker.RandomString();

		A.CallTo(() => jsonHelper.Serialize(A<TestFileDataObject>._)).Returns(saveJson);
	}
}
