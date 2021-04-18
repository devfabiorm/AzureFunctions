using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctions.Function
{
    public static class TimerTrigger
    {
        [FunctionName("TimerTrigger")]
        public async static Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var key = "NioyNf0nlwhzYgOUYi99FNYPsnWq81klgyuuZFFHNdn/73ca5CKT2rOaHtK9TL37KE7wEimHcdVFly7472/dhg==";
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName=aluracourse;AccountKey={key};EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient myClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer myContainer = myClient.GetContainerReference("prod");
            ICloudBlob myBlob = await myContainer.GetBlobReferenceFromServerAsync("teste.txt");

            await Backup(myBlob, log);

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        public async static Task Backup(ICloudBlob myBlob, ILogger log)
        {
            var key = "NioyNf0nlwhzYgOUYi99FNYPsnWq81klgyuuZFFHNdn/73ca5CKT2rOaHtK9TL37KE7wEimHcdVFly7472/dhg==";
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName=aluracourse;AccountKey={key};EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("bkp");
            
            CloudBlockBlob blockBlod = container.GetBlockBlobReference(myBlob.Name);

            try
            {
                await container.CreateIfNotExistsAsync();
            }
            catch (Exception e)
            {
                
                log.LogError(e.Message);
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(myBlob.Name);

            await blockBlob.StartCopyAsync(myBlob as CloudBlockBlob);
        }
    }
}
