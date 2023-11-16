using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;

namespace FatCat.Toolkit.Web;

public interface IWebCallerFactory
{
	IWebCaller GetWebCaller(Uri baseUri);
}

public class WebCallerFactory : IWebCallerFactory
{
	private readonly IJsonOperations jsonOperations;
	private readonly IToolkitLogger toolkitLogger;

	public WebCallerFactory(IToolkitLogger toolkitLogger, IJsonOperations jsonOperations)
	{
		this.toolkitLogger = toolkitLogger;
		this.jsonOperations = jsonOperations;
	}

	public IWebCaller GetWebCaller(Uri baseUri) => new WebCaller(baseUri, jsonOperations, toolkitLogger);
}