namespace StartDS.EventTracking.Observers.Interfaces
{
    public interface ISubject
    {
        void Attach(IObserver observer, IChangeManager changeManager);
        void Detach(IObserver observer, IChangeManager changeManager);
        void Notify(IChangeManager changeManager);
    }
}