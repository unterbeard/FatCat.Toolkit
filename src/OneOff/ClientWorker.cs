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
						var testToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjgzNzJFODE4OUYyNEVGN0VFMDk3RUMzODdCNTlGOTczMjY2MzI5QzkiLCJ4NXQiOiJnM0xvR0o4azczN2dsLXc0ZTFuNWN5WmpLY2siLCJ0eXAiOiJKV1QifQ.eyJ1bmlxdWVfbmFtZSI6IkpvaG4gRG9lIiwibmJmIjoxNjcyNjA2NjAyLCJleHAiOjE2NzI2MDc1MTIsImlhdCI6MTY3MjYwNjYxMiwiaXNzIjoiRm9nSGF6ZSIsImF1ZCI6Imh0dHBzOi8vZm9naGF6ZS5jb20vQnJ1bWUifQ.HcbVNn7SuuHE9QaGT8Ai62lrThEuZPQJbXkX9OTFmtO1lvoL83azPo8y2reOTjZvxcinx-IQ2kCoo4-uEU8Hwam88y8cxIPzVDiwwQHj6HLxCyAuZrKI9QoaxDqYLwas9zYE3GQh9-vHbdpF7DTJ6IDd11nsLOuZlZMFcEY1_4oZ7cd3zlMVUba--G2oDvIPdxhQRxMBBJDEXX05ssim4Ux0o6xitYHfdiJJgpbpNiNC0-RvHSCfoM68rGA4xsXMaWsj-9ZgNvUi5nZ8FNISlB6WDcaO736FPHFIaalcNvkCgiBaEcyAr-YGfFr54vDTW7sX5n60qPMB3cv7UBbb8w";
						
						var hubUrl = $"https://localhost:{webPort}/api/events?access_token={testToken}";

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