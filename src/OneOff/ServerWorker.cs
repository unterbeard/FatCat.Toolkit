using System.Reflection;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.Web.Api;
using FatCat.Toolkit.Web.Api.SignalR;
using Humanizer;
using Newtonsoft.Json;

namespace OneOff;

public class ServerWorker
{
	public void DoWork(ushort webPort)
	{
		var applicationSettings = new ToolkitWebApplicationSettings
								{
									Options = WebApplicationOptions.UseHttps | WebApplicationOptions.UseSignalR,
									CertificationLocation = @"C:\DevelopmentCert\DevelopmentCert.pfx",
									CertificationPassword = "basarab_cert",
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

												// var thread = SystemScope.Container.Resolve<IThread>();
												//
												// thread.Run(async () =>
												// 			{
												// 				await Task.Delay(1.Seconds());
												//
												// 				var hubServer = SystemScope.Container.Resolve<IToolkitHubServer>();
												//
												// 				await hubServer.SendToClientNoResponse(message.ConnectionId, new ToolkitMessage
												// 																			{
												// 																				MessageType = 2,
												// 																				Data = $"Hello World {Faker.RandomString()}"
												// 																			});
												//
												// 				await thread.Sleep(1.5.Seconds());
												//
												// 				ConsoleLog.WriteYellow("Going to send message to client and wait for a response");
												//
												// 				var response = await hubServer.SendToClient(message.ConnectionId, new ToolkitMessage
												// 																				{
												// 																					MessageType = 3,
												// 																					Data = $"Waiting for Response {Faker.RandomString()}"
												// 																				});
												//
												// 				ConsoleLog.WriteGreen($"Response: {JsonConvert.SerializeObject(response)}");
												// 			});
												//
												// var result = $"MessageBack {DateTime.Now:h:mm:ss tt zzs} | {Faker.RandomString()}";
												//
												// ConsoleLog.WriteDarkCyan($"Sending back Response: {result}");
												//
												// return Task.FromResult(result);
											};

		ToolkitWebApplication.Run(applicationSettings);
	}

	private void Started()
	{
		ConsoleLog.WriteGreen("Hey the web application has started!!!!!");

		var thread = SystemScope.Container.Resolve<IThread>();
		var hubServer = SystemScope.Container.Resolve<IToolkitHubServer>();

		thread.Run(async () =>
					{
						while (true)
						{
							var connections = hubServer.GetConnections();

							foreach (var connection in connections)
							{
								ConsoleLog.WriteCyan("Going to send data buffer to the client from the server");

								var dataBuffer = Faker.Create<List<byte>>(1024).ToArray();

								var clientResponse = await hubServer.SendDataBufferToClient(connection,
																							new ToolkitMessage
																							{
																								ConnectionId = connection,
																								MessageType = 1344,
																							},
																							dataBuffer);

								ConsoleLog.WriteGreen($"Client Response: {clientResponse.Data}");

								await Task.Delay(15.Seconds());
							}
						}
					});
	}
}