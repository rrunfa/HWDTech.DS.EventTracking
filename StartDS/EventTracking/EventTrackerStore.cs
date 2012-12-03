using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using HWdTech;
using HWdTech.DS.v30;
using HWdTech.DS.v30.MessageReceivers;
using HWdTech.DS.v30.MessageRouters;
using HWdTech.DS.v30.Messages;
using HWdTech.Factories;
using StartDS.EventTracking.Interfaces;

namespace StartDS.EventTracking
{
    public class EventTrackerStore : IEventTrackerStore
    {
        readonly List<IEventTracker> _trackers = new List<IEventTracker>();

        public void Add(IEventTracker eventTracker, Node node)
        {
            var message = node.CreateStartJobMessage(eventTracker,
                Singleton<DIFactory>.Instance.Create<IChannel>("OutOfNodeInSameProcess"));

            var queue = new BlockingCollection<IMessage>(1);
            var callback = new QueueBasedMessageReceiver(queue);

            var obj = Singleton<DIFactory>.Instance.Create<IObject>(
                new Dictionary<string, object>() { 
                        { TypeBasedMessageReceiverRegistryInfoObjectPattern.Type.Name, "StartJobResponse" } 
                    }
            );

            MessageBus.Connect(obj, callback);
            MessageBus.Send(message);

            IMessage response;
            var failMessage = "Job " + eventTracker.GetType().Name + " could not start.";
            if (!queue.TryTake(out response, 2000))
            {
                throw new Exception(failMessage);
            }
            if (!ResponseMessageObjectPattern.Success[response])
            {
                throw new Exception(failMessage);
            }
            _trackers.Add(eventTracker);
        }

        public void Delete(IEventTracker eventTracker)
        {
            _trackers.Remove(eventTracker);
        }
    }
}
