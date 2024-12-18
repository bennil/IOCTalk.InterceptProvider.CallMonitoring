using System;
using CallMonitoringSourceGen.TestConsole.Interface;
using IOCTalk.InterceptProvider.CallMonitoring.Common;

namespace CallMonitoringSourceGen.TestConsole
{
 public class MyDemoService2CallMonProvAutoGen : IMyDemoService2, IDisposable, ICallMonitoringSource
 {

		private IMyDemoService2 interceptedService;
		private ICallMonitoringHost monitorHost;

		public MyDemoService2CallMonProvAutoGen(IMyDemoService2 interceptedService, ICallMonitoringHost monitorHost)
		{
			this.interceptedService = interceptedService;
			this.monitorHost = monitorHost;
			monitorHost.RegisterSource(this);
		}

		public object InterceptedServiceObject => interceptedService;

		public System.Collections.ICollection GetCollection(Int32 number)
		{
			System.Collections.ICollection result = default;
			try
			{
			    Interlocked.Increment(ref getCollection_int_InvokeCount);
				result = interceptedService.GetCollection(number);
			}
			catch (Exception ex)
			{
			    Interlocked.Increment(ref getCollection_int_ExceptionCount);
			    this.lastException = ex;
			    throw;
			}
			finally
			{
			    Interlocked.Increment(ref getCollection_int_InvokeCompletedCount);
			}
			return result;
		}

		public async System.Threading.Tasks.Task ExecuteAsync(String test)
		{
			try
			{
			    Interlocked.Increment(ref executeAsync_string_InvokeCount);
				await interceptedService.ExecuteAsync(test);
			}
			catch (Exception ex)
			{
			    Interlocked.Increment(ref executeAsync_string_ExceptionCount);
			    this.lastException = ex;
			    throw;
			}
			finally
			{
			    Interlocked.Increment(ref executeAsync_string_InvokeCompletedCount);
			}
		}

		public async System.Threading.Tasks.Task<String> GetDataAsync(String test)
		{
			String result = default;
			try
			{
			    Interlocked.Increment(ref getDataAsync_string_InvokeCount);
				result = await interceptedService.GetDataAsync(test);
			}
			catch (Exception ex)
			{
			    Interlocked.Increment(ref getDataAsync_string_ExceptionCount);
			    this.lastException = ex;
			    throw;
			}
			finally
			{
			    Interlocked.Increment(ref getDataAsync_string_InvokeCompletedCount);
			}
			return result;
		}

		Exception? lastException;

		string getCollection_int_MethodName = "GetCollection(int number)";
		long getCollection_int_InvokeCount;
		long getCollection_int_InvokeCompletedCount;
		int getCollection_int_ExceptionCount;

		string executeAsync_string_MethodName = "ExecuteAsync(string test)";
		long executeAsync_string_InvokeCount;
		long executeAsync_string_InvokeCompletedCount;
		int executeAsync_string_ExceptionCount;

		string getDataAsync_string_MethodName = "GetDataAsync(string test)";
		long getDataAsync_string_InvokeCount;
		long getDataAsync_string_InvokeCompletedCount;
		int getDataAsync_string_ExceptionCount;


		public Type MonitoringInterface => typeof(IMyDemoService2);
		public Exception LastException => this.lastException;

		public IEnumerable<(string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount)> GetCallMonitoringSnapshot()
		{
			yield return (getCollection_int_MethodName, getCollection_int_InvokeCount, getCollection_int_InvokeCompletedCount, getCollection_int_ExceptionCount);
			yield return (executeAsync_string_MethodName, executeAsync_string_InvokeCount, executeAsync_string_InvokeCompletedCount, executeAsync_string_ExceptionCount);
			yield return (getDataAsync_string_MethodName, getDataAsync_string_InvokeCount, getDataAsync_string_InvokeCompletedCount, getDataAsync_string_ExceptionCount);
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
