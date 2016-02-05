using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserversOnEventConsumer
{
    public class SimpleMessageConsumer : IEventProcessor 
    {
        private Stopwatch checkpointStopWatch;

        public static event EventHandler<MessageReceivedEventArgs> OnMessageReceived;        

        public SimpleMessageConsumer()
        { }
                
        public Task OpenAsync(PartitionContext context)
        {
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            try
            {
                foreach (EventData message in messages)
                {
                    string contents = Encoding.UTF8.GetString(message.GetBytes());
                    Console.WriteLine(string.Format("SimpleMessageConsumer: {0}", contents));
                    OnMessageReceived(this, new MessageReceivedEventArgs() { ReceivedOn = DateTimeOffset.UtcNow, Message = message });
                }
                
                if (this.checkpointStopWatch.Elapsed > TimeSpan.FromSeconds(2))
                {
                    lock (this)
                    {
                        this.checkpointStopWatch.Reset();
                        return context.CheckpointAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Task.FromResult<object>(null);
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            if (OnMessageReceived != null)
            {
                foreach (EventHandler<MessageReceivedEventArgs> subscriber in OnMessageReceived.GetInvocationList())
                {
                    OnMessageReceived -= subscriber;
                }
            }
            this.checkpointStopWatch.Stop();
            if (reason.Equals(CloseReason.Shutdown))
            {
                await context.CheckpointAsync();
            }
        }

    }
}
