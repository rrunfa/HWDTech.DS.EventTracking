using System;
using System.Collections.Generic;
using HWdTech.DS.v30;
using HWdTech.DS.v30.Channels;
using HWdTech.DS.v30.PropertyObjects;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.MessageChains.Interfaces;
using StartDS.EventTracking.Observers;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.EventTrackers
{
    public class SimpleEventTracker : IEventTracker
    {
        private readonly IChangeManager _changeManager;
        private readonly List<ITrackedChain> _trackedChains = new List<ITrackedChain>();

        public SimpleEventTracker(IChangeManager changeManager)
        {
            _changeManager = changeManager;
        }

        public SimpleEventTracker()
        {
            _changeManager = new SimpleChangeManager();
        }

        public void Track(ITracked objectToTrack)
        {
            Console.WriteLine("Start Tracking Channel...");
        }

        public void Untrack(ITracked trackedObject)
        {
            Console.WriteLine("Stop Tracking Channel...");
        }

        public void AddChain(ITrackedChain trackedChain)
        {
            _trackedChains.Add(trackedChain);
            trackedChain.Initialize(_changeManager);
            //TODO: Track all Channels from Chain
        }

        public void RemoveChain(ITrackedChain trackedChain)
        {
            _trackedChains.Remove(trackedChain);
            trackedChain.Dispose();
        }

        [ChannelEndpointHanlder("FileSystemSensorChannel")]
        public void ChangeHandle1(IMessage message)
        {
            var text = new Field<string>("text");
            var hash = new Field<string>("hash");

            Console.WriteLine("Change message: " + text[message]);
            if (message is Message)
            {
                Tracked.WithHash(hash[message]).Notify(_changeManager);
            }
        }

        [ChannelEndpointHanlder("ProcessorLoadingSensorChannel")]
        public void ChangeHandle2(IMessage message)
        {
            var text = new Field<string>("text");
            var hash = new Field<string>("hash");

            Console.WriteLine("Change message: " + text[message]);
            if (message is Message)
            {
                Tracked.WithHash(hash[message]).Notify(_changeManager);
            }
        }
    }
}
