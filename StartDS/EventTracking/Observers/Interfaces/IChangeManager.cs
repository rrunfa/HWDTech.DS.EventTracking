using StartDS.EventTracking.Interfaces;

namespace StartDS.EventTracking.Observers.Interfaces
{
    public interface IChangeManager
    {
        void Register(ITracked tracked, IObserver observer);
        void Unregister(ITracked tracked, IObserver observer);
        void Notify(ITracked tracked);
    }
}