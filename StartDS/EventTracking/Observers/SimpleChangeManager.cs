using System.Collections.Generic;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.Observers
{
    public class SimpleChangeManager : IChangeManager
    {
        private readonly HashSet<KeyValuePair<IObserver, ITracked>> _map = new HashSet<KeyValuePair<IObserver, ITracked>>();

        public void Register(ITracked tracked, IObserver observer)
        {
            _map.Add(new KeyValuePair<IObserver, ITracked>(observer, tracked));
        }

        public void Unregister(ITracked tracked, IObserver observer)
        {
            _map.Remove(new KeyValuePair<IObserver, ITracked>(observer, tracked));
        }

        public void Notify(ITracked tracked)
        {
            foreach (var keyValuePair in _map)
            {
                var currentTracked = keyValuePair.Value;
                if (currentTracked.Hash() == tracked.Hash())
                {
                    var currentObserver = keyValuePair.Key;
                    currentObserver.Update(tracked);
                }
            }
        }
    }
}
