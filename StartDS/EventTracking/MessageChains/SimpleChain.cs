using System;
using System.Collections.Generic;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.MessageChains.Interfaces;

namespace StartDS.EventTracking.MessageChains
{
    public class SimpleChain : ITokenChain, ICloneable
    {
        private readonly List<IToken> _tokenChainAtList = new List<IToken>();
        private int _count = 0;
        private readonly object _thisLock = new object();

        public Action ReadyBlock { get; set; }

        public ITokenChain Add(IToken token)
        {
            _tokenChainAtList.Add(token);
            return this;
        }

        public ITokenChain Remove(IToken token)
        {
            _tokenChainAtList.Remove(token);
            return this;
        }

        public void Update(IToken token)
        {
            lock (_thisLock)
            {
                if (_tokenChainAtList[_count].Hash() == token.Hash())
                    _count++;
                
                if (_count == _tokenChainAtList.Count)
                {
                    _count = 0;
                    ReadyBlock.Invoke();
                }
            }
        }

        public object Clone()
        {
            var clone = new SimpleChain();
            foreach (var token in _tokenChainAtList)
            {
                clone.Add(token);
            }
            clone.ReadyBlock = ReadyBlock;
            return clone;
        }
    }
}
