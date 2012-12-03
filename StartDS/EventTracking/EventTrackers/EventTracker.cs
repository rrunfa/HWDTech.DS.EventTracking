using StartDS.EventTracking.Interfaces;

namespace StartDS.EventTracking.EventTrackers
{
    public static class EventTracker<T> where T : IEventTrackerFactory, new()
    {
        private static readonly T _eventTrackerFactory = new T();

        public static IEventTracker Create()
        {
            return _eventTrackerFactory.Create();;
        }
    }
}
