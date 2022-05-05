using System.Diagnostics;
using FatCat.Toolkit.Extensions;

namespace FatCat.Toolkit;

public interface IApplicationTools
{
	string? ExecutingDirectory { get; }
}

public class ApplicationTools : IApplicationTools
{
	private static string? executingDirectory;

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