using Microsoft.Azure.WebJobs;

namespace PiwikPRO.SharePoint.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            config.UseTimers();
            config.DashboardConnectionString = "DefaultEndpointsProtocol=https;AccountName=kogifilist8d24;AccountKey=BNBaVPh1sCHICO3P40LHpaTKnD8KKwgbZfLRIIILK3GP4L15/Sy8+0ZkEB/wrtqwyi0xlWLGwsjjD/JR2e7+1A==;EndpointSuffix=core.windows.net";
            config.StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=kogifilist8d24;AccountKey=BNBaVPh1sCHICO3P40LHpaTKnD8KKwgbZfLRIIILK3GP4L15/Sy8+0ZkEB/wrtqwyi0xlWLGwsjjD/JR2e7+1A==;EndpointSuffix=core.windows.net";

            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
