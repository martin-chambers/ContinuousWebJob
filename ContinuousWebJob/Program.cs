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

        public static void WriteMessageInput([ServiceBusTrigger("pocoqueue")] BrokeredMessage message)
        {
            Console.WriteLine("Received JSON: Id = {0}, Content = {1}", message.Properties["Id"], message.Properties["Content"]);
        }
    }
}
