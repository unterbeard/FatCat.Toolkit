using System.Diagnostics;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Threading;
using FatCat.Toolkit.Web;
using FatCat.Toolkit.Web.Api.SignalR;
using Humanizer;
using Newtonsoft.Json;

namespace OneOff;

public class ClientWorker
{
	private readonly IToolkitHubClientFactory hubFactory;
	private readonly IThread thread;
	private readonly IWebCallerFactory webCallerFactory;

	public ClientWorker(IThread thread,
						IToolkitHubClientFactory hubFactory,
						IWebCallerFactory webCallerFactory)
	{
		this.thread = thread;
		this.hubFactory = hubFactory;
		this.webCallerFactory = webCallerFactory;
	}

	public void DoWork(int webPort)
	{
		thread.Run(async () =>
					{
						var webCaller = webCallerFactory.GetWebCaller(new Uri($"https://localhost:{webPort}/api"));

						var tokenResult = await webCaller.Get("sample/token");

						if (tokenResult.IsUnsuccessful)
						{
							ConsoleLog.WriteRed($"Could not get TOKEN this is an error!!!!! | StatusCode := <{tokenResult.StatusCode}>");

							return;
						}

						var testToken = tokenResult.Content;

						ConsoleLog.Write($"Using Token := {testToken}");

						// Secure Url
						// var hubUrl = $"https://localhost:{webPort}/api/events?access_token={testToken}";

						// Open Url
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

						await SendDataBuffer(hubConnection);
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

	private static async Task SendDataBuffer(IToolkitHubClientConnection hubConnection)
	{
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
	}
}