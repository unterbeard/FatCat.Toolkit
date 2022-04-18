using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;

// ConsoleLog.LogCallerInformation = false;

ConsoleLog.WriteBlue("Hello WORLD!!!!!!");

var manualWaitEvent = new ManualWaitEvent();

var consoleUtilities = new ConsoleUtilities(manualWaitEvent);

try { new WillDoBadStuff().DoBadStuff(); }
catch (Exception e) { ConsoleLog.WriteException(e); }

ConsoleLog.WriteMagenta($"ManualWaitEvent.Triggered => <{manualWaitEvent.HasBeenTriggered}>");

manualWaitEvent.Trigger();

ConsoleLog.WriteCyan($"ManualWaitEvent.Triggered => <{manualWaitEvent.HasBeenTriggered}>");

consoleUtilities.WaitForExit();

ConsoleLog.WriteMagenta("This is after the wait for exit");

public class WillDoBadStuff
{
	public void DoBadStuff() { ActuallyThrow(); }

	private void ActuallyThrow() => throw new Exception("I did something really bad like Bart Simpson");
}