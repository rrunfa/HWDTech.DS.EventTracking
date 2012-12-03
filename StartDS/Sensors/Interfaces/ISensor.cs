using HWdTech.DS.v30;
using StartDS.EventTracking.Interfaces;

namespace StartDS.Sensors.Interfaces
{
    public interface ISensor : IJob
    {
        void Start();
        void Stop();
        ITracked Channel();
        ITracked Message();
    }
}