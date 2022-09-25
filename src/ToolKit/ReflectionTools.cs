using System.Reflection;
using Fasterflect;

namespace FatCat.Toolkit;

public interface IReflectionTools
{
	List<Type> FindTypesImplementing<T>(List<Assembly> assemblies);
}

public class ReflectionTools : IReflectionTools
{
	public List<Type> FindTypesImplementing<T>(List<Assembly> assemblies)
	{
		var foundTypes = new List<Type>();

		foreach (var assembly in assemblies)
		{
			var typesImplementing = assembly.TypesImplementing<T>();

			foundTypes.AddRange(typesImplementing);
		}

		return foundTypes;
	}
}