using HWdTech.DS.v30;
using StartDS.EventTracking.MessageChains.Interfaces;

namespace StartDS.EventTracking.Interfaces
{
    public interface IEventTracker : IJob
    {
        void AddChain(ITokenChain tokenChain);
        void RemoveChain(ITokenChain tokenChain);
    }
}
