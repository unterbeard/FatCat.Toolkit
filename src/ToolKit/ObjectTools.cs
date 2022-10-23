namespace FatCat.Toolkit;

public interface IObjectTools
{
	bool IsEquals(object obj1, object obj2);
}

public class ObjectTools : IObjectTools
{
	public bool IsEquals(object obj1, object obj2) => obj1.Equals(obj2);
}