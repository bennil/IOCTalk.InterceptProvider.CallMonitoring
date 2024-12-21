using System;
using System.Collections.Generic;
using System.Text;

namespace IOCTalk.InterceptProvider.CallMonitoring.Common
{
    public interface ICallMonitoringSourceContainer
    {
        Type MonitoringInterface { get; }

        List<ICallMonitoringSource> SourceItems { get; }


        IEnumerable<(string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount, long ExecTimeTotal, long ExecTimeMax)> GetCallMonitoringSnapshot();

    }
}
