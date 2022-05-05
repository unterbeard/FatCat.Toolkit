using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using FatCat.Toolkit.Console;
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
	private readonly string logName;

	private readonly ConcurrentQueue<string> messageQueue = new();
	private readonly IAutoWaitEvent queueEvent;

	private bool active;
	private LogLevel logLevel = LogLevel.Information;

	private Thread writeMessageThread;

	public int MessageQueueCount => messageQueue.Count;

	public SimpleLogger(IApplicationTools applicationTools,
						IAutoWaitEvent queueEvent,
						string? logName = null)
	{
		this.applicationTools = applicationTools;
		this.queueEvent = queueEvent;

		this.logName = logName ?? this.applicationTools.ExecutableName;
		Start();
	}

	public void Dispose()
	{
		active = false;

		WriteAllMessages();

		queueEvent.Trigger();
		queueEvent.Dispose();
	}

	public void SetLogLevel(LogLevel logLevel) => this.logLevel = logLevel;

	public void Write(LogLevel logLevel, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
	{
		var fullMessage = $"{logLevel} | {Path.GetFileName(sourceFilePath)} @ {sourceLineNumber} {memberName} | {message}";

		messageQueue.Enqueue(fullMessage);

		queueEvent.Trigger();
	}

	public void WriteDebug(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) => Write(LogLevel.Debug, message, memberName, sourceFilePath, sourceLineNumber);

	public void WriteError(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) => Write(LogLevel.Error, message, memberName, sourceFilePath, sourceLineNumber);

	public void WriteFatal(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) => Write(LogLevel.Fatal, message, memberName, sourceFilePath, sourceLineNumber);

	public void WriteInformation(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) => Write(LogLevel.Information, message, memberName, sourceFilePath, sourceLineNumber);

	public void WriteVerbose(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) => Write(LogLevel.Verbose, message, memberName, sourceFilePath, sourceLineNumber);

	public void WriteWarning(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) => Write(LogLevel.Warning, message, memberName, sourceFilePath, sourceLineNumber);

	private string? Dequeue() => messageQueue.TryDequeue(out var result) ? result : null;

	private void LogWritingThread()
	{
		while (active)
		{
			queueEvent.Wait();

			WriteAllMessages();
		}
	}

	private void Start()
	{
		ThreadStart threadStart = LogWritingThread;

		writeMessageThread = new Thread(threadStart);

		writeMessageThread.Start();
	}

	private void WriteAllMessages()
	{
		while (MessageQueueCount > 0)
		{
			var nextMessage = Dequeue();

			if (nextMessage == null) continue;

			// TEMP to prove that queuing event works
			ConsoleLog.Write(nextMessage);
		}
	}
}