using System;
using System.Collections.Generic;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.Observers
{
    public class SimpleChangeManager : IChangeManager
    {
        private readonly Dictionary<ISubject, List<IObserver>> _map =
            new Dictionary<ISubject, List<IObserver>>();

        private static readonly object _thisLock = new object();

        public void Register(ISubject subject, IObserver observer)
        {
            lock (_thisLock)
            {
                try
                {
                    var observers = _map[subject];
                    observers.Add(observer);
                }
                catch (Exception)
                {
                    var observers = new List<IObserver>(1) {observer};
                    _map.Add(subject, observers);
                }
            }
            
        }

        public void Unregister(ISubject subject, IObserver observer)
        {
            lock (_thisLock)
            {
                try
                {
                    var observers = _map[subject];
                    observers.Remove(observer);
                    if (observers.Count == 0)
                        _map.Remove(subject);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Notify(ISubject subject, IToken token)
        {
            lock (_thisLock)
            {
                try
                {
                    _map[subject].ForEach(o => o.Update(token));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
