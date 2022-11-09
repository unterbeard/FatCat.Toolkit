#nullable enable
using FatCat.Toolkit.Web.Api.SignalR;

namespace FatCat.Toolkit.Web.Api;

public delegate Task<string?> HubMessage(ToolkitMessage message);