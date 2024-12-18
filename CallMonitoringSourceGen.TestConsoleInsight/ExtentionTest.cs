using BSAG.IOCTalk.Composition.Fluent;
using CallMonitoringSourceGen.TestConsole.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMonitoringSourceGen.TestConsoleInsight
{
    public static class ExtentionTest
    {
        public static LocalSharedRegistration<InterfaceType> InterceptWithSourceGenProvider<InterfaceType, SourceGenProvider>(this LocalSharedRegistration<InterfaceType> source)
            where InterfaceType : class
        {
            //source.InterceptWithImplementation(InterceptCallMonitoringProvider<InterfaceType>.GetInterceptType());

            return source;
        }
    }
}
