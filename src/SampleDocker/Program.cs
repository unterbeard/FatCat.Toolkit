using System.Reflection;
using Autofac.AspNetCore.Extensions;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web.Api;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace SampleDocker;

public static class Program
{
	public static void Main(params string[] args)
	{
		var applicationSettings = new ToolkitWebApplicationSettings
								{
									Options = WebApplicationOptions.UseSignalR,
									Port = 5000,
									ContainerAssemblies = new List<Assembly> { Assembly.GetExecutingAssembly() },
									CorsUri = new List<Uri> { new($"https://localhost:{5000}") },
								};

		applicationSettings.ClientDataBufferMessage += async (message, buffer) =>
														{
															ConsoleLog.WriteMagenta($"Got data buffer message: {JsonConvert.SerializeObject(message)}");
															ConsoleLog.WriteMagenta($"Data buffer length: {buffer.Length}");

															await Task.CompletedTask;

															var responseMessage = $"BufferResponse {Faker.RandomString()}";

															ConsoleLog.WriteGreen($"Client Response for data buffer: <{responseMessage}>");

															return responseMessage;
														};

		applicationSettings.ClientMessage += async message =>
											{
												await Task.CompletedTask;

												ConsoleLog.WriteDarkCyan($"MessageId <{message.MessageType}> | Data <{message.Data}> | ConnectionId <{message.ConnectionId}>");

												return "ACK";
											};

		// applicationSettings.ClientConnected += OnClientConnected;
		// applicationSettings.ClientDisconnected += OnClientDisconnected;

		ToolkitWebApplication.Run(applicationSettings);
	}
}