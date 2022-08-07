using System.IO.Abstractions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit;
using FatCat.Toolkit.Data.FileSystem;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tests.FatCat.Toolkit.Data.FIleSystem.FileSystemRepositorySpecs;

public class GetTests { }

public abstract class FileSystemRepositoryTests
{
	protected const string ExecutingDirectory = @"C:\Some\Install\Directory";

	protected static string DataDirectory => Path.Join(ExecutingDirectory, "Data");

	protected static string TestFileDataObjectPath => Path.Join(DataDirectory, "TestFileDataObject.data");

	protected readonly FileSystemRepository<TestFileDataObject> repository;
	protected IApplicationTools applicationTools = null!;
	protected string dataJson = null!;
	protected IFileSystem fileSystem = null!;
	protected IJsonHelper jsonHelper = null!;
	protected TestFileDataObject TestFileDataObject = null!;

	protected FileSystemRepositoryTests()
	{
		SetUpFileSystem();
		SetUpApplicationTools();
		SetUpJsonHelper();

		repository = new FileSystemRepository<TestFileDataObject>(fileSystem,
																applicationTools,
																jsonHelper);
	}

	private void SetUpApplicationTools()
	{
		applicationTools = A.Fake<IApplicationTools>();

		A.CallTo(() => applicationTools.ExecutingDirectory)
		.Returns(ExecutingDirectory);
	}

	private void SetUpFileSystem()
	{
		fileSystem = A.Fake<IFileSystem>();

		dataJson = Faker.RandomString();

		A.CallTo(() => fileSystem.File.ReadAllTextAsync(A<string>._, default))
		.Returns(dataJson);
	}

	private void SetUpJsonHelper()
	{
		jsonHelper = A.Fake<IJsonHelper>();

		TestFileDataObject = Faker.Create<TestFileDataObject>();

		A.CallTo(() => jsonHelper.FromJson<TestFileDataObject>(A<string>._))
		.Returns(TestFileDataObject);
	}
}

public class TestFileDataObject : FileSystemDataObject
{
	public string? FirstName { get; set; }

	public DateTime JoinedDate { get; set; }

	public string? LastName { get; set; }
}