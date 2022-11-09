namespace FatCat.Toolkit.Web.Api.SignalR;

public class ToolkitMessage
{
	public string ConnectionId { get; set; } = null!;

	public string? Data { get; set; }

	public int MessageId { get; set; }
}