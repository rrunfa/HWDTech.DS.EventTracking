using System;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.MessageChains.Interfaces
{
    public interface ITrackedChain : IObserver, IDisposable
    {
        ITrackedChain Add(ITracked message);
        ITrackedChain Remove(ITracked message);

        void Initialize(IChangeManager changeManager);
    }
}