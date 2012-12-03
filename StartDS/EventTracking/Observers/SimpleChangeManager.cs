using System.Collections.Generic;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.Observers
{
    public class SimpleChangeManager : IChangeManager
    {
        //private readonly Dictionary<IObserver, ISubject> _map = new Dictionary<IObserver, ISubject>();
        private  readonly HashSet<KeyValuePair<IObserver, ISubject>>  _map = new HashSet<KeyValuePair<IObserver, ISubject>>();

        public void Register(ISubject subject, IObserver observer)
        {
            _map.Add(new KeyValuePair<IObserver, ISubject>(observer, subject));
        }

        public void Unregister(ISubject subject, IObserver observer)
        {
            _map.Remove(new KeyValuePair<IObserver, ISubject>(observer, subject));
        }

        public void Notify(ISubject subject, ITracked tracked)
        {
            foreach (KeyValuePair<IObserver, ISubject> keyValuePair in _map)
            {
                var currentSubject = keyValuePair.Value;
                if (currentSubject == subject)
                {
                    var currentObserver = keyValuePair.Key;
                    currentObserver.Update(currentSubject, tracked);
                }
            }
        }
    }
}
