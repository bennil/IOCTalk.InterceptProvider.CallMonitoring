cd ..
dotnet build -c Release -p:CodeGen=true

cd IOCTalk.InterceptProvider.CallMonitoring.Insight
dotnet pack -c Release --no-build -p:NuspecFile=ioctalk-interception-callmonitoring-insight.nuspec