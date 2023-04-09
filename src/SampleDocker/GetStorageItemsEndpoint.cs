using Azure.Identity;
using Azure.Storage.Blobs;
using FatCat.Toolkit.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SampleDocker;

public class GetStorageItemsEndpoint : Endpoint
{
	private const string ContainerName = "quickstartblobs-da3c108f-fb28-47cb-8e57-4a2578253c3c";
	private readonly IConfiguration configuration;

	public GetStorageItemsEndpoint(IConfiguration configuration) => this.configuration = configuration;

	[HttpPost("api/store")]
	public async Task<WebResult> GetStorageItems()
	{
		var blobUrl = configuration["BlobUrl"];

		// TODO: Replace <storage-account-name> with your actual storage account name
		var blobServiceClient = new BlobServiceClient(
													new Uri(blobUrl),
													new DefaultAzureCredential());

		//Create a unique name for the container

		// Create the container and return a container client object
		BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(ContainerName);

		return NotImplemented();
	}
}