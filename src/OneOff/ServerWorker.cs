using System.Reflection;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web.Api;
using Newtonsoft.Json;

namespace OneOff;

public class ServerWorker
{
	public void DoWork(ushort webPort)
	{
		var applicationSettings = new ToolkitWebApplicationSettings
								{
									Options = WebApplicationOptions.UseHttps | WebApplicationOptions.UseSignalR | WebApplicationOptions.UseAuthentication,
									TlsCertificate = new CertificationSettings
													{
														Location = @"C:\DevelopmentCert\DevelopmentCert.pfx",
														Password = "basarab_cert"
													},
									TokenCertificate = new CertificationSettings
														{
															Location = @"C:\DevelopmentCert\DevelopmentCert.pfx",
															Password = "basarab_cert"
														},
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

		ToolkitWebApplication.Run(applicationSettings);
	}

	private void Started() => ConsoleLog.WriteGreen("Hey the web application has started!!!!!");
}