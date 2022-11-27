using System.Diagnostics;
using System.Reflection;
using Autofac;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.Web.Api;
using FatCat.Toolkit.Web.Api.SignalR;
using Humanizer;
using Newtonsoft.Json;

namespace OneOff;

public static class Program
{
	private const int WebPort = 14555;

	public static void Main(params string[] args)
	{
		ConsoleLog.LogCallerInformation = true;

		try
		{
			var isClient = args.Any() && args.Any(i => i.Equals("client", StringComparison.OrdinalIgnoreCase));

			if (isClient) ConnectClient();
			else RunServer();
		}
		catch (Exception ex) { ConsoleLog.WriteException(ex); }
	}

	private static void ConnectClient()
	{
		SystemScope.Initialize(new ContainerBuilder(), ScopeOptions.SetLifetimeScope);

		var consoleUtilities = SystemScope.Container.Resolve<IConsoleUtilities>();

		var thread = SystemScope.Container.Resolve<IThread>();
		var hubFactory = SystemScope.Container.Resolve<IToolkitHubClientFactory>();

		thread.Run(async () =>
					{
						var hubUrl = $"https://localhost:{WebPort}/api/events";

						var result = await hubFactory.TryToConnectToClient(hubUrl);

						if (!result.Connected)
						{
							ConsoleLog.WriteRed($"Could not connect too <{hubUrl}>");

							return;
						}

						var hubConnection = result.Connection;

						hubConnection.ServerMessage += OnServerMessage;
						hubConnection.ServerDataBufferMessage += OnDataBufferFromServer;

						await thread.Sleep(1.Seconds());

						ConsoleLog.WriteDarkGreen($"Done connecting to hub at {hubUrl}");

						var dataBuffer = Faker.Create<List<byte>>(1024).ToArray();

						ConsoleLog.WriteCyan($"Going to send data message of length {dataBuffer.Length}");

						var watch = Stopwatch.StartNew();

						var response = await hubConnection.SendDataBuffer(new ToolkitMessage
																		{
																			Data = "Junk",
																			MessageType = 123
																		}, dataBuffer);

						watch.Stop();

						ConsoleLog.WriteYellow($"Send DataBuffer Message Took <{watch.Elapsed}>");
						ConsoleLog.WriteCyan($"Response from server was {JsonConvert.SerializeObject(response, Formatting.Indented)}");

						// await hubConnection.SendNoResponse(new ToolkitMessage
						// 									{
						// 										MessageId = 5,
						// 										Data = $"Some Data {Faker.RandomString()}"
						// 									});

						// thread.Run(async () =>
						// 			{
						// 				await thread.Sleep(3.Seconds());
						//
						// 				var secondConnection = await hubFactory.ConnectToClient(hubUrl);
						//
						// 				await secondConnection.SendNoResponse(new ToolkitMessage
						// 													{
						// 														MessageId = 2,
						// 														Data = $"Hello World {Faker.RandomString()}"
						// 													});
						//
						// 				await thread.Sleep(3.Seconds());
						//
						// 				ConsoleLog.WriteCyan("Going to send a message and wait for a response");
						//
						// 				var watch = Stopwatch.StartNew();
						//
						// 				var response = await secondConnection.Send(new ToolkitMessage
						// 															{
						// 																MessageId = 2 * 1000,
						// 																Data = $"Go More {Faker.RandomString()}"
						// 															});
						//
						// 				watch.Stop();
						//
						// 				ConsoleLog.WriteMagenta($"Response: {JsonConvert.SerializeObject(response)}");
						// 				ConsoleLog.WriteGreen($"Took <{watch.Elapsed}>");
						// 			});
					});

		consoleUtilities.WaitForExit();
	}

	private static async Task<string> OnDataBufferFromServer(ToolkitMessage message, byte[] dataBuffer)
	{
		await Task.CompletedTask;

		ConsoleLog.WriteYellow($"Client Received DataBuffer Message of length {dataBuffer.Length}");

		var responseMessage = Faker.RandomString("ClientDataBufferResponse_");

		ConsoleLog.WriteMagenta($"Client DataBufferResponse : <{responseMessage}>");

		return responseMessage;
	}

	private static async Task<string> OnServerMessage(ToolkitMessage message)
	{
		await Task.CompletedTask;

		var responseMessage = $"{Faker.RandomString()}";

		ConsoleLog.WriteGreen($"Client Response: <{responseMessage}>");

		return responseMessage;
	}

	private static void RunServer()
	{
		var applicationSettings = new ToolkitWebApplicationSettings
								{
									Options = WebApplicationOptions.UseHttps | WebApplicationOptions.UseSignalR,
									CertificationLocation = @"C:\DevelopmentCert\DevelopmentCert.pfx",
									CertificationPassword = "basarab_cert",
									Port = WebPort,
									ContainerAssemblies = new List<Assembly> { Assembly.GetExecutingAssembly() },
									CorsUri = new List<Uri> { new($"https://localhost:{WebPort}") },
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

		applicationSettings.ClientMessage += message =>
											{
												ConsoleLog.WriteDarkCyan($"MessageId <{message.MessageType}> | Data <{message.Data}> | ConnectionId <{message.ConnectionId}>");

												var thread = SystemScope.Container.Resolve<IThread>();

												thread.Run(async () =>
															{
																await Task.Delay(1.Seconds());

																var hubServer = SystemScope.Container.Resolve<IToolkitHubServer>();

																await hubServer.SendToClientNoResponse(message.ConnectionId, new ToolkitMessage
																															{
																																MessageType = 2,
																																Data = $"Hello World {Faker.RandomString()}"
																															});

																await thread.Sleep(1.5.Seconds());

																ConsoleLog.WriteYellow("Going to send message to client and wait for a response");

																var response = await hubServer.SendToClient(message.ConnectionId, new ToolkitMessage
																																{
																																	MessageType = 3,
																																	Data = $"Waiting for Response {Faker.RandomString()}"
																																});

																ConsoleLog.WriteGreen($"Response: {JsonConvert.SerializeObject(response)}");
															});

												var result = $"MessageBack {DateTime.Now:h:mm:ss tt zzs} | {Faker.RandomString()}";

												ConsoleLog.WriteDarkCyan($"Sending back Response: {result}");

												return Task.FromResult(result);
											};

		ToolkitWebApplication.Run(applicationSettings);
	}

	private static void Started()
	{
		ConsoleLog.WriteGreen("Hey the web application has started!!!!!");

		var hubServer = SystemScope.Container.Resolve<IToolkitHubServer>();
		var thread = SystemScope.Container.Resolve<IThread>();

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