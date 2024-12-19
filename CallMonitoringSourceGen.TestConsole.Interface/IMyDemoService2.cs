using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CallMonitoringSourceGen.TestConsole.Interface
{
    public interface IMyDemoService2
    {
        ICollection GetCollection(int number, out int test);

        Task ExecuteAsync(string test);

        Task<string> GetDataAsync(string test);

        Task<IDataItem> ModifyData(IDataItem dataItem);

        //int OutTestMethod(int input, out int output2);
    }
}
