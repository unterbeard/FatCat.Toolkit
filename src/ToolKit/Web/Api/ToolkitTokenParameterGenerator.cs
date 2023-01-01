using System.Security.Cryptography.X509Certificates;
using FatCat.Toolkit.Console;
using Humanizer;
using Microsoft.IdentityModel.Tokens;

namespace FatCat.Toolkit.Web.Api;

public interface IToolkitTokenParameters
{
	TokenValidationParameters Get();
}

public class ToolkitTokenParameters : IToolkitTokenParameters
{
	public TokenValidationParameters Get()
	{
		ConsoleLog.WriteCyan("Getting token parameters");

		var cert = new X509Certificate2(@"C:\DevelopmentCert\DevelopmentCert.pfx", "basarab_cert");

		return new TokenValidationParameters
				{
					IssuerSigningKey = new X509SecurityKey(cert),
					ValidAudience = "https://foghaze.com/Brume",
					ValidIssuer = "FogHaze",
					ClockSkew = 10.Seconds()
				};
	}
}