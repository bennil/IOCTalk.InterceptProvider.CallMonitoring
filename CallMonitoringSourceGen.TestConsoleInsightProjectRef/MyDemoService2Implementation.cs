using CallMonitoringSourceGen.TestConsole.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMonitoringSourceGen.TestConsoleInsightProjectRef
{
    internal class MyDemoService2Implementation : IMyDemoService2
    {
        public async Task ExecuteAsync(string test)
        {
            if (test == "long-running")
            {
                await Task.Delay(3000);
            }
            else if (test == "throw-exception")
                throw new InvalidOperationException("Demo exception");
        }

        public ICollection GetCollection(int number, out int test)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDataAsync(string test)
        {
            throw new NotImplementedException();
        }

        public Task<IDataItem> ModifyData(IDataItem dataItem)
        {
            throw new NotImplementedException();
        }
    }
}
