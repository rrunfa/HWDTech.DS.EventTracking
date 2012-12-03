using StartDS.EventTracking.Interfaces;

namespace StartDS.EventTracking.Observers.Interfaces
{
    public interface IChangeManager
    {
        void Register(ISubject subject, IObserver observer);
        void Unregister(ISubject subject, IObserver observer);
        void Notify(ISubject subject, ITracked tracked);
    }
}