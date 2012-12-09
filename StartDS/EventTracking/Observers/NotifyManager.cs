using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.Observers
{
    public class NotifyManager : ISubject
    {
        private readonly IChangeManager _changeManager;

        public NotifyManager(IChangeManager changeManager)
        {
            _changeManager = changeManager;
        }

        public void Attach(IObserver observer)
        {
            _changeManager.Register(this, observer);
        }

        public void Detach(IObserver observer)
        {
            _changeManager.Unregister(this, observer);
        }

        public void Notify(IToken token)
        {
            _changeManager.Notify(this, token);
        }
    }
}