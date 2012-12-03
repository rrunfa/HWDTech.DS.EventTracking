using HWdTech.DS.v30;

namespace StartDS.EventTracking.Interfaces
{
    public interface IEventTrackerStore
    {
        void Add(IEventTracker eventTracker, Node node);
        void Delete(IEventTracker eventTracker);
    }
}
