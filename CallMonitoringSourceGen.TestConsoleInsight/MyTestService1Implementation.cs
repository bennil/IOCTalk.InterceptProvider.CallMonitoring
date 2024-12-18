using CallMonitoringSourceGen.TestConsole.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMonitoringSourceGen.TestConsoleInsight
{
    internal class MyTestService1Implementation : IMyTestService1
    {
        public void SayHello(int id)
        {
            Console.WriteLine($"Say Hello: {id}");
        }
    }
}
