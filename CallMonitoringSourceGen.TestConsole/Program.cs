using CallMonitoringSourceGen.TestConsole.Interface;

namespace CallMonitoringSourceGen.TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start SourceGen Test");

            var type = InterceptCallMonitoringProvider<IMyTestService1>.GetInterceptType();
            Console.WriteLine($"Auto gen Type: {type}");

            var typex = InterceptCallMonitoringProvider<IMyTestService1>.GetInterceptType();

            var type2 = InterceptCallMonitoringProvider<IMyDemoService2>.GetInterceptType();
            Console.WriteLine($"Auto gen Type: {type2}");

            var mainImplementation = new MainMyDemoService2();

            var instance2 = (IMyDemoService2)Activator.CreateInstance(type2, mainImplementation);

            instance2.GetCollection(5);

            Console.WriteLine("done");
        }
    }
}
