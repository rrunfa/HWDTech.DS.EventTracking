using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.MessageChains.Interfaces
{
    public interface ITrackedChain : IObserver
    {
        ITrackedChain Add(ITracked message);
        ITrackedChain Remove(ITracked message);
    }
}