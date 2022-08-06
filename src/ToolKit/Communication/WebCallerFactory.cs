namespace FatCat.Toolkit.Communication;

public interface IWebCallerFactory
{
	IWebCaller GetWebCaller(Uri baseUri);
}

public class WebCallerFactory : IWebCallerFactory
{
	public IWebCaller GetWebCaller(Uri baseUri) => new WebCaller(baseUri);
}