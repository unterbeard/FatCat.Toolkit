using FatCat.Toolkit;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;

ConsoleLog.LogCallerInformation = false;

using var simpleLogger = new SimpleLogger(new ApplicationTools(),
									new AutoWaitEvent());

ConsoleLog.Write("Before write");

simpleLogger.WriteInformation("This is my first test");

ConsoleLog.Write("After Write | Exiting Application");

