using BSAG.IOCTalk.Common.Interface.Session;
using GenHTTP.Engine.Internal;
using GenHTTP.Modules.IO;
using IOCTalk.Insight.Interface;
using IOCTalk.Insight.Interface.Handler;
using IOCTalk.Insight.Interface.Handler.Response;
using IOCTalk.Insight.WebHost.Common;
using IOCTalk.InterceptProvider.CallMonitoring.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCTalk.InterceptProvider.CallMonitoring.Insight
{
    public class CallMonitoringInsightModule : IInsightModule, IInsightHandler
    {
        ICallMonitoringHost callMonitoringHost;
        InsightResponseType[] supportedResponseTypes = { InsightResponseType.HtmlDefaultTemplate };
        public const string QueryNameService = "service";
        public const string QueryNameInstanceId = "instanceid";
        public const string QueryNameTopList = "top";

        public CallMonitoringInsightModule(ICallMonitoringHost callMonitoringHost)
        {
            this.callMonitoringHost = callMonitoringHost;
        }

        public string Route => "callmonitoring";

        public string Name => "Call Monitoring";

        public IInsightHandler Handler => this;

        public InsightResponseType[] SupportedResponseTypes => supportedResponseTypes;

        public ValueTask<IInsightResponse> HandleAsync(IInsightRequest request)
        {
            InsightHtmlResponse resp = new InsightHtmlResponse
            {
                ResponseType = InsightResponseType.HtmlDefaultTemplate,
                Title = "IOCTalk Call Monitoring",
            };

            StringBuilder html = new StringBuilder();
            if (request.Queries.Count == 0)
            {
                html.AppendLine($"<div><a href=\"?{QueryNameTopList}=10\">Top 10</a></div>");
                html.AppendLine($"<div><a href=\"?{QueryNameTopList}=50\">Top 50</a></div>");

                html.AppendLine("<table>");
                html.AppendLine("<tr>");
                html.AppendLine("<th>Service</th>");
                html.AppendLine("<th>Instance Count</th>");
                html.AppendLine("<th>Invoke Count</th>");
                //html.AppendLine("<th>Invoke Completed Count</th>");
                html.AppendLine("<th>Invoke Pending</th>");
                html.AppendLine("<th>Propagated Exceptions</th>");
                html.AppendLine("</tr>");

                foreach (var item in callMonitoringHost.MonitorSources)
                {
                    var snapshot = item.GetCallMonitoringSnapshot();

                    var invokeCount = snapshot.Sum(s => s.InvokeCount);
                    var invokeCompletedCount = snapshot.Sum(s => s.InvokeCompletedCount);
                    var exceptionCount = snapshot.Sum(s => s.ExceptionCount);

                    html.AppendLine("<tr>");
                    html.AppendLine($"<td><a href=\"?{QueryNameService}={item.MonitoringInterface.FullName}\">{item.MonitoringInterface.FullName}</a></td>");
                    html.AppendLine($"<td>{item.SourceItems.Count:N0}</td>");
                    html.AppendLine($"<td>{invokeCount:N0}</td>");
                    //html.AppendLine($"<td>{invokeCompletedCount}</td>");
                    html.AppendLine($"<td>{invokeCount - invokeCompletedCount:N0}</td>");
                    html.AppendLine($"<td>{exceptionCount:N0}</td>");
                    html.AppendLine("</tr>");
                }

                html.AppendLine("</table>");

            }
            else
            {
                string queryValue;
                if (request.Queries.TryGetValue(QueryNameService, out queryValue))
                {
                    var sourceContainer = callMonitoringHost.GetSourceByInterface(queryValue);

                    if (request.Queries.TryGetValue(QueryNameInstanceId, out string instanceIdStr))
                    {
                        int instanceId = int.Parse(instanceIdStr);
                        var source = sourceContainer.SourceItems.Where(si => si.InterceptedServiceObject.GetHashCode() == instanceId).FirstOrDefault();

                        if (source is null)
                        {
                            html.AppendLine($"<div>Could not find instance ID: {instanceId}</div>");
                        }
                        else
                        {
                            var snapshot = source.GetCallMonitoringSnapshot();

                            html.AppendLine($"<div>Service Interface: {source.MonitoringInterface.FullName}</div>");
                            html.AppendLine($"<div>Intercepted Service: {source.InterceptedServiceObject?.GetType().FullName}</div>");
                            html.AppendLine($"<div>Instance ID: {source.InterceptedServiceObject?.GetHashCode()}</div>");

                            if (source.InterceptedServiceObject is ISessionContext sessionContext)
                            {
                                html.AppendLine($"<div style=\"margin-top: 15px;\">Container Host: {sessionContext.CommunicationService.ContainerHost.Name}</div>");
                                html.AppendLine($"<div>Session: {sessionContext.Session.SessionId} - {sessionContext.Session.Description}</div>");
                            }

                            html.AppendLine("<table>");
                            html.AppendLine("<tr>");
                            html.AppendLine("<th>Method Name</th>");
                            html.AppendLine("<th>Invoke Count</th>");
                            html.AppendLine("<th>Invoke Completed Count</th>");
                            html.AppendLine("<th>Invoke Pending</th>");
                            html.AppendLine("<th>Propagated Exceptions</th>");
                            WriteExecTimeColumnsHeader(html);
                            html.AppendLine("</tr>");

                            foreach (var item in snapshot)
                            {
                                html.AppendLine("<tr>");
                                html.AppendLine($"<td>{item.MethodName}</td>");
                                html.AppendLine($"<td>{item.InvokeCount:N0}</td>");
                                html.AppendLine($"<td>{item.InvokeCompletedCount:N0}</td>");
                                html.AppendLine($"<td>{item.InvokeCount - item.InvokeCompletedCount:N0}</td>");
                                html.AppendLine($"<td>{item.ExceptionCount:N0}</td>");

                                WriteExecTimeColumns(html, item.ExecTimeTotal, item.ExecTimeMax, item.InvokeCompletedCount);

                                html.AppendLine("</tr>");
                            }

                            html.AppendLine("</table>");


                            if (source.LastException is not null)
                            {
                                html.AppendLine($"<div>Last Exception:<br /><pre>{source.LastException}</pre></div>");
                            }
                        }
                    }
                    else
                    {
                        html.AppendLine($"<div>Service Interface: {sourceContainer.MonitoringInterface.FullName}</div>");

                        html.AppendLine("<table>");
                        html.AppendLine("<tr>");
                        html.AppendLine("<th>Service Implementation</th>");
                        html.AppendLine("<th>Invoke Count</th>");
                        html.AppendLine("<th>Invoke Completed Count</th>");
                        html.AppendLine("<th>Invoke Pending</th>");
                        html.AppendLine("<th>Propagated Exceptions</th>");
                        WriteExecTimeColumnsHeader(html);
                        html.AppendLine("</tr>");


                        foreach (var srcItem in sourceContainer.SourceItems)
                        {
                            var snapshot = srcItem.GetCallMonitoringSnapshot();

                            var invokeCount = snapshot.Sum(s => s.InvokeCount);
                            var invokeCompletedCount = snapshot.Sum(s => s.InvokeCompletedCount);
                            var exceptionCount = snapshot.Sum(s => s.ExceptionCount);
                            var execTimeTotal = snapshot.Sum(s => s.ExecTimeTotal);
                            var execTimeMax = snapshot.Max(s => s.ExecTimeMax);

                            html.AppendLine("<tr>");
                            html.AppendLine($"<td><a href=\"?{QueryNameService}={srcItem.MonitoringInterface.FullName}&{QueryNameInstanceId}={srcItem.InterceptedServiceObject.GetHashCode()}\">{srcItem.InterceptedServiceObject.GetType().FullName}</a></td>");
                            html.AppendLine($"<td>{invokeCount:N0}</td>");
                            html.AppendLine($"<td>{invokeCompletedCount:N0}</td>");
                            html.AppendLine($"<td>{invokeCount - invokeCompletedCount:N0}</td>");
                            html.AppendLine($"<td>{exceptionCount:N0}</td>");

                            WriteExecTimeColumns(html, execTimeTotal, execTimeMax, invokeCompletedCount);

                            html.AppendLine("</tr>");
                        }

                        html.AppendLine("</table>");
                    }

                }
                else if (request.Queries.TryGetValue(QueryNameTopList, out string topCountStr))
                {
                    int topCount = int.Parse(topCountStr);

                    // Top list aggregation
                    List<TopAggregate<long>> topInvokeList = new(topCount + 1);
                    List<TopAggregate<long>> topExceptionList = new(topCount + 1);
                    List<TopAggregate<long>> topAvgExecTimeList = new(topCount + 1);
                    List<TopAggregate<long>> topMaxExecTimeList = new(topCount + 1);
                    List<TopAggregate<long>> topTotalExecTimeList = new(topCount + 1);

                    foreach (var monitorSrc in callMonitoringHost.MonitorSources)
                    {
                        var snapshot = monitorSrc.GetCallMonitoringSnapshot();

                        foreach (var si in snapshot)
                        {
                            ProcessTopAggregate<long>(topCount, topInvokeList, monitorSrc, si, si.InvokeCount);
                            ProcessTopAggregate<long>(topCount, topExceptionList, monitorSrc, si, si.ExceptionCount);

                            long avgExecTimeTicks = CalculateAverageExecTime(si.ExecTimeTotal, si.InvokeCompletedCount);
                            ProcessTopAggregate<long>(topCount, topAvgExecTimeList, monitorSrc, si, avgExecTimeTicks);

                            ProcessTopAggregate<long>(topCount, topMaxExecTimeList, monitorSrc, si, si.ExecTimeMax);
                            ProcessTopAggregate<long>(topCount, topTotalExecTimeList, monitorSrc, si, si.ExecTimeTotal);
                        }
                    }

                    OutputTopList(html, topCount, "Method Invoke", "Invoke Count", topInvokeList);
                    OutputTopList(html, topCount, "Propagated Exception Count", "Exception Count", topExceptionList);
                    OutputTopList(html, topCount, "Average Execution Time", "Exec. Time Avg.", topAvgExecTimeList, v => TimeSpan.FromTicks(v).ToString());
                    OutputTopList(html, topCount, "Max. Execution Time", "Max. Time", topMaxExecTimeList, v => TimeSpan.FromTicks(v).ToString());
                    OutputTopList(html, topCount, "Total Execution Time", "Total Time", topTotalExecTimeList, v => TimeSpan.FromTicks(v).ToString());
                }
            }
            resp.HtmlData = html.ToString();


            return ValueTask.FromResult<IInsightResponse>(resp);
        }

        private static void OutputTopList<ValueType>(StringBuilder html, int topCount, string topListName, string valueLabel, List<TopAggregate<ValueType>> topList, Func<ValueType, string>? optionalValueFormater = null)
                    where ValueType : struct
        {
            topList.Reverse();

            // top list output
            html.AppendLine($"<h3>Top {topCount} - {topListName}</h3>");

            if (topList.Count == 0)
            {
                html.AppendLine($"<div>No current utilization for top list generation found.</div>");
            }
            else
            {
                html.AppendLine("<table>");
                html.AppendLine("<tr>");
                html.AppendLine("<th>Service Method</th>");
                html.AppendLine($"<th>{valueLabel}</th>");
                html.AppendLine("</tr>");

                foreach (var item in topList)
                {
                    var srcItem = item.Value;

                    html.AppendLine("<tr>");
                    html.AppendLine($"<td><a href=\"?{QueryNameService}={item.InterfaceType.FullName}\">{item.InterfaceType.FullName}</a> {item.ServiceMethod}</td>");
                    if (optionalValueFormater is null)
                        html.AppendLine($"<td>{item.Value:N0}</td>");
                    else
                        html.AppendLine($"<td>{optionalValueFormater(item.Value)}</td>");

                    html.AppendLine("</tr>");
                }

                html.AppendLine("</table>");
            }

            // spacing
            html.AppendLine("<div style=\"margin-bottom: 10px;\">&nbsp;</div>");
        }

        private static void ProcessTopAggregate<ValueType>(int topCount, List<TopAggregate<ValueType>> topInvokeList, ICallMonitoringSourceContainer monitorSrc, (string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount, long ExecTimeTotal, long ExecTimeMax) si, ValueType value)
                    where ValueType : struct
        {
            if (value is long longVal 
                && longVal == 0
                || value is int intVal
                && intVal == 0)
            {
                return; // ignore zero values
            }

            var topAggr = new TopAggregate<ValueType> { InterfaceType = monitorSrc.MonitoringInterface, ServiceMethod = si.MethodName, Value = value };
            int targetIndex = topInvokeList.BinarySearch(topAggr, TopAggregate<ValueType>.ValueComparer);

            if (targetIndex < 0)
            {
                topInvokeList.Insert(~targetIndex, topAggr);
            }
            else
            {
                bool isTopListFull = topInvokeList.Count >= topCount;
                bool isLowest = targetIndex == 0;
                bool isOutsideToplistValues = isLowest == true && isTopListFull == true;

                if (isOutsideToplistValues == false)
                    topInvokeList.Insert(targetIndex, topAggr);
            }

            if (topInvokeList.Count > topCount)
                topInvokeList.RemoveAt(0);  // remove previous lower value item
        }

        private static void WriteExecTimeColumnsHeader(StringBuilder html)
        {
            html.AppendLine("<th>Avg. Execution Time</th>");
            html.AppendLine("<th>Max. Exec. Time</th>");
            html.AppendLine("<th>Total Exec. Time</th>");
        }

        private static void WriteExecTimeColumns(StringBuilder html, long execTimeTotal, long execTimeMax, long invokeCompletedCount)
        {
            long ticksPerExec = CalculateAverageExecTime(execTimeTotal, invokeCompletedCount);

            TimeSpan avgExecTime = TimeSpan.FromTicks(ticksPerExec);
            html.AppendLine($"<td>{avgExecTime}</td>");
            html.AppendLine($"<td>{TimeSpan.FromTicks(execTimeMax)}</td>");
            html.AppendLine($"<td>{TimeSpan.FromTicks(execTimeTotal)}</td>");
        }

        private static long CalculateAverageExecTime(long execTimeTotal, long invokeCompletedCount)
        {
            long ticksPerExec = 0;
            if (execTimeTotal > 0)
                ticksPerExec = execTimeTotal / invokeCompletedCount;
            return ticksPerExec;
        }
    }
}
