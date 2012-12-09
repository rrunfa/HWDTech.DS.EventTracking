using StartDS.EventTracking.Interfaces;

namespace StartDS.EventTracking.Observers.Interfaces
{
    public interface IChangeManager
    {
        void Register(ISubject tracked, IObserver observer);
        void Unregister(ISubject tracked, IObserver observer);
        void Notify(ISubject subject, IToken token);
    }
}