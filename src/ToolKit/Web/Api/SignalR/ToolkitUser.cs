namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitUser : EqualObject
{
	public string AuthenticationType { get; set; }

	public List<ToolkitClaim> Claims { get; set; } = new();

	public bool IsAuthenticated { get; set; }

	public string Name { get; set; }
}