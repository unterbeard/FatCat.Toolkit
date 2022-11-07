using System.Reflection;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web.Api;

namespace OneOff;

public static class Program
{
	public static async Task Main(params string[] args)
	{
		ConsoleLog.LogCallerInformation = true;

		try
		{
			var applicationSettings = new ApplicationSettings
									{
										Options = WebApplicationOptions.UseHttps,
										CertificationLocation = @"C:\DevelopmentCert\DevelopmentCert.pfx",
										CertificationPassword = "basarab_cert",
										Port = 14555,
										ContainerAssemblies = new List<Assembly> { Assembly.GetExecutingAssembly() },
										CorsUri = new List<Uri> { new("https://localhost:14555") }
									};

			WebApplication.Run(applicationSettings);
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}
}