using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserversOnEventConsumer
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public DateTimeOffset ReceivedOn { get; set; }

        public EventData Message { get; set; } 
    }
}
