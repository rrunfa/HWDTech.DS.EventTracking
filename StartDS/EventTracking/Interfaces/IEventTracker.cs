using HWdTech.DS.v30;
using StartDS.EventTracking.MessageChains.Interfaces;

namespace StartDS.EventTracking.Interfaces
{
    public interface IEventTracker : IJob
    {
        void Track(ITracked objectToTrack);
        void Untrack(ITracked trackedObject);
        void AddChain(ITrackedChain trackedChain);
        void RemoveChain(ITrackedChain trackedChain);
    }
}
