using System.Collections.Generic;
using HWdTech.DS.v30;
using HWdTech.DS.v30.Channels;
using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.MessageChains.Interfaces;
using StartDS.EventTracking.Observers;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.EventTrackers
{
    public class SimpleEventTracker : IEventTracker
    {
        private readonly List<ITokenChain> _trackedChains = new List<ITokenChain>();
        private readonly NotifyManager _notifyManager;

        public SimpleEventTracker(IChangeManager changeManager)
        {
            _notifyManager = new NotifyManager(changeManager);
        }

        public SimpleEventTracker()
        {
            _notifyManager = new NotifyManager(new SimpleChangeManager());
        }

        public void AddChain(ITokenChain tokenChain)
        {
            _trackedChains.Add(tokenChain);
            _notifyManager.Attach(tokenChain);
        }

        public void RemoveChain(ITokenChain tokenChain)
        {
            _trackedChains.Remove(tokenChain);
            _notifyManager.Detach(tokenChain);
        }

        [ChannelEndpointHanlder("TrackersChannel")]
        public void ChangeHandle(IMessage message)
        {
            IToken token = Token.WithMessage(message);
            _notifyManager.Notify(token);
        }
    }
}
