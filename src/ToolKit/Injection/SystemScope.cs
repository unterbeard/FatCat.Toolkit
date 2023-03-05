#nullable enable
using System.Reflection;
using Autofac;
using FatCat.Toolkit.Console;

#pragma warning disable CS8767

namespace FatCat.Toolkit.Injection;

public interface ISystemScope
{
	List<Assembly> SystemAssemblies { get; }

	TItem Resolve<TItem>() where TItem : class;

	object Resolve(Type type);

	bool TryResolve(Type type, out object instance);

	bool TryResolve<TItem>(out TItem instance) where TItem : class;
}

public class SystemScope : ISystemScope
{
	private static readonly List<Assembly> defaultAssemblies = new ReflectionTools().GetDomainAssemblies();

	private static readonly Lazy<SystemScope> instance = new(() => new SystemScope());

	public static SystemScope Container => instance.Value;

	public static List<Assembly> ContainerAssemblies { get; set; } = defaultAssemblies;

	public static void Initialize(ContainerBuilder builder, ScopeOptions options = ScopeOptions.None) => Initialize(builder, defaultAssemblies.ToList(), options);

	public static void Initialize(ContainerBuilder builder, List<Assembly> assemblies, ScopeOptions options = ScopeOptions.None)
	{
		if (!assemblies.Contains(typeof(SystemScope).Assembly)) assemblies.Insert(0, typeof(SystemScope).Assembly);

		Container.BuildContainer(builder, assemblies);

		if (options.IsFlagSet(ScopeOptions.SetLifetimeScope))
		{
			ConsoleLog.WriteMagenta("Setting lifetime scope");

			Container.LifetimeScope = builder.Build();
		}
	}

	public ILifetimeScope? LifetimeScope { get; set; }

	public List<Assembly> SystemAssemblies => ContainerAssemblies;

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

	private void BuildContainer(ContainerBuilder builder, List<Assembly> assemblies)
	{
		ContainerAssemblies = assemblies;

		foreach (var assembly in ContainerAssemblies) ConsoleLog.WriteDarkCyan($"   Loading modules for assembly {assembly.FullName}");

		builder.RegisterAssemblyModules(ContainerAssemblies.ToArray());

		builder.RegisterInstance(this)
				.As<ISystemScope>();

		builder.RegisterAssemblyTypes(ContainerAssemblies.ToArray())
				.AsImplementedInterfaces()
				.HasPublicConstructor()
				.PublicOnly()
				.PreserveExistingDefaults()
				.Except<ISystemScope>()
				.AsSelf();
	}
}