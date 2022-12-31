using System.Text;
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
		
		return new()
				{
					IssuerSigningKey = new SymmetricSecurityKey("This is a secret key"u8.ToArray()),
					ValidAudience = "https://foghaze.com/Brume",
					ValidIssuer = "FogHaze",
					ClockSkew = 10.Seconds()
				};
	}
}