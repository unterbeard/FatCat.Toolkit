using System.IO.Abstractions;

namespace FatCat.Toolkit;

public interface IFileSystemTools
{
	Task AppendToFile(string path, string text);

	void DeleteDirectory(string path, bool recursive = true);

	bool DeleteFile(string path);

	bool DirectoryExists(string path);

	void EnsureDirectory(string path);

	void EnsureFile(string path);

	bool FileExists(string path);

	Task<byte[]> ReadAllBytes(string path);

	Task<List<string>> ReadAllLines(string path);

	Task<string> ReadAllText(string path);

	Task WriteAllBytes(string path, byte[] bytes);

	Task WriteAllText(string path, string text);
}

public class FileSystemTools : IFileSystemTools
{
	private readonly IFileSystem fileSystem;

	public FileSystemTools(IFileSystem fileSystem) => this.fileSystem = fileSystem;

	public async Task AppendToFile(string path, string text)
	{
		EnsureFile(path);

		await fileSystem.File.AppendAllTextAsync(path, text);
	}

	public void DeleteDirectory(string path, bool recursive = true)
	{
		if (DirectoryExists(path)) fileSystem.Directory.Delete(path, recursive);
	}

	public bool DeleteFile(string path)
	{
		if (!FileExists(path)) return false;

		fileSystem.File.Delete(path);

		return true;
	}

	public bool DirectoryExists(string path) => fileSystem.Directory.Exists(path);

	public void EnsureDirectory(string path)
	{
		if (DirectoryExists(path)) return;

		fileSystem.Directory.CreateDirectory(path);
	}

	public void EnsureFile(string path)
	{
		EnsureDirectory(Path.GetDirectoryName(path)!);

		if (FileExists(path)) return;

		using var _ = fileSystem.File.Create(path);
	}

	public bool FileExists(string path) => fileSystem.File.Exists(path);

	public async Task<byte[]> ReadAllBytes(string path) => FileDoesNotExist(path) ? Array.Empty<byte>() : await fileSystem.File.ReadAllBytesAsync(path);

	public async Task<List<string>> ReadAllLines(string path)
	{
		if (!fileSystem.File.Exists(path)) return new List<string>();

		var lines = await fileSystem.File.ReadAllLinesAsync(path);

		return lines.ToList();
	}

	public async Task<string> ReadAllText(string path) => FileDoesNotExist(path) ? string.Empty : await fileSystem.File.ReadAllTextAsync(path);

	public async Task WriteAllBytes(string path, byte[] bytes)
	{
		EnsureFile(path);

		await fileSystem.File.WriteAllBytesAsync(path, bytes);
	}

	public async Task WriteAllText(string path, string text)
	{
		EnsureFile(path);

		await fileSystem.File.WriteAllTextAsync(path, text);
	}

	private bool FileDoesNotExist(string path) => !FileExists(path);
}