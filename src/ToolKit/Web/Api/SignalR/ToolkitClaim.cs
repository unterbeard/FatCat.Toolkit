namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitClaim : EqualObject
{
	public string? Issuer { get; set; }

	public string? Type { get; set; }

	public string? Value { get; set; }

	public string? OriginalIssuer { get; set; }
}