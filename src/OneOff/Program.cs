using FatCat.Toolkit;
using FatCat.Toolkit.Console;

ConsoleLog.LogCallerInformation = false;

var simpleLogger = new SimpleLogger(new ApplicationTools());

ConsoleLog.Write("Before write");

simpleLogger.WriteInformation("This is my first test");

ConsoleLog.Write("After Write | Exiting Application");

await Task.Delay(TimeSpan.FromMilliseconds(250));