﻿namespace FatCat.Toolkit.Console;

public interface IConsoleAccess
{
	void WriteLineWithColor(ConsoleColor color, string message);
}

public class ConsoleAccess : IConsoleAccess
{
	private readonly object lockObject = new();

	public void WriteLineWithColor(ConsoleColor color, string message)
	{
		lock (lockObject)
		{
			var oldColor = System.Console.ForegroundColor;

			System.Console.ForegroundColor = color;

			System.Console.WriteLine(message);

			System.Console.ForegroundColor = oldColor;
		}
	}
}