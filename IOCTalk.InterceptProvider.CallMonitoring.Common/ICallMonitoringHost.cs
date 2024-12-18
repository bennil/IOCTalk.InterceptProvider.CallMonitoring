using System;
using System.Collections.Generic;

namespace IOCTalk.InterceptProvider.CallMonitoring.Common
{
    public interface ICallMonitoringHost
    {
        void RegisterSource(ICallMonitoringSource source);

        void UnregisterSource(ICallMonitoringSource source);

        IReadOnlyList<ICallMonitoringSourceContainer> MonitorSources { get; }

        ICallMonitoringSourceContainer GetSourceByInterface(string interfaceName);
    }
}
