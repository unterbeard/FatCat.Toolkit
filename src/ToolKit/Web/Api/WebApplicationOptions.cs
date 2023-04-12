namespace FatCat.Toolkit.Web.Api;

[Flags]
public enum WebApplicationOptions
{
	None = 0,
	Https = 1,
	FileSystem = 2,
	SignalR = 4,
	Authentication = 8,
	HttpsRedirection = 16,
	Cors = 32,
	CommonOptions = Cors | HttpsRedirection | Https
}