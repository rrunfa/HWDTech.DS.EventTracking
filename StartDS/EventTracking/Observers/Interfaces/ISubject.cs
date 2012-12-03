using StartDS.EventTracking.Interfaces;

namespace StartDS.EventTracking.Observers.Interfaces
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(ITracked tracked);
    }
}