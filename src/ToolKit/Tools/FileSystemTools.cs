using System.IO.Abstractions;

namespace FatCat.Toolkit.Tools;

public interface IFileSystemTools
{
	void DeleteDirectory(string path);

	bool DeleteFile(string path);

	bool DirectoryExists(string path);

	void EnsureDirectory(string path);

	bool FileExists(string path);

	Task WriteAllBytes(string path, byte[] bytes);

	Task WriteAllText(string path, string text);
}

public class FileSystemTools : IFileSystemTools
{
	private readonly IFileSystem fileSystem;

	public FileSystemTools(IFileSystem fileSystem) => this.fileSystem = fileSystem;

	public void DeleteDirectory(string path) { throw new NotImplementedException(); }

	public bool DeleteFile(string path)
	{
		if (!FileExists(path)) return false;

		fileSystem.File.Delete(path);

		return true;
	}

	public bool DirectoryExists(string path) => throw new NotImplementedException();

	public void EnsureDirectory(string path) { throw new NotImplementedException(); }

	public bool FileExists(string path) => fileSystem.File.Exists(path);

	public Task WriteAllBytes(string path, byte[] bytes) => throw new NotImplementedException();

	public Task WriteAllText(string path, string text) => throw new NotImplementedException();
}