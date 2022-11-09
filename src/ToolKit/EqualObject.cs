#nullable enable
using FatCat.Toolkit.Extensions;

namespace FatCat.Toolkit;

public class CompareExclude : Attribute { }

public abstract class EqualObject
{
	public static bool operator ==(EqualObject? rhs, EqualObject? lhs) => ObjectEquals.EqualsOperator(rhs, lhs);

	public static bool operator !=(EqualObject? rhs, EqualObject? lhs) => !ObjectEquals.EqualsOperator(rhs, lhs);

	public override bool Equals(object? obj) => obj != null && ObjectEquals.AreEqual(this, obj as EqualObject);

	public override int GetHashCode() => ObjectEquals.GenerateHashCode(this);
}