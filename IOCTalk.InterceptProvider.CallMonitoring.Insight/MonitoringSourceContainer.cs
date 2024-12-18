using IOCTalk.InterceptProvider.CallMonitoring.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCTalk.InterceptProvider.CallMonitoring.Insight
{
    internal class MonitoringSourceContainer : ICallMonitoringSourceContainer
    {
        Type monitoringInterface;
        List<ICallMonitoringSource> sourceItems = new List<ICallMonitoringSource>();

        public MonitoringSourceContainer(Type monitoringInterface)
        {
            this.monitoringInterface = monitoringInterface;
        }

        public Type MonitoringInterface => monitoringInterface;

        public List<ICallMonitoringSource> SourceItems => sourceItems;


        public IEnumerable<(string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount)> GetCallMonitoringSnapshot()
        {
            foreach (var item in sourceItems)
            {
                foreach (var callCounter in item.GetCallMonitoringSnapshot())
                {
                    yield return callCounter;
                }
            }
        }
    }
}
