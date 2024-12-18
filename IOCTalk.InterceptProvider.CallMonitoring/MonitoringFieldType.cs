using System;
using System.Collections.Generic;
using System.Text;

namespace IOCTalk.InterceptProvider.CallMonitoring
{
    internal enum MonitoringFieldType
    {
        Undefined = 0,

        InvokeCount,

        InvokeCompletedCount,

        ExceptionCount,

        MethodName,
    }
}
