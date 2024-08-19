dotnet run --launch-profile "Development"
dotnet run --launch-profile "Staging"
dotnet run --launch-profile "Production"

http://localhost:5024/swagger/index.html

docker build -t clipboardutil-webapp1 .
docker run -d -p 5024:8080 --name test1 clipboardutil-webapp1
docker run -d -p 5025:8080 --name test2 clipboardutil-webapp1