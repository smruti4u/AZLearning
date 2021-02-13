using System;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace AZFunctionApp
{
    public static class TimerTrigger
    {
        [FunctionName("TimerTrigger")]
        [return: Table("VSTable")]
        public static MyEntity Run([TimerTrigger("0 */1 * * * *", RunOnStartup = true)]TimerInfo myTimer, [Blob("test/test.txt", System.IO.FileAccess.Read)] CloudBlob blob, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {blob.Name}");
            return new MyEntity() { Name = blob.Name + DateTime.Now };
        }


        public class MyEntity : TableEntity
        {
            public MyEntity()
            {
                this.PartitionKey = "OutPutBinding";
                RowKey = Guid.NewGuid().ToString();
            }

            public string Name { get; set; }
        }
    }
}
