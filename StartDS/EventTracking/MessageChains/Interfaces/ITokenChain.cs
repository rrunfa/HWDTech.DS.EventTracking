using StartDS.EventTracking.Interfaces;
using StartDS.EventTracking.Observers.Interfaces;

namespace StartDS.EventTracking.MessageChains.Interfaces
{
    public interface ITokenChain : IObserver
    {
        ITokenChain Add(IToken token);
        ITokenChain Remove(IToken token);
    }
}