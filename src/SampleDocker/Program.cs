using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Data;
using FatCat.Toolkit.Web.Api;
using FatCat.Toolkit.Web.Api.SignalR;
using Humanizer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace SampleDocker;

public static class Program
{
	public static void Main(params string[] args)
	{
		var environmentRepository = new EnvironmentRepository();

		environmentRepository.Set("MongoConnectionString", "mongodb+srv://dbasarab617:TSsKClHCCbWo3iJy@basarabcluster.jlophzn.mongodb.net/");
		environmentRepository.Set("MongoDatabaseName", "SampleDock");
		
		var applicationSettings = new ToolkitWebApplicationSettings
								{
									Options = WebApplicationOptions.UseHttps | WebApplicationOptions.UseSignalR,
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

	private static Task OnClientConnected(ToolkitUser user, string connectionId)
	{
		ConsoleLog.WriteDarkCyan($"A client has connected: <{user.Name}> | <{connectionId}>");

		return Task.CompletedTask;
	}

	private static Task OnClientDisconnected(ToolkitUser user, string connectionId)
	{
		ConsoleLog.WriteDarkYellow($"A client has disconnected: <{user.Name}> | <{connectionId}>");

		return Task.CompletedTask;
	}

	private static void Started() => ConsoleLog.WriteGreen("Hey the web application has started!!!!!");
}

public class SpikeToolkitParameters : IToolkitTokenParameters
{
	public TokenValidationParameters Get()
	{
		ConsoleLog.WriteCyan("Getting token parameters");

		var cert = new X509Certificate2(@"C:\DevelopmentCert\DevelopmentCert.pfx", "basarab_cert");

		return new TokenValidationParameters
				{
					IssuerSigningKey = new X509SecurityKey(cert),
					ValidAudience = "https://foghaze.com/Brume",
					ValidIssuer = "FogHaze",
					ClockSkew = 10.Seconds()
				};
	}
}