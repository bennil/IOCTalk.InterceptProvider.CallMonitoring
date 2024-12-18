using System;
using System.Collections.Generic;
using System.Text;

namespace CallMonitoringSourceGen.TestConsole
{
    public static class InterceptCallMonitoringProvider<InterfaceType>
        where InterfaceType : class
    {
        static Type? interceptImplementationType = null;

        static InterceptCallMonitoringProvider() 
        {
            // Intercept implementation count: 2
			InterceptCallMonitoringProvider<CallMonitoringSourceGen.TestConsole.Interface.IMyTestService1>.interceptImplementationType = typeof(MyTestService1CallMonProvAutoGen);
			InterceptCallMonitoringProvider<CallMonitoringSourceGen.TestConsole.Interface.IMyDemoService2>.interceptImplementationType = typeof(MyDemoService2CallMonProvAutoGen);
        }
        public static Type GetInterceptType()
        {
            if (interceptImplementationType is null)
                throw new InvalidOperationException($"No auto generated intecept interface implementation assigned! Source generator did not execute properly? Epected implementation interface: {typeof(InterfaceType).FullName}");

            return interceptImplementationType;
        }
    }
}
