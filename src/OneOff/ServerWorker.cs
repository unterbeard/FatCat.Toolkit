using System.Reflection;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web.Api;
using FatCat.Toolkit.Web.Api.SignalR;
using Newtonsoft.Json;

namespace OneOff;

public class ServerWorker
{
	public void DoWork(ushort webPort)
	{
		var applicationSettings = new ToolkitWebApplicationSettings
								{
									Options = WebApplicationOptions.UseHttps | WebApplicationOptions.UseSignalR,
									TlsCertificate = new CertificationSettings
													{
														Location = @"C:\DevelopmentCert\DevelopmentCert.pfx",
														Password = "basarab_cert"
													},
									ToolkitTokenParameters = new SpikeToolkitParameters(),
									Port = webPort,
									ContainerAssemblies = new List<Assembly> { Assembly.GetExecutingAssembly() },
									CorsUri = new List<Uri> { new($"https://localhost:{webPort}") },
									OnWebApplicationStarted = Started,
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

		applicationSettings.ClientConnected += OnClientConnected;
		applicationSettings.ClientDisconnected += OnClientDisconnected;

		ToolkitWebApplication.Run(applicationSettings);
	}

	private Task OnClientConnected(ToolkitUser user, string connectionId)
	{
		ConsoleLog.WriteDarkCyan($"A client has connected: <{user.Name}> | <{connectionId}>");

		return Task.CompletedTask;
	}

	private Task OnClientDisconnected(ToolkitUser user, string connectionId)
	{
		ConsoleLog.WriteDarkYellow($"A client has disconnected: <{user.Name}> | <{connectionId}>");

		return Task.CompletedTask;
	}

	private void Started() => ConsoleLog.WriteGreen("Hey the web application has started!!!!!");
}