using BenchmarkDotNet.Running;

namespace CallMonitoringSourceGen.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Benchmarks");

            var summary = BenchmarkRunner.Run<TimeMeasureBenchmark>();

        }
    }
}
