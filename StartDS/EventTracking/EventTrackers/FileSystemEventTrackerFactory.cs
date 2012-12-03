using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers;

namespace StartDS.EventTracking.EventTrackers
{
    public class FileSystemEventTrackerFactory : IEventTrackerFactory
    {
        public IEventTracker Create()
        {
            return new FileSystemEventTracker(new SimpleChangeManager());
        }
    }
}
