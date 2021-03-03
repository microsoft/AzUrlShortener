using System;
using System.Globalization;
using System.Threading.Tasks;
using Cloud5mins.domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cloud5mins.Function
{
    public static class ScheduleCheck
    {
        [FunctionName("ScheduleCheck")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var curTime = DateTime.Now;
            var changePoint = DateTime.ParseExact("20210303104800","yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            ShortUrlEntity input;
            ShortUrlEntity result;
            log.LogInformation($"The change point is : {changePoint}");

            if(DateTime.Compare(changePoint, curTime) <= 0){

                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
                    
                StorageTableHelper stgHelper = new StorageTableHelper(config["UlsDataStorage"]);

                input = new ShortUrlEntity{ 
                                Url = "http://github.com/fboucher",
                                Title = "Active Test Schedule",
                                PartitionKey = "t",
                                RowKey = "test"
                };

                result = await stgHelper.UpdateShortUrlEntity(input);
                log.LogInformation("--> URL Updated.");
            }
            else{
                log.LogInformation("--> Nothing was required at this time.");

            }
        }
    }
}
