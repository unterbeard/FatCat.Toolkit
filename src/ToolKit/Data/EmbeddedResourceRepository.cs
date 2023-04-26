using System.Reflection;

namespace FatCat.Toolkit.Data;

public interface IEmbeddedResourceRepository
{
	List<string> GetAllResourceNames(Assembly assembly);

	string GetText(Assembly assembly, string resourceName);
}

public class EmbeddedResourceRepository : IEmbeddedResourceRepository
{
	public List<string> GetAllResourceNames(Assembly assembly) => assembly.GetManifestResourceNames().ToList();

	public string GetText(Assembly assembly, string resourceName)
	{
		var manifestStream = assembly.GetManifestResourceStream(resourceName);

		if (manifestStream is null) return null;

		var streamReader = new StreamReader(manifestStream);

		return streamReader.ReadToEnd();
	}
}