using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMonitoringSourceGen.Benchmark
{
    //[MediumRunJob, SkewnessColumn, KurtosisColumn, MemoryDiagnoser]
    [AllStatisticsColumn]
    [MemoryDiagnoser]
    public class TimeMeasureBenchmark
    {
        [Benchmark(Baseline = true)]
        public long WithoutMeasurement()
        {
            BurnCpu();

            return 0;
        }

        

        [Benchmark]
        public long UseStopwatch()
        {
            var st = Stopwatch.StartNew();

            BurnCpu();

            st.Stop();
            return st.ElapsedTicks;
        }

        [Benchmark()]
        public long UseStopwatchTicks()
        {
            var start = Stopwatch.GetTimestamp();

            BurnCpu();

            long end = Stopwatch.GetTimestamp();
            return end - start;
        }

        [Benchmark]
        public long UseDateTimeUtc()
        {
            long start = DateTime.UtcNow.Ticks;
            
            BurnCpu();

            long end = DateTime.UtcNow.Ticks;
            long diff = end - start;
            return diff;
        }


        void BurnCpu()
        {
            for (int i = 0; i < 1000; i++)
            {
                int x = i * i;
            }
        }
    }


}
