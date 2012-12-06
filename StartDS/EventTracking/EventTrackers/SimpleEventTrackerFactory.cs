using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers;

namespace StartDS.EventTracking.EventTrackers
{
    public class SimpleEventTrackerFactory : IEventTrackerFactory
    {
        public IEventTracker Create()
        {
            return new SimpleEventTracker(new SimpleChangeManager());
        }
    }
}
