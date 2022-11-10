#nullable enable
namespace FatCat.Toolkit.Extensions;

public static class TypeExtensions
{
	public static bool Implements(this Type? type, Type? interfaceType)
	{
		if (type == null || interfaceType == null || type == interfaceType) return false;

		return (interfaceType.IsGenericTypeDefinition && type.GetInterfaces().Where(t => t.IsGenericType).Select(t => t.GetGenericTypeDefinition()).Any(gt => gt == interfaceType)) || interfaceType.IsAssignableFrom(type);
	}
}