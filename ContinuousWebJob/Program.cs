using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuousWebJob
{
    public class Program
    {
        private static string sbConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsServiceBus"].ToString();
        public static void Main(string[] args)
        {
            JobHostConfiguration config = new JobHostConfiguration();
            ServiceBusConfiguration servicebusConfig = new ServiceBusConfiguration
            {
                ConnectionString = sbConnectionString
            };
            config.UseServiceBus(servicebusConfig);
            JobHost host = new JobHost(config);
            host.RunAndBlock();
        }

        public class Poco
        {
            public string Id { get; set; }
            public string content { get; set; }
        }

        public static void WriteMessageInput([ServiceBusTrigger("pocoqueue")] Poco poco)
        {
            Console.WriteLine("Received JSON: {0}", poco.content);
        }
    }
}
