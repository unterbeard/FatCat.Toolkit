using System.Diagnostics;
using FatCat.Toolkit.Extensions;

namespace FatCat.Toolkit;

public interface IApplicationTools
{
	string ExecutableName { get; }

	string? ExecutingDirectory { get; }
}

public class ApplicationTools : IApplicationTools
{
	private string? executableName;
	private string? executingDirectory;

	public string ExecutableName
	{
		get
		{
			if (executableName.IsNullOrEmpty())
			{
				var fileName = Process.GetCurrentProcess().MainModule?.FileName;

				if (fileName.IsNotNullOrEmpty()) executableName = Path.GetFileNameWithoutExtension(fileName);
			}

			return executableName;
		}
	}

	public string? ExecutingDirectory
	{
		get
		{
			if (executingDirectory.IsNullOrEmpty())
			{
				var fileName = Process.GetCurrentProcess().MainModule?.FileName;

				if (fileName.IsNotNullOrEmpty()) executingDirectory = Path.GetDirectoryName(fileName);
			}

			return executingDirectory;
		}
	}
}