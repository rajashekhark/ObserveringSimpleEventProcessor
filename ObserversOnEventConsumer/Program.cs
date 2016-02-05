using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserversOnEventConsumer
{
    class Program
    {

        private static readonly string _eventHubName = CloudConfigurationManager.GetSetting("EventHubName");
        private static readonly string _producerConnectionStr = CloudConfigurationManager.GetSetting("ProducerConnectionStr");
        private static readonly string _consumerConnectionStr = CloudConfigurationManager.GetSetting("ConsumerConnectionStr");
        private static readonly string _storageConnectionStr = CloudConfigurationManager.GetSetting("StorageConnectionStr");

        static void Main(string[] args)
        {
            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(
                eventProcessorHostName,
                _eventHubName,
                EventHubConsumerGroup.DefaultGroupName,
                _consumerConnectionStr,
                _storageConnectionStr);

            try
            {
                eventProcessorHost.RegisterEventProcessorAsync<SimpleMessageConsumer>().Wait();

                ObserverRegistry registry = new ObserverRegistry();
                foreach (IObserver observer in registry.GetObservers())
                {
                    SimpleMessageConsumer.OnMessageReceived += new EventHandler<MessageReceivedEventArgs>(
                    (sender, e) => observer.When(e));
                }
                
                var producer = EventHubClient.CreateFromConnectionString(_producerConnectionStr, _eventHubName);

                while (true)
                {
                    producer.Send(new EventData(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())));
                    Task.Delay(TimeSpan.FromSeconds(30)).Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                eventProcessorHost.UnregisterEventProcessorAsync().Wait();
            }
        }

    }
}
