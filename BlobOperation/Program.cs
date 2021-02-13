using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobOperation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=azsaleran;AccountKey=A480h8tk5hBv9r9TZ3KqGD6MZJHNPt69g1mUjzj/+ntL61OURlfi6VWVSQ6N5XIHGqiU6S2i1ql9ijZveq2k5A==;EndpointSuffix=core.windows.net";

            BlobServiceClient client = new BlobServiceClient(connectionString);

            BlobContainerClient blobContainer =  client.GetBlobContainerClient("vsconatiner");
            
            await blobContainer.CreateIfNotExistsAsync();

            string fileName = "Demo.txt";
            var blobClient =  blobContainer.GetBlobClient("test/" + fileName);
            await blobClient.UploadAsync(fileName, overwrite: true);
           var properties =  await blobClient.GetPropertiesAsync();
            var prop = blobContainer.GetPropertiesAsync();
            var items =  blobContainer.GetBlobs(prefix:"test");
            foreach(var item in items)
            {
                await HandleConCurrency(blobClient, item, ConcurrencyType.Default);
                await HandleConCurrency(blobClient, item, ConcurrencyType.Pessimistic);
                await HandleConCurrency(blobClient, item, ConcurrencyType.Optimistic);
                Console.WriteLine(item.Name);
            }

            IDictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("Author", "VisualStudio");
            metadata.Add("Category", "operation");
            await blobClient.SetMetadataAsync(metadata);
           
            await ListBlobsAsPaging(blobContainer, 2);
        }

        private static async Task ListBlobsAsPaging(BlobContainerClient client, int size)
        {
            var resultSegment = client.GetBlobsAsync().AsPages(default, size);
            await foreach(var blobPages in resultSegment)
            {
                foreach(var blobs in blobPages?.Values)
                {
                    Console.WriteLine(blobs.Name);
                }
            }
        }

        private static async Task HandleConCurrency(BlobClient blobClient, BlobItem item, ConcurrencyType type)
        {
            switch(type)
            {
                case ConcurrencyType.Default:
                    IDictionary<string, string> metadata = new Dictionary<string, string>();
                    metadata.Add("Author", "VisualStudio");
                    metadata.Add("Category", "default");
                    await blobClient.SetMetadataAsync(metadata);
                    break;
                case ConcurrencyType.Optimistic:
                    IDictionary<string, string> metadata1 = new Dictionary<string, string>();
                    metadata1.Add("Author", "VisualStudio");
                    metadata1.Add("Category", "Optimistic");
                    try
                    {
                        BlobRequestConditions conditions = new BlobRequestConditions()
                        {
                            IfMatch = item.Properties.ETag
                        };
                        await blobClient.SetMetadataAsync(metadata1, conditions);
                    }
                    catch(Exception exe)
                    {

                    }
                    
                    break;
                case ConcurrencyType.Pessimistic:
                    IDictionary<string, string> metadata2 = new Dictionary<string, string>();
                    metadata2.Add("Author", "VisualStudio");
                    metadata2.Add("Category", "Optimistic");
                    try
                    {
                        BlobRequestConditions conditions1 = new BlobRequestConditions()
                        {
                            LeaseId = "12345"
                        };
                        await blobClient.SetMetadataAsync(metadata2, conditions1);
                    }
                    catch(Exception exe)
                    {

                    }
                    
                    break;
            }
        }

    }
}
