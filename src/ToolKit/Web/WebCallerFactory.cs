using FatCat.Toolkit.Logging;

namespace FatCat.Toolkit.Web;

public interface IWebCallerFactory
{
	IWebCaller GetWebCaller(Uri baseUri);
}

public class WebCallerFactory : IWebCallerFactory
{
	private readonly IToolkitLogger toolkitLogger;

	public WebCallerFactory(IToolkitLogger toolkitLogger) => this.toolkitLogger = toolkitLogger;

	public IWebCaller GetWebCaller(Uri baseUri) => new WebCaller(baseUri, toolkitLogger);
}
