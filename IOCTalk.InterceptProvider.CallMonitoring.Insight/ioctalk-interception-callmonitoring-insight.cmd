cd ..
cd IOCTalk.InterceptProvider.CallMonitoring
dotnet build -c Release -p:CodeGen=true

cd ..
cd IOCTalk.InterceptProvider.CallMonitoring.Insight
dotnet build -c Release -p:CodeGen=true
dotnet pack -c Release --no-build -p:NuspecFile=ioctalk-interception-callmonitoring-insight.nuspec