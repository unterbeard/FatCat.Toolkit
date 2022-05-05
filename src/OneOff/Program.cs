using FatCat.Toolkit;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;

var simpleLogger = new SimpleLogger(new ApplicationTools(),
									new AutoWaitEvent());

ConsoleLog.Write("Before write");

simpleLogger.WriteInformation("This is my first test");

ConsoleLog.Write("After Write");

await Task.Delay(TimeSpan.FromMilliseconds(250));