using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FatCat.Toolkit.Data.FileSystem;

public interface IFileSystemRepository<T> where T : FileSystemDataObject
{
	Task<T> Get();

	Task Save(T item);
}

public class FileSystemRepository<T> : IFileSystemRepository<T> where T : FileSystemDataObject
{
	private readonly IFileSystem fileSystem;
	private readonly IApplicationTools applicationTools;
	private readonly IJsonHelper jsonHelper;

	public FileSystemRepository(IFileSystem fileSystem,
								IApplicationTools applicationTools,
								IJsonHelper jsonHelper)
	{
		this.fileSystem = fileSystem;
		this.applicationTools = applicationTools;
		this.jsonHelper = jsonHelper;
	}

	public Task<T> Get() => throw new NotImplementedException();

	public Task Save(T item) => throw new NotImplementedException();
}