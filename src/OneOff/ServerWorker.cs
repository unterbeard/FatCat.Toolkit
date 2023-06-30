using System.Reflection;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Web;
using FatCat.Toolkit.Web.Api;
using FatCat.Toolkit.Web.Api.SignalR;
using Newtonsoft.Json;

namespace OneOff;

public class ServerWorker
{
	public void DoWork(string[] args)
	{
		var applicationSettings = new ToolkitWebApplicationSettings
								{
									Options = WebApplicationOptions.Https | WebApplicationOptions.SignalR,
									TlsCertificate = new CertificationSettings
													{
														Location = @"C:\DevelopmentCert\DevelopmentCert.pfx",
														Password = "basarab_cert"
													},
									SignalRPath = "events",
									ToolkitTokenParameters = new SpikeToolkitParameters(),
									ContainerAssemblies = new List<Assembly> { Assembly.GetExecutingAssembly() },
									OnWebApplicationStarted = Started,
									Args = args
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

	private void Started()
	{
		ConsoleLog.WriteGreen("Hey the web application has started!!!!!");

		var factory = SystemScope.Container.Resolve<IWebCallerFactory>();

		var caller = factory.GetWebCaller(new Uri("http://localhost:5000"));

		var response = caller.Get("api/test").Result;

		// var finalResult = new WebResult<TestModel>(response);
		var finalResult = response;

		if (finalResult.IsSuccessful) ConsoleLog.WriteGreen(finalResult.Content);
		else ConsoleLog.WriteRed(finalResult.Content);
	}
}