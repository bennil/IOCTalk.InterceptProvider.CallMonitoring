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
                html.AppendLine("<table>");
                html.AppendLine("<tr>");
                html.AppendLine("<th>Service</th>");
                html.AppendLine("<th>Instance Count</th>");
                html.AppendLine("<th>Invoke Count</th>");
                html.AppendLine("<th>Invoke Completed Count</th>");
                html.AppendLine("<th>Invoke Pending</th>");
                html.AppendLine("<th>Exception Count</th>");
                html.AppendLine("</tr>");

                foreach (var item in callMonitoringHost.MonitorSources)
                {
                    var snapshot = item.GetCallMonitoringSnapshot();

                    var invokeCount = snapshot.Sum(s => s.InvokeCount);
                    var invokeCompletedCount = snapshot.Sum(s => s.InvokeCompletedCount);
                    var exceptionCount = snapshot.Sum(s => s.ExceptionCount);

                    html.AppendLine("<tr>");
                    html.AppendLine($"<td><a href=\"?{QueryNameService}={item.MonitoringInterface.FullName}\">{item.MonitoringInterface.FullName}</a></td>");
                    html.AppendLine($"<td>{item.SourceItems.Count}</td>");
                    html.AppendLine($"<td>{invokeCount}</td>");
                    html.AppendLine($"<td>{invokeCompletedCount}</td>");
                    html.AppendLine($"<td>{invokeCount - invokeCompletedCount}</td>");
                    html.AppendLine($"<td>{exceptionCount}</td>");
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
                            html.AppendLine("<th>Exception Count</th>");
                            html.AppendLine("</tr>");

                            foreach (var item in snapshot)
                            {
                                html.AppendLine("<tr>");
                                html.AppendLine($"<td>{item.MethodName}</td>");
                                html.AppendLine($"<td>{item.InvokeCount}</td>");
                                html.AppendLine($"<td>{item.InvokeCompletedCount}</td>");
                                html.AppendLine($"<td>{item.InvokeCount - item.InvokeCompletedCount}</td>");
                                html.AppendLine($"<td>{item.ExceptionCount}</td>");
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
                        html.AppendLine("<th>Exception Count</th>");
                        html.AppendLine("</tr>");


                        foreach (var srcItem in sourceContainer.SourceItems)
                        {
                                var snapshot = srcItem.GetCallMonitoringSnapshot();

                                var invokeCount = snapshot.Sum(s => s.InvokeCount);
                                var invokeCompletedCount = snapshot.Sum(s => s.InvokeCompletedCount);
                                var exceptionCount = snapshot.Sum(s => s.ExceptionCount);

                                html.AppendLine("<tr>");
                                html.AppendLine($"<td><a href=\"?{QueryNameService}={srcItem.MonitoringInterface.FullName}&{QueryNameInstanceId}={srcItem.InterceptedServiceObject.GetHashCode()}\">{srcItem.InterceptedServiceObject.GetType().FullName}</a></td>");
                                html.AppendLine($"<td>{invokeCount}</td>");
                                html.AppendLine($"<td>{invokeCompletedCount}</td>");
                                html.AppendLine($"<td>{invokeCount - invokeCompletedCount}</td>");
                                html.AppendLine($"<td>{exceptionCount}</td>");
                                html.AppendLine("</tr>");
                        }

                        html.AppendLine("</table>");
                    }

                }
            }
            resp.HtmlData = html.ToString();


            return ValueTask.FromResult<IInsightResponse?>(resp);
        }
    }
}
