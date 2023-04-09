using FatCat.Toolkit.Web;
using Microsoft.AspNetCore.Mvc;

namespace SampleDocker;

public class GetStorageItemsEndpoint : Endpoint
{
	[HttpPost("api/store")]
	public WebResult GetStorageItems()
	{
		// TODO: Replace <storage-account-name> with your actual storage account name
		var blobServiceClient = new BlobServiceClient(
													new Uri("https://<storage-account-name>.blob.core.windows.net"),
													new DefaultAzureCredential());

		//Create a unique name for the container
		string containerName = "quickstartblobs" + Guid.NewGuid().ToString();

		// Create the container and return a container client object
		BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
		
		return NotImplemented();
	}
}