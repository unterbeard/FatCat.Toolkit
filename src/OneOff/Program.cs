using System.IO.Abstractions;
using FatCat.Toolkit;
using FatCat.Toolkit.Console;

ConsoleLog.LogCallerInformation = true;

try
{
	var somePathToCreate = @"D:\Temp\ANewDirectory\AnotherBites\ThenThisOne\AndAgain\SomeFile.txt";

	var fileTools = new FileSystemTools(new FileSystem());

	await fileTools.WriteAllText(somePathToCreate, "Go for it ");
}
catch (Exception ex) { ConsoleLog.WriteException(ex); }