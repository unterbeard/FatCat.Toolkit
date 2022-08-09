using FatCat.Toolkit.Console;

namespace FatCat.Toolkit;

public interface IToolkitLogger
{
	void Debug(string message);

	void Error(string message);

	void Exception(Exception ex);

	void Information(string message);

	void Warning(string message);
}

public class ToolkitLogger : IToolkitLogger
{
	public void Debug(string message) => ConsoleLog.WriteGray(message);

	public void Error(string message) => ConsoleLog.WriteRed(message);

	public void Exception(Exception ex) => ConsoleLog.WriteException(ex);

	public void Information(string message) => ConsoleLog.WriteGreen(message);

	public void Warning(string message) => ConsoleLog.WriteYellow(message);
}