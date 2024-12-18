using System;
using System.Collections.Generic;
using System.Text;

namespace IOCTalk.InterceptProvider.CallMonitoring.Common
{
    public interface ICallMonitoringSource
    {
        Type MonitoringInterface { get; }

        object InterceptedServiceObject { get; }

        Exception LastException { get; }

        IEnumerable<(string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount)> GetCallMonitoringSnapshot();

    }
}
