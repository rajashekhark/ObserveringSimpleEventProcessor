using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserversOnEventConsumer
{
    public class Observer1 : IObserver
    {

        public void When(MessageReceivedEventArgs msgEvent)
        {
            string contents = Encoding.UTF8.GetString(msgEvent.Message.GetBytes());
            Console.WriteLine(string.Format("Observer1: {0}", contents));
        }

    }
}
