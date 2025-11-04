using IOCTalk.InterceptProvider.CallMonitoring.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCTalk.InterceptProvider.CallMonitoring.Insight
{
    public class CallMonitoringHost : ICallMonitoringHost
    {
        List<ICallMonitoringSourceContainer> sourceItems = new List<ICallMonitoringSourceContainer>();
        Dictionary<string, ICallMonitoringSourceContainer> sourceItemsDic = new Dictionary<string, ICallMonitoringSourceContainer>();

        public IReadOnlyList<ICallMonitoringSourceContainer> MonitorSources => sourceItems.AsReadOnly();

        readonly static CallMonComparer callMonComparer = new();

        public void RegisterSource(ICallMonitoringSource source)
        {
            ICallMonitoringSourceContainer container;
            string containerKey = source.MonitoringInterface.FullName;
            if (sourceItemsDic.TryGetValue(containerKey, out container) == false)
            {
                container = new MonitoringSourceContainer(source.MonitoringInterface);

                // insert sorted
                int insertIndex = sourceItems.BinarySearch(container, callMonComparer);
                if (insertIndex < 0)
                    sourceItems.Insert(~insertIndex, container);
                else
                    sourceItems.Add(container);

                sourceItemsDic.Add(containerKey, container);
            }
            container.SourceItems.Add(source);
        }

        public void UnregisterSource(ICallMonitoringSource source)
        {
            string containerKey = source.MonitoringInterface.FullName;
            if (sourceItemsDic.TryGetValue(containerKey, out var container) == true)
            {
                container.SourceItems.Remove(source);
            }
        }

        public ICallMonitoringSourceContainer GetSourceByInterface(string interfaceName)
        {
            return sourceItemsDic[interfaceName];
        }

        // Comparer to sort ICallMonitoringSourceContainer list
        internal class CallMonComparer : IComparer<ICallMonitoringSourceContainer>
        {
            public int Compare(ICallMonitoringSourceContainer x, ICallMonitoringSourceContainer y)
            {
                return string.Compare(x?.MonitoringInterface?.FullName ?? string.Empty, y?.MonitoringInterface?.FullName ?? string.Empty);
            }
        }


    }

}
