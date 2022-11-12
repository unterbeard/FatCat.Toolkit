#nullable enable
using FatCat.Toolkit.Web.Api.SignalR;

namespace FatCat.Toolkit.Web.Api;

public delegate Task<string?> ToolkitHubMessage(ToolkitMessage message);

public delegate Task<string?> ToolkitHubDataBufferMessage(ToolkitMessage message, byte[] dataBuffer);