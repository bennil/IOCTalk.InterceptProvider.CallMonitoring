using CallMonitoringSourceGen.TestConsole.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMonitoringSourceGen.TestConsole
{
    internal class MainMyDemoService2 : IMyDemoService2
    {
        public async Task ExecuteAsync(string test)
        {
        }

        public ICollection GetCollection(int number, out int x)
        {
            x = 0;
            List<int> test = new List<int>();
            test.Add(number);
            return test;
        }

        public Task<string> GetDataAsync(string test)
        {
            return Task<string>.FromResult(test);
        }

        public Task<IDataItem> ModifyData(IDataItem dataItem)
        {
            return Task.FromResult(dataItem);
        }
    }
}
