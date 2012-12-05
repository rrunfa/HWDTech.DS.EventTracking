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
        private readonly object _thisLock = new object();

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

        public void Initialize(IChangeManager changeManager)
        {
            var firstTracked = _trackedChainAtList[0];
            firstTracked.Attach(this);
        }

        public void Update(ITracked tracked)
        {
            lock (_thisLock)
            {
                if (_trackedChainAtList[_count].Hash() == tracked.Hash())
                    _count++;
                
                if (_count == _trackedChainAtList.Count)
                {
                    _count = 0;
                    ReadyBlock.Invoke();
                }
                tracked.Detach(this);
                var nextTracked = _trackedChainAtList[_count];
                nextTracked.Attach(this);
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

        public void Dispose()
        {
            var nextTracked = _trackedChainAtList[_count];
            nextTracked.Detach(this);
        }
    }
}
