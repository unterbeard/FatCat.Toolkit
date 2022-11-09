#nullable enable
using FatCat.Toolkit.Extensions;

namespace FatCat.Toolkit;

public interface IObjectTools
{
	T? DeepClone<T>(T? obj) where T : class;

	bool IsEquals(EqualObject? obj1, EqualObject? obj2);
}

public class ObjectTools : IObjectTools
{
	public T? DeepClone<T>(T? obj) where T : class => obj?.DeepCopy();

	public bool IsEquals(EqualObject? obj1, EqualObject? obj2) => ObjectEquals.AreEqual(obj1, obj2);
}