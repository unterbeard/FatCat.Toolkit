// See https://aka.ms/new-console-template for more information

using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;

ConsoleLog.LogCallerInformation = false;

ConsoleLog.WriteBlue("Hello WORLD!!!!!!");

var consoleUtilities = new ConsoleUtilities(new WaitEvent());

consoleUtilities.WaitForExit();

ConsoleLog.WriteMagenta("This is after the wait for exit");