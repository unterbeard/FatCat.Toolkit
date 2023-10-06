#nullable enable
namespace FatCat.Toolkit.Injection;

#pragma warning disable CS8767
[Flags]
public enum ScopeOptions
{
	None = 0,
	SetLifetimeScope = 1,
}
