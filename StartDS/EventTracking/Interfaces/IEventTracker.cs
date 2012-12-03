using HWdTech.DS.v30;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.Interfaces
{
    public interface IEventTracker : IJob, ISubject
    {
        void Track(ITracked objectToTrack);
        void Untrack(ITracked trackedObject);
    }
}
