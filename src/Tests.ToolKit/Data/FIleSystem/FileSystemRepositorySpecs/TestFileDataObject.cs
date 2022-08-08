using FatCat.Toolkit.Data.FileSystem;

namespace Tests.FatCat.Toolkit.Data.FIleSystem.FileSystemRepositorySpecs;

public class TestFileDataObject : FileSystemDataObject
{
	public string? FirstName { get; set; }

	public DateTime JoinedDate { get; set; }

	public string? LastName { get; set; }
}