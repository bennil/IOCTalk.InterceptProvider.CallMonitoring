using IOCTalk.InterceptProvider.SourceGenBase;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace IOCTalk.InterceptProvider.CallMonitoring
{
    [Generator]
    public class SourceGeneratorCallMonitoring : AbstractInterceptSourceGenerator
    {
        public SourceGeneratorCallMonitoring()
        {
#if DEBUG
            // only when referenced lib is debug compiled
            this.attachDebugger = false;
            this.isVerboseLogging = false;
#endif
        }

        public override string InterceptProviderName => "InterceptCallMonitoringProvider";

        public override string InterceptProviderShortName => "CallMonProv";

        protected override void AppendInterceptUsings(StringBuilder mainSource, ITypeSymbol interfaceType)
        {
            mainSource.AppendLine("using System.Diagnostics;");
            mainSource.AppendLine("using IOCTalk.InterceptProvider.CallMonitoring.Common;");
        }

        protected override void AppendInterceptInheritance(StringBuilder mainSource, ITypeSymbol interfaceType)
        {
            mainSource.Append(", ICallMonitoringSource");
        }

        protected override void AppendInterceptConstructorParameter(StringBuilder source, ITypeSymbol interfaceType)
        {
            source.Append(", ICallMonitoringHost monitorHost");
        }

        protected override void AppendInterceptConstructorImplementation(StringBuilder source, ITypeSymbol interfaceType)
        {
            source.Append(MethodLineIntention);
            source.AppendLine("monitorHost.RegisterSource(this);");
        }

        protected override void AppendInterceptMethodBeforeNestedCall(StringBuilder methodSource, ITypeSymbol interfaceType, IMethodSymbol method)
        {
            methodSource.AppendLine($"{MethodLineIntention}long timestampStart = Stopwatch.GetTimestamp();");

            methodSource.AppendLine($"{MethodLineIntention}try");
            methodSource.AppendLine($"{MethodLineIntention}{{");
            methodSource.AppendLine($"{MethodLineIntention}    Interlocked.Increment(ref {GetMonitorFieldName(method, MonitoringFieldType.InvokeCount)});");
        }

        protected override void AppendInterceptMethodAfterNestedCall(StringBuilder methodSource, ITypeSymbol interfaceType, IMethodSymbol method)
        {
            methodSource.AppendLine($"{MethodLineIntention}}}");
            methodSource.AppendLine($"{MethodLineIntention}catch (Exception ex)");
            methodSource.AppendLine($"{MethodLineIntention}{{");
            methodSource.AppendLine($"{MethodLineIntention}    Interlocked.Increment(ref {GetMonitorFieldName(method, MonitoringFieldType.ExceptionCount)});");
            methodSource.AppendLine($"{MethodLineIntention}    this.lastException = ex;");
            methodSource.AppendLine($"{MethodLineIntention}    throw;");
            methodSource.AppendLine($"{MethodLineIntention}}}");
            methodSource.AppendLine($"{MethodLineIntention}finally");
            methodSource.AppendLine($"{MethodLineIntention}{{");
            methodSource.AppendLine($"{MethodLineIntention}    TimeSpan duration = Stopwatch.GetElapsedTime(timestampStart);");
            methodSource.AppendLine($"{MethodLineIntention}    Interlocked.Increment(ref {GetMonitorFieldName(method, MonitoringFieldType.InvokeCompletedCount)});");
            methodSource.AppendLine($"{MethodLineIntention}    Interlocked.Add(ref {GetMonitorFieldName(method, MonitoringFieldType.ExecTimeTotal)}, duration.Ticks);");
            methodSource.AppendLine($"{MethodLineIntention}    if ({GetMonitorFieldName(method, MonitoringFieldType.ExecTimeMax)} < duration.Ticks)");
            methodSource.AppendLine($"{MethodLineIntention}         {GetMonitorFieldName(method, MonitoringFieldType.ExecTimeMax)} = duration.Ticks;");
            methodSource.AppendLine($"{MethodLineIntention}}}");
        }


        protected override void AppendInterceptCodeAfterMethods(StringBuilder mainSource, ITypeSymbol interfaceType, List<IMethodSymbol> methods)
        {
            mainSource.AppendLine($"{MethodBodyIntention}Exception? lastException;");
            mainSource.AppendLine();
            
            foreach (var m in methods)
            {
                mainSource.AppendLine($"{MethodBodyIntention}string {GetMonitorFieldName(m, MonitoringFieldType.MethodName)} = \"{GetMethodSignatureWithoutNamespace(m)}\";");
                mainSource.AppendLine($"{MethodBodyIntention}long {GetMonitorFieldName(m, MonitoringFieldType.InvokeCount)};");
                mainSource.AppendLine($"{MethodBodyIntention}long {GetMonitorFieldName(m, MonitoringFieldType.InvokeCompletedCount)};");
                mainSource.AppendLine($"{MethodBodyIntention}int {GetMonitorFieldName(m, MonitoringFieldType.ExceptionCount)};");
                mainSource.AppendLine($"{MethodBodyIntention}long {GetMonitorFieldName(m, MonitoringFieldType.ExecTimeTotal)};");
                mainSource.AppendLine($"{MethodBodyIntention}long {GetMonitorFieldName(m, MonitoringFieldType.ExecTimeMax)};");

                mainSource.AppendLine();
            }


            mainSource.AppendLine();

            // GetCallMonitoringSnapshot method
            mainSource.AppendLine($"{MethodBodyIntention}public Type MonitoringInterface => typeof({interfaceType.Name});");
            mainSource.AppendLine($"{MethodBodyIntention}public Exception LastException => this.lastException;");
            mainSource.AppendLine();

            mainSource.Append(MethodBodyIntention);
            mainSource.AppendLine("public IEnumerable<(string MethodName, long InvokeCount, long InvokeCompletedCount, int ExceptionCount, long ExecTimeTotal, long ExecTimeMax)> GetCallMonitoringSnapshot()");
            mainSource.AppendLine($"{MethodBodyIntention}{{");
            foreach (var m in methods)
            {
                mainSource.AppendLine($"{MethodLineIntention}yield return ({GetMonitorFieldName(m, MonitoringFieldType.MethodName)}, {GetMonitorFieldName(m, MonitoringFieldType.InvokeCount)}, {GetMonitorFieldName(m, MonitoringFieldType.InvokeCompletedCount)}, {GetMonitorFieldName(m, MonitoringFieldType.ExceptionCount)}, {GetMonitorFieldName(m, MonitoringFieldType.ExecTimeTotal)}, {GetMonitorFieldName(m, MonitoringFieldType.ExecTimeMax)});");
            }
            mainSource.AppendLine($"{MethodBodyIntention}}}");
        }


        string GetMonitorFieldName(IMethodSymbol method, MonitoringFieldType fieldType) => $"{FirstToLower(GetUniqueMethodName(method))}_{fieldType}";
    }
}
