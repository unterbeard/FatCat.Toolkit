using System.Reflection;

namespace FatCat.Toolkit.Data;

public interface IEmbeddedResourceRepository
{
	List<string> GetAllResourceNames(Assembly assembly);

	Stream GetStream(Assembly assembly, string resourceName);

	string GetText(Assembly assembly, string resourceName);
}

public class EmbeddedResourceRepository : IEmbeddedResourceRepository
{
	public List<string> GetAllResourceNames(Assembly assembly)
	{
		return assembly.GetManifestResourceNames().ToList();
	}

	public Stream GetStream(Assembly assembly, string resourceName)
	{
		var manifestStream = assembly.GetManifestResourceStream(resourceName);

		return manifestStream;
	}

	public string GetText(Assembly assembly, string resourceName)
	{
		var manifestStream = GetStream(assembly, resourceName);

		if (manifestStream is null)
		{
			return null;
		}

		var streamReader = new StreamReader(manifestStream);

		return streamReader.ReadToEnd();
	}
}
