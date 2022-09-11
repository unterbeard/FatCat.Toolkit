using System.IO.Abstractions;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Tools;

ConsoleLog.LogCallerInformation = true;

try
{
	var somePathToCreate = @"D:\Temp\ANewDirectory\AnotherBites\ThenThisOne\AndAgain\SomeFile.txt";

	var fileTools = new FileSystemTools(new FileSystem());

	fileTools.EnsureFile(somePathToCreate);
}
catch (Exception ex) { ConsoleLog.WriteException(ex); }