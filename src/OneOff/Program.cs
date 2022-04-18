using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;

// ConsoleLog.LogCallerInformation = false;

ConsoleLog.WriteBlue("Hello WORLD!!!!!!");

var consoleUtilities = new ConsoleUtilities(new WaitEvent());

try { new WillDoBadStuff().DoBadStuff(); }
catch (Exception e) { ConsoleLog.WriteException(e); }

consoleUtilities.WaitForExit();

ConsoleLog.WriteMagenta("This is after the wait for exit");

public class WillDoBadStuff
{
	public void DoBadStuff() { ActuallyThrow(); }

	private void ActuallyThrow() => throw new Exception("I did something really bad like Bart Simpson");
}