using FatCat.Toolkit;
using FatCat.Toolkit.Communication;
using FatCat.Toolkit.Console;

ConsoleLog.LogCallerInformation = true;

var webCaller = new WebCaller(new Uri("https://localhost:14555/api"), new ToolkitLogger());

try
{
	var result = await webCaller.Get("Fog");

	ConsoleLog.WriteCyan($"Result StatusCode | <{result.StatusCode}> | Content <{result.Content}>");
}
catch (Exception ex)
{
	ConsoleLog.WriteMagenta($"DUDE EXCEPTION <{ex.GetType().FullName}>");
	
	ConsoleLog.WriteException(ex);
}