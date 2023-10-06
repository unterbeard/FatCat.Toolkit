#nullable enable
using FatCat.Toolkit.Extensions;

namespace FatCat.Toolkit;

public class CompareExclude : Attribute { }

public abstract class EqualObject
{
	public static bool operator ==(EqualObject? rhs, EqualObject? lhs)
	{
		return ObjectEquals.EqualsOperator(rhs, lhs);
	}

	public static bool operator !=(EqualObject? rhs, EqualObject? lhs)
	{
		return !ObjectEquals.EqualsOperator(rhs, lhs);
	}

	public override bool Equals(object? obj)
	{
		return obj != null && ObjectEquals.AreEqual(this, obj as EqualObject);
	}

	public override int GetHashCode()
	{
		return ObjectEquals.GenerateHashCode(this);
	}
}
