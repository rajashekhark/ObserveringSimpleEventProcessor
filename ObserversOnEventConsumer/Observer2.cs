using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserversOnEventConsumer
{
    public class Observer2 : IObserver
    {
        public void When(MessageReceivedEventArgs msgEvent)
        {
            string contents = Encoding.UTF8.GetString(msgEvent.Message.GetBytes());
            Console.WriteLine(string.Format("Observer2: {0}", contents));
        }
    }
}
