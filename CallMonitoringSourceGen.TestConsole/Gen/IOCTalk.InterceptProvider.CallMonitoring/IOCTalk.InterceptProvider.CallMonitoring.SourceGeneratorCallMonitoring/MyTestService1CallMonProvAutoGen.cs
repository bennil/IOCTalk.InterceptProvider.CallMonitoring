using System;
using System.Collections.Generic;
using System.Threading;
using CallMonitoringSourceGen.TestConsole.Interface;
using System.Diagnostics;
using IOCTalk.InterceptProvider.CallMonitoring.Common;

namespace CallMonitoringSourceGen.TestConsole
{
 public class MyTestService1CallMonProvAutoGen : IMyTestService1, IDisposable, ICallMonitoringSource
 {

		private IMyTestService1 interceptedService;
		private ICallMonitoringHost monitorHost;

		public MyTestService1CallMonProvAutoGen(IMyTestService1 interceptedService, ICallMonitoringHost monitorHost)
		{
			this.interceptedService = interceptedService;
			this.monitorHost = monitorHost;
			monitorHost.RegisterSource(this);
		}

		public object InterceptedServiceObject => interceptedService;

		public void SayHello(Int32 id)
		{
			long ticksStart = Stopwatch.GetTimestamp();
			try
			{
			    Interlocked.Increment(ref sayHello_1226284721_InvokeCount);
				interceptedService.SayHello(id);
			}
			catch (Exception ex)
			{
			    Interlocked.Increment(ref sayHello_1226284721_ExceptionCount);
			    this.lastException = ex;
			    throw;
			}
			finally
			{
			    long ticksEnd = Stopwatch.GetTimestamp();
			    Interlocked.Increment(ref sayHello_1226284721_InvokeCompletedCount);
			    long ticksDuration = ticksEnd - ticksStart;
			    Interlocked.Add(ref sayHello_1226284721_ExecTimeTotal, ticksDuration);
			    if (sayHello_1226284721_ExecTimeMax < ticksDuration)
			         sayHello_1226284721_ExecTimeMax = ticksDuration;
			}
		}

		Exception? lastException;

		string sayHello_1226284721_MethodName = "SayHello(int id)";
		long sayHello_1226284721_InvokeCount;
		long sayHello_1226284721_InvokeCompletedCount;
		int sayHello_1226284721_ExceptionCount;
		long sayHello_1226284721_ExecTimeTotal;
		long sayHello_1226284721_ExecTimeMax;


		public Type MonitoringInterface => typeof(IMyTestService1);
		public Exception LastException => this.lastException;

		public IEnumerable<(string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount, long ExecTimeTotal, long ExecTimeMax)> GetCallMonitoringSnapshot()
		{
			yield return (sayHello_1226284721_MethodName, sayHello_1226284721_InvokeCount, sayHello_1226284721_InvokeCompletedCount, sayHello_1226284721_ExceptionCount, sayHello_1226284721_ExecTimeTotal, sayHello_1226284721_ExecTimeMax);
		}

		public void Dispose()
		{
			if (interceptedService is IDisposable interceptedDisposable)
			{
			   interceptedDisposable.Dispose();
			}
			monitorHost.UnregisterSource(this);
		}
 }
}
