using FatCat.Toolkit.Console;
using FatCat.Toolkit.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace FatCat.Toolkit.Web.Api;

internal static class OAuthExtensions
{
	public static void AddPermissionsPolicies(this AuthorizationOptions options)
	{
		options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
		options.AddPolicy("User", policy => policy.RequireClaim("User"));
	}

	public static JwtBearerEvents GetTokenBearerEvents()
	{
		return new JwtBearerEvents
				{
					OnTokenValidated = _ => Task.CompletedTask,
					OnMessageReceived = _ => Task.CompletedTask,
					OnForbidden = _ => Task.CompletedTask,
					OnAuthenticationFailed = context =>
											{
												if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
												{
													context
														.Response
														.Headers
														.Add("Token-Expired", "true");
												}
												else ConsoleLog.WriteException(context.Exception);

												return Task.CompletedTask;
											},
					OnChallenge = context =>
								{
									var hasError = context.Error.IsNotNullOrEmpty() || context.ErrorDescription.IsNotNullOrEmpty();

									if (!hasError) return Task.CompletedTask;

									ConsoleLog.WriteRed("Error: {Error}", context.Error);
									ConsoleLog.WriteRed("ErrorDescription: {ErrorDescription}", context.ErrorDescription);

									return Task.CompletedTask;
								}
				};
	}
}