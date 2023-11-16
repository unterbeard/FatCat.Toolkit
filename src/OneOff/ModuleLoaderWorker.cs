using FatCat.Toolkit;
using FatCat.Toolkit.Console;

namespace OneOff;

public class ModuleLoaderWorker : SpikeWorker
{
	private readonly IApplicationTools applicationTools;

	public ModuleLoaderWorker(IApplicationTools applicationTools) => this.applicationTools = applicationTools;

	public override async Task DoWork()
	{
		await Task.CompletedTask;

		ConsoleLog.WriteCyan($"ApplicationTools.Type := <{applicationTools.GetType().FullName}>");
		ConsoleLog.WriteMagenta($"ExecutableName := <{applicationTools.ExecutableName}>");
	}
}