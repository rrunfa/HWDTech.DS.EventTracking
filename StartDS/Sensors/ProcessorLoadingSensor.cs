using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using HWdTech;
using HWdTech.DS.v30;
using HWdTech.DS.v30.Messages;
using HWdTech.DS.v30.PropertyObjects;
using HWdTech.Factories;
using StartDS.EventTracking.EventTrackers;
using StartDS.EventTracking.Interfaces;
using StartDS.Sensors.Interfaces;

namespace StartDS.Sensors
{
    public class ProcessorLoadingSensor : ISensor
    {
        private bool _isWathcing = false;
        private static readonly string _channelName = typeof (ProcessorLoadingSensor).Name + "Channel";

        public void Start()
        {
            StartWatching();
        }

        public void Stop()
        {
            _isWathcing = false;
        }

        public ITracked Channel()
        {
            return Tracked.WithString(_channelName);
        }

        public ITracked Message()
        {
            return null;
        }

        public static ProcessorLoadingSensor Create()
        {
            return new ProcessorLoadingSensor();
        }

        private void StartWatching()
        {
            var wathingThread = new Thread(WathcingInThread);
            _isWathcing = true;
            wathingThread.Start();
        }

        private void SendMessage(string messageText)
        {
            var message = PrepareMessageWithText(messageText);
            var channel = Singleton<DIFactory>.Instance.Create<IChannel>(_channelName);
            var builder = Singleton<DIFactory>.Instance.Create<ObjectBuilder>();
            builder.Message().To(channel).Make(message);
            MessageBus.Send(message);
        }

        private IMessage PrepareMessageWithText(string messageText)
        {
            var message = Singleton<DIFactory>.Instance.Create<IMessage>();
            var text = new Field<string>("text");
            var hash = new Field<string>("hash");
            text[message] = messageText;
            hash[message] = Tracked.WithString(_channelName).Hash();
            return message;
        }

        private void WathcingInThread()
        {
            var counters = GetPerformanceCounters();
            while (_isWathcing)
            {
                foreach (var performanceCounter in counters)
                {
                    var coreNumber = performanceCounter.InstanceName;
                    double coreLoading = performanceCounter.NextValue();
                    if (coreLoading > 50.0)
                    {
                        SendMessage("Core " + coreNumber + "was loaded over 50 % (" + coreLoading + ")");
                        Thread.Sleep(50);
                    }
                }
                Thread.Sleep(2000);
            }
        }

        private List<PerformanceCounter> GetPerformanceCounters()
        {
            var performanceCounters = new List<PerformanceCounter>();
            var procCount = Environment.ProcessorCount;
            for (var i = 0; i < procCount; i++)
            {
                var pc = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                performanceCounters.Add(pc);
            }
            return performanceCounters;
        }
    }
}
