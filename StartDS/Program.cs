using HWdTech.DS.v30;
using System;
using StartDS.EventTracking;
using StartDS.EventTracking.EventTrackers;
using StartDS.EventTracking.MessageChains;
using StartDS.EventTracking.MessageChains.Interfaces;
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
                
                var simpleTracker = EventTracker<SimpleEventTrackerFactory>.Create();

                var store = new EventTrackerStore();
                store.Add(simpleTracker, node);

                var fileSystemSensor = FileSystemSensor.CreateWithPath("C:\\work");
                var processorLoadingSensor = ProcessorLoadingSensor.Create();

                var fileSystemTrack = fileSystemSensor.Channel();
                var processorLoadingTrack = processorLoadingSensor.Channel();

                var fileSystemChain = new SimpleChain();
                fileSystemChain.Add(fileSystemTrack).Add(fileSystemTrack).Add(fileSystemTrack);
                fileSystemChain.ReadyBlock = () => Console.WriteLine("FileSystemChain Ready!");

                var processorLoadingChain = new SimpleChain();
                processorLoadingChain.Add(processorLoadingTrack).Add(processorLoadingTrack);
                processorLoadingChain.ReadyBlock = () => Console.WriteLine("ProcessorLoadingChain Ready!");

                simpleTracker.AddChain(fileSystemChain);
                simpleTracker.AddChain(processorLoadingChain);

                fileSystemSensor.Start();
                processorLoadingSensor.Start();

                Console.ReadLine();
            }
        }
    }
}
