﻿using BSAG.IOCTalk.Common.Interface.Logging;
using BSAG.IOCTalk.Common.Interface.Session;
using BSAG.IOCTalk.Communication.NetTcp.WireFraming;
using BSAG.IOCTalk.Communication.NetTcp;
using BSAG.IOCTalk.Composition;
using BSAG.IOCTalk.Serialization.Json;
using CallMonitoringSourceGen.TestConsole.Interface;
using IOCTalk.Insight.Interface;
using IOCTalk.Insight.SessionInfo;
using IOCTalk.InterceptProvider.CallMonitoring.Common;
using IOCTalk.InterceptProvider.CallMonitoring.Insight;
using IOCTalk.Insight.WebHost;

namespace CallMonitoringSourceGen.TestConsoleInsightProjectRef
{
    internal class Program
    {
        static LocalShareContext localShare;

        static void Main(string[] args)
        {
            Console.WriteLine("Start ioctalk insight codegen test");

            localShare = new LocalShareContext("InsightTestApp");


            var tcpMyService = new TcpCommunicationController(new ShortWireFraming(), new JsonMessageSerializer());
            var compositionHostService = new TalkCompositionHost(localShare, "MyService");
            compositionHostService.RegisterAutoGeneratedProxyInterfaceMappings();
            compositionHostService.RegisterLocalSessionService<IMyDemoService3, MyDemoService3Implementation>()
                                  .InterceptWithImplementation(InterceptCallMonitoringProvider<IMyDemoService3>.GetInterceptType())
                                  ;


            var tcpMyClient = new TcpCommunicationController(new ShortWireFraming(), new JsonMessageSerializer());
            var compositionHostClient = new TalkCompositionHost(localShare, "MyClient");
            compositionHostClient.RegisterAutoGeneratedProxyInterfaceMappings();

            compositionHostClient.RegisterRemoteService<IMyDemoService3>()
                                 .InterceptWithImplementation(InterceptCallMonitoringProvider<IMyDemoService3>.GetInterceptType())
                                 ;

            localShare.RegisterLocalSharedService<IMyTestService1, MyTestService1Implementation>()
               .InterceptWithImplementation(InterceptCallMonitoringProvider<IMyTestService1>.GetInterceptType())
               ;

            localShare.RegisterLocalSharedService<IMyDemoService2, MyDemoService2Implementation>()
                      .InterceptWithImplementation(InterceptCallMonitoringProvider<IMyDemoService2>.GetInterceptType())
                      ;



            // ioctalk insight web dashboard registration
            localShare.RegisterInsightWebHost("admin", "my-pw");
            localShare.MapInterfaceImplementationType<IInsightModule, SessionInfoWebModule>()
               .MapAdditionalMultiImportImplementation<CallMonitoringInsightModule>();

            localShare.RegisterLocalSharedService<ICallMonitoringHost, CallMonitoringHost>();


            compositionHostClient.SessionCreated += OnCompositionHostClient_SessionCreated;

            compositionHostService.InitGenericCommunication(tcpMyService);

            compositionHostClient.InitGenericCommunication(tcpMyClient);

            // bind to tcp port 14341
            tcpMyService.InitService(14341);


            tcpMyClient.InitClient("127.0.0.1", 14341);

            Console.WriteLine("Running");
            Console.ReadLine();
            Console.WriteLine("stopping...");

            tcpMyService.Shutdown();
        }

        private static void OnCompositionHostClient_SessionCreated(object contractSession, BSAG.IOCTalk.Common.Session.SessionEventArgs e)
        {
            var log = localShare.GetExport<ILogger>();
            var myTestService = localShare.GetExport<IMyTestService1>();
            var demo2Service = localShare.GetExport<IMyDemoService2>();

            var demo3Service = e.SessionContract.GetSessionInstance<IMyDemoService3>();

            Task.Run(() => PeriodicFakeCaller(log, myTestService, demo2Service, demo3Service, e.Session));
        }


        static async Task PeriodicFakeCaller(ILogger log, IMyTestService1 testService1, IMyDemoService2 demo2Service, IMyDemoService3 demo3Service, ISession session)
        {
            try
            {
                log.Info("Start periodic fake calls...");

                int count = 0;
                while (session.IsActive)
                {
                    count++;

                    try
                    {
                        testService1.SayHello(count);

                        if ((count % 10) == 0)
                        {
                            await demo2Service.ExecuteAsync("long-running");
                        }

                        if ((count % 17) == 0)
                        {
                            await demo2Service.ExecuteAsync("throw-exception");
                        }

                        if ((count % 5) == 0)
                        {
                            demo3Service.DemoMethod3(count);
                        }
                    }
                    catch (Exception exInner)
                    {
                        log.Warn(exInner.ToString());
                    }

                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            finally
            {
                log.Info("Periodic fake caller stopped");
            }
        }
    }
}
