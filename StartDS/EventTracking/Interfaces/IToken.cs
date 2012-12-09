using HWdTech.DS.v30;

namespace StartDS.EventTracking.Interfaces
{
    public interface IToken
    {
        string Hash();
        string Type();
        string Data();

        IMessage WrapMessage(IMessage message);
    }
}