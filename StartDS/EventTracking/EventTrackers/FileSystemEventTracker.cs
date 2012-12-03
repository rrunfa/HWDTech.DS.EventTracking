using System;
using HWdTech.DS.v30;
using HWdTech.DS.v30.Channels;
using HWdTech.DS.v30.PropertyObjects;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.EventTrackers
{
    public class FileSystemEventTracker : IEventTracker
    {
        private readonly IChangeManager _changeManager;

        public FileSystemEventTracker(IChangeManager changeManager)
        {
            _changeManager = changeManager;
        }

        public void Track(ITracked objectToTrack)
        {
            Console.WriteLine("Start Tracking...");
        }

        public void Untrack(ITracked trackedObject)
        {
            Console.WriteLine("Stop Tracking...");
        }

        [ChannelEndpointHanlder("FileSystemSensorChannel")]
        public void ChangeHandle(IMessage message)
        {
            var text = new Field<string>("text");
            var hash = new Field<string>("hash");

            Console.WriteLine("Change message: " + text[message]);
            if (message is Message)
            {
                Notify(Tracked.WithHash(hash[message]));
            }
        }

        public void Attach(IObserver observer)
        {
            _changeManager.Register(this, observer);
        }

        public void Detach(IObserver observer)
        {
            _changeManager.Unregister(this, observer);
        }

        public void Notify(ITracked tracked)
        {
            _changeManager.Notify(this, tracked);
        }
    }
}
