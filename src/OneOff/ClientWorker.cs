using System.Diagnostics;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.Web.Api.SignalR;
using Humanizer;
using Newtonsoft.Json;

namespace OneOff;

public class ClientWorker
{
	private readonly IToolkitHubClientFactory hubFactory;
	private readonly IThread thread;

	public ClientWorker(IThread thread,
						IToolkitHubClientFactory hubFactory)
	{
		this.thread = thread;
		this.hubFactory = hubFactory;
	}

	public void DoWork(int webPort)
	{
		thread.Run(async () =>
					{
						var hubUrl = $"https://localhost:{webPort}/api/events";

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
}