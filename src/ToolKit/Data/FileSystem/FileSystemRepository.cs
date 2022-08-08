using System.IO.Abstractions;
using FatCat.Toolkit.Json;

namespace FatCat.Toolkit.Data.FileSystem;

public interface ISingleItemFileSystemRepository<T> where T : FileSystemDataObject, new()
{
	Task<T> Get();

	Task Save(T item);
}

public class SingleItemFileSystemRepository<T> : ISingleItemFileSystemRepository<T> where T : FileSystemDataObject, new()
{
	private readonly IApplicationTools applicationTools;
	private readonly IFileSystem fileSystem;
	private readonly IJsonOperations jsonOperations;

	public T? Data { get; set; }

	private string DataDirectory => Path.Join(applicationTools.ExecutingDirectory, "Data");

	private bool DataDirectoryDoesNotExist => !fileSystem.Directory.Exists(DataDirectory);

	private bool DataFileNotFound => !fileSystem.File.Exists(DataPath);

	private string DataPath => Path.Join(DataDirectory, $"{typeof(T).Name}.data");

	public SingleItemFileSystemRepository(IFileSystem fileSystem,
										IApplicationTools applicationTools,
										IJsonOperations jsonOperations)
	{
		this.fileSystem = fileSystem;
		this.applicationTools = applicationTools;
		this.jsonOperations = jsonOperations;
	}

	public async Task<T> Get()
	{
		if (Data != null) return Data;
		if (DataDirectoryDoesNotExist || DataFileNotFound) return new T();

		var json = await fileSystem.File.ReadAllTextAsync(DataPath);

		Data = jsonOperations.FromJson<T>(json);

		return Data!;
	}

	public async Task Save(T item)
	{
		Data = item;

		var json = jsonOperations.ToJson(Data);

		if (DataDirectoryDoesNotExist) fileSystem.Directory.CreateDirectory(DataDirectory);

		await fileSystem.File.WriteAllTextAsync(DataPath, json);
	}
}