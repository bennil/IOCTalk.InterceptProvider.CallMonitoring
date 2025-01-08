using CallMonitoringSourceGen.TestConsole.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMonitoringSourceGen.TestConsoleInsightProjectRef
{
    internal class MyDemoService3Implementation : IMyDemoService3
    {
        public string DemoMethod3(int id)
        {
            return $"Hello {id}";
        }
    }
}
