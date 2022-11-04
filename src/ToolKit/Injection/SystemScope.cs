using System.Reflection;
using Autofac;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Enumerations;

#pragma warning disable CS8767

namespace FatCat.Toolkit.Injection;

public interface ISystemScope
{
	Assembly[] SystemAssemblies { get; }

	TItem Resolve<TItem>() where TItem : class;

	object Resolve(Type type);

	bool TryResolve(Type type, out object instance);

	bool TryResolve<TItem>(out TItem instance) where TItem : class;
}

[Flags]
public enum ScopeOptions
{
	None = 0,
	SetLifetimeScope = 1,
}

public class SystemScope : ISystemScope
{
	private static readonly Assembly[] defaultAssemblies = { typeof(SystemScope).Assembly, Assembly.GetEntryAssembly()! };

	private static readonly Lazy<SystemScope> instance = new(() => new SystemScope());

	public static SystemScope Container => instance.Value;

	public static Assembly[] LoadedAssemblies { get; private set; } = defaultAssemblies;

	public static void Initialize(ContainerBuilder builder, ScopeOptions options = ScopeOptions.None) => Initialize(builder, defaultAssemblies.ToList(), options);

	public static void Initialize(ContainerBuilder builder, List<Assembly> assemblies, ScopeOptions options = ScopeOptions.None)
	{
		if (!assemblies.Contains(typeof(SystemScope).Assembly)) assemblies.Add(typeof(SystemScope).Assembly);

		Container.BuildContainer(builder, assemblies.ToArray());

		if (options.IsFlagSet(ScopeOptions.SetLifetimeScope))
		{
			ConsoleLog.WriteMagenta("Setting lifetime scope");

			Container.LifetimeScope = builder.Build();
		}
	}

	public ILifetimeScope? LifetimeScope { get; set; }

	public Assembly[] SystemAssemblies => LoadedAssemblies;

	private SystemScope() { }

	public TItem Resolve<TItem>() where TItem : class => LifetimeScope!.Resolve<TItem>();

	public object Resolve(Type type) => LifetimeScope!.Resolve(type);

	public bool TryResolve(Type type, out object? instance)
	{
		if (LifetimeScope == null)
		{
			instance = null;
			return false;
		}

		return LifetimeScope.TryResolve(type, out instance);
	}

	public bool TryResolve<TItem>(out TItem? instance) where TItem : class
	{
		if (LifetimeScope != null) return LifetimeScope.TryResolve(out instance);

		instance = null;

		return false;
	}

	private void BuildContainer(ContainerBuilder builder, Assembly[] assemblies)
	{
		LoadedAssemblies = assemblies;

		foreach (var assembly in LoadedAssemblies) ConsoleLog.WriteDarkCyan($"   Loading modules for assembly {assembly.FullName}");

		builder.RegisterAssemblyModules(LoadedAssemblies);

		builder.RegisterInstance(this)
				.As<ISystemScope>();

		builder.RegisterAssemblyTypes(LoadedAssemblies)
				.AsImplementedInterfaces()
				.HasPublicConstructor()
				.PublicOnly()
				.PreserveExistingDefaults()
				.Except<ISystemScope>()
				.AsSelf();
	}
}