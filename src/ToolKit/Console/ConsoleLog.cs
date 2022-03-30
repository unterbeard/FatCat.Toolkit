using System.Runtime.CompilerServices;

namespace FatCat.Toolkit.Console;

public static class ConsoleLog
{
	private static readonly object lockObj = new();

	public static bool LogCallerInformation { get; set; } = true;

	public static void Write(string message,
							[CallerMemberName] string memberName = "",
							[CallerFilePath] string sourceFilePath = "",
							[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.Gray, message, memberName, sourceFilePath, sourceLineNumber);

	public static void Write(ConsoleColor color, string message,
							[CallerMemberName] string memberName = "",
							[CallerFilePath] string sourceFilePath = "",
							[CallerLineNumber] int sourceLineNumber = 0)
	{
		var messageToLog = $"{Path.GetFileName(sourceFilePath)} @ {sourceLineNumber} {memberName} | {message}";

		WriteLineWithColor(color, LogCallerInformation ? messageToLog : message);
	}

	public static void WriteBlue(string message,
								[CallerMemberName] string memberName = "",
								[CallerFilePath] string sourceFilePath = "",
								[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.Blue, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteCyan(string message,
								[CallerMemberName] string memberName = "",
								[CallerFilePath] string sourceFilePath = "",
								[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.Cyan, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteDarkBlue(string message,
									[CallerMemberName] string memberName = "",
									[CallerFilePath] string sourceFilePath = "",
									[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.DarkBlue, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteDarkCyan(string message,
									[CallerMemberName] string memberName = "",
									[CallerFilePath] string sourceFilePath = "",
									[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.DarkCyan, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteDarkGray(string message,
									[CallerMemberName] string memberName = "",
									[CallerFilePath] string sourceFilePath = "",
									[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.DarkGray, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteDarkGreen(string message,
									[CallerMemberName] string memberName = "",
									[CallerFilePath] string sourceFilePath = "",
									[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.DarkGreen, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteDarkMagenta(string message,
										[CallerMemberName] string memberName = "",
										[CallerFilePath] string sourceFilePath = "",
										[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.DarkMagenta, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteDarkRed(string message,
									[CallerMemberName] string memberName = "",
									[CallerFilePath] string sourceFilePath = "",
									[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.DarkRed, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteDarkYellow(string message,
										[CallerMemberName] string memberName = "",
										[CallerFilePath] string sourceFilePath = "",
										[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.DarkYellow, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteGray(string message,
								[CallerMemberName] string memberName = "",
								[CallerFilePath] string sourceFilePath = "",
								[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.Gray, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteGreen(string message,
								[CallerMemberName] string memberName = "",
								[CallerFilePath] string sourceFilePath = "",
								[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.Green, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteMagenta(string message,
									[CallerMemberName] string memberName = "",
									[CallerFilePath] string sourceFilePath = "",
									[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.Magenta, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteRed(string message,
								[CallerMemberName] string memberName = "",
								[CallerFilePath] string sourceFilePath = "",
								[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.Red, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteWhite(string message,
								[CallerMemberName] string memberName = "",
								[CallerFilePath] string sourceFilePath = "",
								[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.White, message, memberName, sourceFilePath, sourceLineNumber);

	public static void WriteYellow(string message,
									[CallerMemberName] string memberName = "",
									[CallerFilePath] string sourceFilePath = "",
									[CallerLineNumber] int sourceLineNumber = 0) => Write(ConsoleColor.Yellow, message, memberName, sourceFilePath, sourceLineNumber);

	private static void WriteLineWithColor(ConsoleColor color, string message)
	{
		lock (lockObj)
		{
			var oldColor = System.Console.ForegroundColor;

			System.Console.ForegroundColor = color;

			System.Console.WriteLine(message);

			System.Console.ForegroundColor = oldColor;
		}
	}
}