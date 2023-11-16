#nullable enable
namespace FatCat.Toolkit.Data;

public interface IEnvironmentRepository
{
	string? Get(string name);

	string? Get(string name, EnvironmentVariableTarget target);

	void Set(string name, string? value);

	void Set(string name, string? value, EnvironmentVariableTarget target);
}

public class EnvironmentRepository : IEnvironmentRepository
{
	public string? Get(string name) => Environment.GetEnvironmentVariable(name);

	public string? Get(string name, EnvironmentVariableTarget target) => Environment.GetEnvironmentVariable(name, target);

	public void Set(string name, string? value) { Environment.SetEnvironmentVariable(name, value); }

	public void Set(string name, string? value, EnvironmentVariableTarget target) { Environment.SetEnvironmentVariable(name, value, target); }
}