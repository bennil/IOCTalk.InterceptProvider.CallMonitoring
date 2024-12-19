using System;
using System.Collections.Generic;
using System.Threading;
using CallMonitoringSourceGen.TestConsole.Interface;
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
			    Interlocked.Increment(ref sayHello_1226284721_InvokeCompletedCount);
			}
		}

		Exception? lastException;

		string sayHello_1226284721_MethodName = "SayHello(int id)";
		long sayHello_1226284721_InvokeCount;
		long sayHello_1226284721_InvokeCompletedCount;
		int sayHello_1226284721_ExceptionCount;


		public Type MonitoringInterface => typeof(IMyTestService1);
		public Exception LastException => this.lastException;

		public IEnumerable<(string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount)> GetCallMonitoringSnapshot()
		{
			yield return (sayHello_1226284721_MethodName, sayHello_1226284721_InvokeCount, sayHello_1226284721_InvokeCompletedCount, sayHello_1226284721_ExceptionCount);
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
