using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserversOnEventConsumer
{
    public class ObserverRegistry
    {
        public IEnumerable<IObserver> GetObservers()
        {
            yield return new Observer1();
            yield return new Observer2();
        }
    }
}
