using FatCat.Toolkit.Extensions;

namespace FatCat.Toolkit;

public interface IObjectTools
{
	bool IsEquals(EqualObject? obj1, EqualObject? obj2);
}

public class ObjectTools : IObjectTools
{
	public bool IsEquals(EqualObject? obj1, EqualObject? obj2) => ObjectEquals.AreEqual(obj1, obj2);
}