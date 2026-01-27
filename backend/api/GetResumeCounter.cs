using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Company.Function
{
    public static class GetResumeCounter
    {
        [FunctionName("GetResumeCounter")]
        public static  HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,

            //Input binding: fetch the single document by id + partition key
            [CosmosDB(
                databaseName:"AzureResume", 
                containerName : "Counter", 
                Connection = "AzureResumeConnectionString", 
                Id = "1", 
                PartitionKey = "1")] Counter counter,

             // Output binding: write the updated entity   
            [CosmosDB(
                databaseName:"AzureResume", 
                containerName : "Counter", 
                Connection = "AzureResumeConnectionString",
                Id = "1", 
                PartitionKey = "1")] out Counter  updatedCounter,

            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
                
                updatedCounter = counter;
                updatedCounter.Count += 1;

            
 //Return the updated document as JSON

            var jasonToRetun = JsonConvert.SerializeObject(counter);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(jasonToRetun, Encoding.UTF8, "application/json")
            };
        }
        
    }
}