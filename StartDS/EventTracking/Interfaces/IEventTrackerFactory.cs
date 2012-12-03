namespace StartDS.EventTracking.Interfaces
{
    public interface IEventTrackerFactory
    {
        IEventTracker Create();
    }
}