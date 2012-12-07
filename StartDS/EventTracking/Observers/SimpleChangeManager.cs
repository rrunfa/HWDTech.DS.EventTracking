using System;
using System.Collections.Generic;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.Observers
{
    public class SimpleChangeManager : IChangeManager
    {
        private readonly Dictionary<string, List<IObserver>> _map =
            new Dictionary<string, List<IObserver>>();

        private static object _thisLock = new object();

        public void Register(ITracked tracked, IObserver observer)
        {
            lock (_thisLock)
            {
                try
                {
                    var observers = _map[tracked.Hash()];
                    observers.Add(observer);
                }
                catch (Exception)
                {
                    var observers = new List<IObserver>(1) {observer};
                    _map.Add(tracked.Hash(), observers);
                }
            }
            
        }

        public void Unregister(ITracked tracked, IObserver observer)
        {
            lock (_thisLock)
            {
                try
                {
                    var observers = _map[tracked.Hash()];
                    observers.Remove(observer);
                    if (observers.Count == 0)
                        _map.Remove(tracked.Hash());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Notify(ITracked tracked)
        {
            lock (_thisLock)
            {
                try
                {
                    _map[tracked.Hash()].ForEach(o => o.Update(tracked));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
