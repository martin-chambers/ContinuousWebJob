using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuousWebJob;

namespace WebJobClient
{
    public class Program
    {
        private static string ServiceNamespace;
        private static string sasKeyName = "RootManageSharedAccessKey";
        private static string sasKeyValue;
        private const string QUEUE = "pocoqueue";

        public static void Main(string[] args)
        {
            ServiceNamespace = ConfigurationManager.AppSettings["ServiceNamespace"];
            // Issuer key
            sasKeyValue = ConfigurationManager.AppSettings["SASKey"];            

            // Create management credentials
            TokenProvider credentials = TokenProvider.CreateSharedAccessSignatureTokenProvider(sasKeyName, sasKeyValue);
            NamespaceManager namespaceClient = 
                new NamespaceManager(
                    ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty), 
                    credentials);
            QueueDescription myQueue;
            if (!namespaceClient.QueueExists(QUEUE))
            {
                myQueue = namespaceClient.CreateQueue(QUEUE);
            }
            MessagingFactory factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty), credentials);
            QueueClient myQueueClient = factory.CreateQueueClient(QUEUE);

            // Send message
            Poco poco = new Poco();
            poco.Id = "001";
            poco.content = "This is some content";
            BrokeredMessage message = new BrokeredMessage();
            message.Properties.Add("Id", poco.Id);
            message.Properties.Add("Content", poco.content);
            myQueueClient.Send(message);
        }
    }
}
