using Microsoft.IdentityModel.Tokens;

namespace FatCat.Toolkit.Web.Api;

/// <summary>
///  Used to get TokenParameters for the Api
/// </summary>
public interface IToolkitTokenParameters
{
	TokenValidationParameters Get();
}
