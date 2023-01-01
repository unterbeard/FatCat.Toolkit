using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web;
using FatCat.Toolkit.Web.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace OneOff;

[AllowAnonymous]
public class GetSampleToken : Endpoint
{
	private readonly JwtSecurityTokenHandler securityTokenHandler = new();

	[HttpGet("api/Sample/Token")]
	public WebResult GetToken()
	{
		var userClaims = new[] { new Claim(ClaimTypes.Name, "John Doe"), };

		var identity = new ClaimsIdentity(userClaims);

		var securityToken = GetSecurityTokenDescriptorCommon(identity);

		var token = securityTokenHandler.CreateToken(securityToken);

		var tokenString = securityTokenHandler.WriteToken(token);

		ConsoleLog.WriteCyan($"TokenString <{tokenString}>");

		return Ok(tokenString);
	}

	private SecurityTokenDescriptor GetSecurityTokenDescriptorCommon(ClaimsIdentity user)
	{
		var signingCredentials = ToolkitWebApplication.Settings.TokenCertificate.GetCertificate() ?? throw new NullReferenceException(nameof(ToolkitWebApplication.Settings.TokenCertificate));

		return new SecurityTokenDescriptor
				{
					Subject = user,
					Expires = DateTime.UtcNow.AddMinutes(15),
					Audience = "https://foghaze.com/Brume",
					Issuer = "FogHaze",
					NotBefore = DateTime.UtcNow.AddSeconds(-10),
					SigningCredentials = new X509SigningCredentials(signingCredentials)
				};
	}
}