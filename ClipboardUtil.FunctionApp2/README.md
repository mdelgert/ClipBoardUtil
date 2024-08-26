# Links
https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows&pivots=dotnet-8-0
https://learn.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=node-v4%2Cpython-v2%2Cisolated-process%2Cquick-create&pivots=programming-language-csharp
https://stackoverflow.com/questions/58765236/how-to-build-dockerfile-created-by-visual-studio-2019-from-command-line

# Does not support or run on raspberry pi ARM processor
docker build -t clipboardutilfunctionapp2 -f Dockerfile ..
docker run -d -p 8080:80 --name test1 clipboardutilfunctionapp2
docker run -d -p 8081:80 --name test2 clipboardutilfunctionapp2
docker inspect test1 | jq '.[0].NetworkSettings.Ports'
http://localhost:8080/api/swagger/ui
http://localhost:8081/api/swagger/ui