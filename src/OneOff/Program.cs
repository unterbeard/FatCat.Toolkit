// See https://aka.ms/new-console-template for more information

using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;

Console.WriteLine("Hello, World!");

var consoleUtilities = new ConsoleUtilities(new WaitEvent());

consoleUtilities.WaitForExit();

Console.WriteLine("Yo yo after exit");