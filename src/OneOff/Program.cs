using FatCat.Toolkit;
using FatCat.Toolkit.Console;

ConsoleLog.LogCallerInformation = false;

ConsoleLog.Write("Going to test Registry Repository stuff");

var registryRepository = new RegistryRepository();

registryRepository.Set("OneOff", "Dude", $"Perfect | {DateTime.Now}");

var value = registryRepository.Get("OneOff", "Dude");

ConsoleLog.WriteCyan($"Found from the Registry <{value}>");