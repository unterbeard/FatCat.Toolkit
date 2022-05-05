using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using FatCat.Toolkit.Events;

namespace FatCat.Toolkit;

/// <summary>
///  Will write a {ApplicationName}.log file to executing directory to be used as a quick and easy way
///  to log messages to the file system without a lot of setup
/// </summary>
public interface ISimpleLogger : IDisposable
{
	void SetLogLevel(LogLevel logLevel);

	void Write(LogLevel logLevel, string message, [CallerMemberName] string memberName = "",
				[CallerFilePath] string sourceFilePath = "",
				[CallerLineNumber] int sourceLineNumber = 0);

	void WriteDebug(string message, [CallerMemberName] string memberName = "",
					[CallerFilePath] string sourceFilePath = "",
					[CallerLineNumber] int sourceLineNumber = 0);

	void WriteError(string message, [CallerMemberName] string memberName = "",
					[CallerFilePath] string sourceFilePath = "",
					[CallerLineNumber] int sourceLineNumber = 0);

	void WriteFatal(string message, [CallerMemberName] string memberName = "",
					[CallerFilePath] string sourceFilePath = "",
					[CallerLineNumber] int sourceLineNumber = 0);

	void WriteInformation(string message, [CallerMemberName] string memberName = "",
						[CallerFilePath] string sourceFilePath = "",
						[CallerLineNumber] int sourceLineNumber = 0);

	void WriteVerbose(string message, [CallerMemberName] string memberName = "",
					[CallerFilePath] string sourceFilePath = "",
					[CallerLineNumber] int sourceLineNumber = 0);

	void WriteWarning(string message, [CallerMemberName] string memberName = "",
					[CallerFilePath] string sourceFilePath = "",
					[CallerLineNumber] int sourceLineNumber = 0);
}

public class SimpleLogger : ISimpleLogger
{
	private readonly IApplicationTools applicationTools;
	private readonly IAutoWaitEvent autoWaitEvent;
	private readonly string logName;

	private readonly ConcurrentQueue<string> messageQueue = new();
	private TaskFactory taskFactory;

	private Thread writeMessageThread;

	public CancellationToken CanelToken { get; }

	public int MessageQueueCount => messageQueue.Count;

	public SimpleLogger(IApplicationTools applicationTools,
						IAutoWaitEvent autoWaitEvent,
						string? logName = null)
	{
		this.applicationTools = applicationTools;
		this.autoWaitEvent = autoWaitEvent;

		this.logName = logName ?? this.applicationTools.ExecutableName;

		/*
		 *  - Take Log Messages into Queue
		 *	- On Separate thread write to log file
		 */
	}

	public void Dispose() { autoWaitEvent.Dispose(); }

	public void SetLogLevel(LogLevel logLevel) { throw new NotImplementedException(); }

	public void Write(LogLevel logLevel, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { throw new NotImplementedException(); }

	public void WriteDebug(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { throw new NotImplementedException(); }

	public void WriteError(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { throw new NotImplementedException(); }

	public void WriteFatal(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { throw new NotImplementedException(); }

	public void WriteInformation(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { throw new NotImplementedException(); }

	public void WriteVerbose(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { throw new NotImplementedException(); }

	public void WriteWarning(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { throw new NotImplementedException(); }
}