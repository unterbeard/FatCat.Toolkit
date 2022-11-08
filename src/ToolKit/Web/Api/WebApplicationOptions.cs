namespace FatCat.Toolkit.Web.Api;

[Flags]
public enum WebApplicationOptions
{
	None = 0,
	UseHttps = 1,
	UseFileSystem = 2,
	UseSignalR = 4
}