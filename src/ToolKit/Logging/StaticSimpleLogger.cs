using System.Runtime.CompilerServices;

namespace FatCat.Toolkit.Logging;

public static class StaticSimpleLogger
{
	private static readonly SimpleLogger simpleLogger = new(new ApplicationTools());

	public static void SetLogLevel(LogLevel logLevel) { simpleLogger.SetLogLevel(logLevel); }

	public static void Write(
		LogLevel logLevel,
		string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0
	)
	{
		simpleLogger.Write(logLevel, message, memberName, sourceFilePath, sourceLineNumber);
	}

	public static void WriteDebug(
		string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0
	)
	{
		simpleLogger.WriteDebug(message, memberName, sourceFilePath, sourceLineNumber);
	}

	public static void WriteError(
		string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0
	)
	{
		simpleLogger.WriteError(message, memberName, sourceFilePath, sourceLineNumber);
	}

	public static void WriteFatal(
		string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0
	)
	{
		simpleLogger.WriteFatal(message, memberName, sourceFilePath, sourceLineNumber);
	}

	public static void WriteInformation(
		string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0
	)
	{
		simpleLogger.WriteInformation(message, memberName, sourceFilePath, sourceLineNumber);
	}

	public static void WriteVerbose(
		string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0
	)
	{
		simpleLogger.WriteVerbose(message, memberName, sourceFilePath, sourceLineNumber);
	}

	public static void WriteWarning(
		string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0
	)
	{
		simpleLogger.WriteWarning(message, memberName, sourceFilePath, sourceLineNumber);
	}
}