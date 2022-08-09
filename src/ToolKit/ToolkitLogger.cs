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
	public void Debug(string message) { }

	public void Error(string message) { }

	public void Exception(Exception ex) { }

	public void Information(string message) { }

	public void Warning(string message) { }
}