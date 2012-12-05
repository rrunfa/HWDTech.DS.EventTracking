using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.Interfaces
{
    public interface ITracked : ISubject
    {
        string Hash();
    }
}