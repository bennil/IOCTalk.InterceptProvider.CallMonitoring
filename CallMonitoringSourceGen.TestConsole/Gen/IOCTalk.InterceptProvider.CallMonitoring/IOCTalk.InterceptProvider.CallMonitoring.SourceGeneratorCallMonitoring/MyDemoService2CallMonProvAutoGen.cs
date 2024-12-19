using System;
using System.Collections.Generic;
using System.Threading;
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

		public System.Collections.ICollection GetCollection(Int32 number, out Int32 test)
		{
			System.Collections.ICollection result = default;
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
			    Interlocked.Increment(ref getCollection_1482904633_InvokeCompletedCount);
			}
			return result;
		}

		public async System.Threading.Tasks.Task ExecuteAsync(String test)
		{
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
			    Interlocked.Increment(ref executeAsync_983953243_InvokeCompletedCount);
			}
		}

		public async System.Threading.Tasks.Task<String> GetDataAsync(String test)
		{
			String result = default;
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
			    Interlocked.Increment(ref getDataAsync_983953243_InvokeCompletedCount);
			}
			return result;
		}

		public async System.Threading.Tasks.Task<CallMonitoringSourceGen.TestConsole.Interface.IDataItem> ModifyData(CallMonitoringSourceGen.TestConsole.Interface.IDataItem dataItem)
		{
			CallMonitoringSourceGen.TestConsole.Interface.IDataItem result = default;
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
			    Interlocked.Increment(ref modifyData_229559583_InvokeCompletedCount);
			}
			return result;
		}

		Exception? lastException;

		string getCollection_1482904633_MethodName = "GetCollection(int number, int test)";
		long getCollection_1482904633_InvokeCount;
		long getCollection_1482904633_InvokeCompletedCount;
		int getCollection_1482904633_ExceptionCount;

		string executeAsync_983953243_MethodName = "ExecuteAsync(string test)";
		long executeAsync_983953243_InvokeCount;
		long executeAsync_983953243_InvokeCompletedCount;
		int executeAsync_983953243_ExceptionCount;

		string getDataAsync_983953243_MethodName = "GetDataAsync(string test)";
		long getDataAsync_983953243_InvokeCount;
		long getDataAsync_983953243_InvokeCompletedCount;
		int getDataAsync_983953243_ExceptionCount;

		string modifyData_229559583_MethodName = "ModifyData(CallMonitoringSourceGen.TestConsole.Interface.IDataItem dataItem)";
		long modifyData_229559583_InvokeCount;
		long modifyData_229559583_InvokeCompletedCount;
		int modifyData_229559583_ExceptionCount;


		public Type MonitoringInterface => typeof(IMyDemoService2);
		public Exception LastException => this.lastException;

		public IEnumerable<(string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount)> GetCallMonitoringSnapshot()
		{
			yield return (getCollection_1482904633_MethodName, getCollection_1482904633_InvokeCount, getCollection_1482904633_InvokeCompletedCount, getCollection_1482904633_ExceptionCount);
			yield return (executeAsync_983953243_MethodName, executeAsync_983953243_InvokeCount, executeAsync_983953243_InvokeCompletedCount, executeAsync_983953243_ExceptionCount);
			yield return (getDataAsync_983953243_MethodName, getDataAsync_983953243_InvokeCount, getDataAsync_983953243_InvokeCompletedCount, getDataAsync_983953243_ExceptionCount);
			yield return (modifyData_229559583_MethodName, modifyData_229559583_InvokeCount, modifyData_229559583_InvokeCompletedCount, modifyData_229559583_ExceptionCount);
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
