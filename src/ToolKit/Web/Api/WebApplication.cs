namespace FatCat.Toolkit.Web.Api;

public static class WebApplication
{
	public static ApplicationSettings Settings { get; private set; } = null!;
	
	public static void Run(ApplicationSettings settings)
	{
		Settings = settings;
	}
}