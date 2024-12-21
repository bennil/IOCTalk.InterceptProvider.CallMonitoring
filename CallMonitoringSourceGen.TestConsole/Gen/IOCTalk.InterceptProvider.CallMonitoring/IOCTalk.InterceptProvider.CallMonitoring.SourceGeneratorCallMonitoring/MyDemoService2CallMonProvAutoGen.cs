using System;
using System.Collections.Generic;
using System.Threading;
using CallMonitoringSourceGen.TestConsole.Interface;
using System.Diagnostics;
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

		public System.Collections.ICollection GetCollection(Int32 number, out Int32 test)
		{
			System.Collections.ICollection result = default;
			long ticksStart = Stopwatch.GetTimestamp();
			try
			{
			    Interlocked.Increment(ref getCollection_1482904633_InvokeCount);
				result = interceptedService.GetCollection(number, out test);
			}
			catch (Exception ex)
			{
			    Interlocked.Increment(ref getCollection_1482904633_ExceptionCount);
			    this.lastException = ex;
			    throw;
			}
			finally
			{
			    long ticksEnd = Stopwatch.GetTimestamp();
			    Interlocked.Increment(ref getCollection_1482904633_InvokeCompletedCount);
			    long ticksDuration = ticksEnd - ticksStart;
			    Interlocked.Add(ref getCollection_1482904633_ExecTimeTotal, ticksDuration);
			    if (getCollection_1482904633_ExecTimeMax < ticksDuration)
			         getCollection_1482904633_ExecTimeMax = ticksDuration;
			}
			return result;
		}

		public async System.Threading.Tasks.Task ExecuteAsync(String test)
		{
			long ticksStart = Stopwatch.GetTimestamp();
			try
			{
			    Interlocked.Increment(ref executeAsync_983953243_InvokeCount);
				await interceptedService.ExecuteAsync(test);
			}
			catch (Exception ex)
			{
			    Interlocked.Increment(ref executeAsync_983953243_ExceptionCount);
			    this.lastException = ex;
			    throw;
			}
			finally
			{
			    long ticksEnd = Stopwatch.GetTimestamp();
			    Interlocked.Increment(ref executeAsync_983953243_InvokeCompletedCount);
			    long ticksDuration = ticksEnd - ticksStart;
			    Interlocked.Add(ref executeAsync_983953243_ExecTimeTotal, ticksDuration);
			    if (executeAsync_983953243_ExecTimeMax < ticksDuration)
			         executeAsync_983953243_ExecTimeMax = ticksDuration;
			}
		}

		public async System.Threading.Tasks.Task<String> GetDataAsync(String test)
		{
			String result = default;
			long ticksStart = Stopwatch.GetTimestamp();
			try
			{
			    Interlocked.Increment(ref getDataAsync_983953243_InvokeCount);
				result = await interceptedService.GetDataAsync(test);
			}
			catch (Exception ex)
			{
			    Interlocked.Increment(ref getDataAsync_983953243_ExceptionCount);
			    this.lastException = ex;
			    throw;
			}
			finally
			{
			    long ticksEnd = Stopwatch.GetTimestamp();
			    Interlocked.Increment(ref getDataAsync_983953243_InvokeCompletedCount);
			    long ticksDuration = ticksEnd - ticksStart;
			    Interlocked.Add(ref getDataAsync_983953243_ExecTimeTotal, ticksDuration);
			    if (getDataAsync_983953243_ExecTimeMax < ticksDuration)
			         getDataAsync_983953243_ExecTimeMax = ticksDuration;
			}
			return result;
		}

		public async System.Threading.Tasks.Task<CallMonitoringSourceGen.TestConsole.Interface.IDataItem> ModifyData(CallMonitoringSourceGen.TestConsole.Interface.IDataItem dataItem)
		{
			CallMonitoringSourceGen.TestConsole.Interface.IDataItem result = default;
			long ticksStart = Stopwatch.GetTimestamp();
			try
			{
			    Interlocked.Increment(ref modifyData_229559583_InvokeCount);
				result = await interceptedService.ModifyData(dataItem);
			}
			catch (Exception ex)
			{
			    Interlocked.Increment(ref modifyData_229559583_ExceptionCount);
			    this.lastException = ex;
			    throw;
			}
			finally
			{
			    long ticksEnd = Stopwatch.GetTimestamp();
			    Interlocked.Increment(ref modifyData_229559583_InvokeCompletedCount);
			    long ticksDuration = ticksEnd - ticksStart;
			    Interlocked.Add(ref modifyData_229559583_ExecTimeTotal, ticksDuration);
			    if (modifyData_229559583_ExecTimeMax < ticksDuration)
			         modifyData_229559583_ExecTimeMax = ticksDuration;
			}
			return result;
		}

		Exception? lastException;

		string getCollection_1482904633_MethodName = "GetCollection(int number, int test)";
		long getCollection_1482904633_InvokeCount;
		long getCollection_1482904633_InvokeCompletedCount;
		int getCollection_1482904633_ExceptionCount;
		long getCollection_1482904633_ExecTimeTotal;
		long getCollection_1482904633_ExecTimeMax;

		string executeAsync_983953243_MethodName = "ExecuteAsync(string test)";
		long executeAsync_983953243_InvokeCount;
		long executeAsync_983953243_InvokeCompletedCount;
		int executeAsync_983953243_ExceptionCount;
		long executeAsync_983953243_ExecTimeTotal;
		long executeAsync_983953243_ExecTimeMax;

		string getDataAsync_983953243_MethodName = "GetDataAsync(string test)";
		long getDataAsync_983953243_InvokeCount;
		long getDataAsync_983953243_InvokeCompletedCount;
		int getDataAsync_983953243_ExceptionCount;
		long getDataAsync_983953243_ExecTimeTotal;
		long getDataAsync_983953243_ExecTimeMax;

		string modifyData_229559583_MethodName = "ModifyData(CallMonitoringSourceGen.TestConsole.Interface.IDataItem dataItem)";
		long modifyData_229559583_InvokeCount;
		long modifyData_229559583_InvokeCompletedCount;
		int modifyData_229559583_ExceptionCount;
		long modifyData_229559583_ExecTimeTotal;
		long modifyData_229559583_ExecTimeMax;


		public Type MonitoringInterface => typeof(IMyDemoService2);
		public Exception LastException => this.lastException;

		public IEnumerable<(string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount, long ExecTimeTotal, long ExecTimeMax)> GetCallMonitoringSnapshot()
		{
			yield return (getCollection_1482904633_MethodName, getCollection_1482904633_InvokeCount, getCollection_1482904633_InvokeCompletedCount, getCollection_1482904633_ExceptionCount, getCollection_1482904633_ExecTimeTotal, getCollection_1482904633_ExecTimeMax);
			yield return (executeAsync_983953243_MethodName, executeAsync_983953243_InvokeCount, executeAsync_983953243_InvokeCompletedCount, executeAsync_983953243_ExceptionCount, executeAsync_983953243_ExecTimeTotal, executeAsync_983953243_ExecTimeMax);
			yield return (getDataAsync_983953243_MethodName, getDataAsync_983953243_InvokeCount, getDataAsync_983953243_InvokeCompletedCount, getDataAsync_983953243_ExceptionCount, getDataAsync_983953243_ExecTimeTotal, getDataAsync_983953243_ExecTimeMax);
			yield return (modifyData_229559583_MethodName, modifyData_229559583_InvokeCount, modifyData_229559583_InvokeCompletedCount, modifyData_229559583_ExceptionCount, modifyData_229559583_ExecTimeTotal, modifyData_229559583_ExecTimeMax);
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
