# IOCTalk.InterceptProvider.CallMonitoring.Insight

IOC-Talk interception source generator for monitoring service calls featuring the IOCTalk.Insight web frontend.

### Integration Sample

```csharp
var localShare = new LocalShareContext("MyInsightTestApp");

localShare.RegisterLocalSharedService<IMyDemoService2, MyDemoService2Implementation>()
          .InterceptWithImplementation(InterceptCallMonitoringProvider<IMyDemoService2>.GetInterceptType());


// ioctalk insight web dashboard registration
localShare.RegisterInsightWebHost("admin", "my-pw");
localShare.MapInterfaceImplementationType<IInsightModule, SessionInfoWebModule>()
          .MapAdditionalMultiImportImplementation<CallMonitoringInsightModule>();

localShare.RegisterLocalSharedService<ICallMonitoringHost, CallMonitoringHost>();
```

