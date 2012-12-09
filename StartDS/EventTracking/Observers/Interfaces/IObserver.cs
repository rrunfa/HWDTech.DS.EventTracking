using StartDS.EventTracking.Interfaces;

namespace StartDS.EventTracking.Observers.Interfaces
{
    public interface IObserver
    {
        void Update(IToken token);
    }
}