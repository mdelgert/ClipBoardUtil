docker build -t clipboardutil-webapp1 .
docker run -d -p 5024:8080 --name test1 clipboardutil-webapp1
docker run -d -p 5025:8080 --name test2 clipboardutil-webapp1

dotnet run --launch-profile "Dev"
dotnet run --launch-profile "Stag"
dotnet run --launch-profile "Prod"

http://localhost:5024/swagger/index.html

