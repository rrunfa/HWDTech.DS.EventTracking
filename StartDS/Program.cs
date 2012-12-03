using HWdTech.DS.v30;
using System;
using StartDS.EventTracking;
using StartDS.EventTracking.EventTrackers;
using StartDS.EventTracking.MessageChains;
using StartDS.EventTracking.Observers.Interfaces;
using StartDS.Sensors;

namespace StartDS
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var node = new Node(new Uri("http://localhost:9000/in")))
            {
                node.Start();
                var store = new EventTrackerStore();
                var filesystemTracker = EventTracker<FileSystemEventTrackerFactory>.Create();
                
                store.Add(filesystemTracker, node);

                var fileSystemSensor = FileSystemSensor.CreateWithPath("C:\\work");

                var toTrack = fileSystemSensor.Message();

                var simpleChain = new SimpleChain();
                simpleChain.Add(toTrack).Add(toTrack).Add(toTrack);
                simpleChain.ReadyBlock = () => Console.WriteLine("TestSimpleChain Ready!");

                filesystemTracker.Attach(simpleChain);
                filesystemTracker.Attach((IObserver) simpleChain.Clone());

                fileSystemSensor.Start();

                Console.ReadLine();
            }
        }
    }
}
