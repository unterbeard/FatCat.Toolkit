#nullable enable
using System.Reflection;
using Fasterflect;

namespace FatCat.Toolkit;

public interface IReflectionTools
{
	TAttribute? FindAttributeOnType<TAttribute>(Type type)
		where TAttribute : Attribute;

	List<Type> FindTypesImplementing<TTypeImplementing>(List<Assembly> assemblies);

	List<Assembly> GetDomainAssemblies();
}

public class ReflectionTools : IReflectionTools
{
	public TAttribute? FindAttributeOnType<TAttribute>(Type type)
		where TAttribute : Attribute => type.GetCustomAttribute<TAttribute>();

	public List<Type> FindTypesImplementing<TTypeImplementing>(List<Assembly> assemblies)
	{
		var foundTypes = new List<Type>();

		foreach (var assembly in assemblies)
		{
			var typesImplementing = assembly.TypesImplementing<TTypeImplementing>();

			foundTypes.AddRange(typesImplementing);
		}

		return foundTypes;
	}

	public List<Assembly> GetDomainAssemblies() => AppDomain.CurrentDomain.GetAssemblies().ToList();
}
