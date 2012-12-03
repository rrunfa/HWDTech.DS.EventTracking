using System;
using System.Collections.Generic;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.MessageChains.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.MessageChains
{
    public class SimpleChain : ITrackedChain, ICloneable
    {
        private readonly List<ITracked> _trackedChainAtList = new List<ITracked>();
        private int _count = 0;

        public Action ReadyBlock { get; set; }

        public ITrackedChain Add(ITracked message)
        {
            _trackedChainAtList.Add(message);
            return this;
        }

        public ITrackedChain Remove(ITracked message)
        {
            _trackedChainAtList.Remove(message);
            return this;
        }

        public void Update(ISubject subject, ITracked tracked)
        {
            if (_trackedChainAtList[_count].Hash() == tracked.Hash())
            {
                _count++;
            }
            
            if (_count == _trackedChainAtList.Count)
            {
                ReadyBlock.Invoke();
                _count = 0;
            }
        }

        public object Clone()
        {
            var clone = new SimpleChain();
            foreach (var tracked in _trackedChainAtList)
            {
                clone.Add(tracked);
            }
            clone.ReadyBlock = ReadyBlock;
            return clone;
        }
    }
}
